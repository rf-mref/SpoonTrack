using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using SpoonTrack.Models;
using SpoonTrack.Services;

namespace SpoonTrack.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private readonly DatabaseService _database;
        private DailyEntry? _todayEntry;
        private int _currentEnergy;
        private int _currentSleep;
        private string _notes = string.Empty;
        private double _averageEnergy7Days;

        public MainViewModel()
        {
            _database = new DatabaseService();
            
            SaveTodayCommand = new Command(async () => await SaveTodayEntryAsync());
            QuickSymptomCommand = new Command<string>(async (symptom) => await AddQuickSymptomAsync(symptom));
            
            _ = LoadTodayDataAsync();
        }

        // Properties
        public int CurrentEnergy
        {
            get => _currentEnergy;
            set { _currentEnergy = value; OnPropertyChanged(); }
        }

        public int CurrentSleep
        {
            get => _currentSleep;
            set { _currentSleep = value; OnPropertyChanged(); }
        }

        public string Notes
        {
            get => _notes;
            set { _notes = value; OnPropertyChanged(); }
        }

        public double AverageEnergy7Days
        {
            get => _averageEnergy7Days;
            set { _averageEnergy7Days = value; OnPropertyChanged(); }
        }

        public ObservableCollection<Symptom> TodaySymptoms { get; } = new();
        public ObservableCollection<Activity> TodayActivities { get; } = new();

        // Commands
        public ICommand SaveTodayCommand { get; }
        public ICommand QuickSymptomCommand { get; }

        // Methods
        private async Task LoadTodayDataAsync()
        {
            _todayEntry = await _database.GetEntryByDateAsync(DateTime.Today);
            
            if (_todayEntry != null)
            {
                CurrentEnergy = _todayEntry.EnergyLevel;
                CurrentSleep = _todayEntry.SleepQuality;
                Notes = _todayEntry.Notes ?? string.Empty;
                
                TodaySymptoms.Clear();
                foreach (var s in _todayEntry.Symptoms)
                    TodaySymptoms.Add(s);
                
                TodayActivities.Clear();
                foreach (var a in _todayEntry.Activities)
                    TodayActivities.Add(a);
            }
            else
            {
                CurrentEnergy = 5;
                CurrentSleep = 5;
            }

            AverageEnergy7Days = await _database.GetAverageEnergyLast7DaysAsync();
        }

        private async Task SaveTodayEntryAsync()
        {
            var entry = _todayEntry ?? new DailyEntry { Date = DateTime.Today };
            
            entry.EnergyLevel = CurrentEnergy;
            entry.SleepQuality = CurrentSleep;
            entry.Notes = Notes;

            var entryId = await _database.SaveDailyEntryAsync(entry);
            
            if (_todayEntry == null)
            {
                _todayEntry = await _database.GetDailyEntryAsync(entryId);
            }

            // Aviso energia baixa
            if (CurrentEnergy <= 4)
            {
                await Application.Current.MainPage.DisplayAlert(
                    "⚠️ Energia Baixa!", 
                    $"Só tens {CurrentEnergy} spoons hoje. Considera descansar e evitar atividades pesadas para prevenir crash (PEM).",
                    "OK");
            }
        }

        private async Task AddQuickSymptomAsync(string symptomType)
        {
            if (_todayEntry == null)
                await SaveTodayEntryAsync();

            if (_todayEntry != null)
            {
                var symptom = new Symptom
                {
                    DailyEntryId = _todayEntry.Id,
                    SymptomType = symptomType,
                    Severity = 5, // Default médio
                    Timestamp = DateTime.Now
                };

                await _database.SaveSymptomAsync(symptom);
                TodaySymptoms.Add(symptom);
            }
        }

        public async Task AddActivityAsync(string activityName, int duration, int impact)
        {
            if (_todayEntry == null)
                await SaveTodayEntryAsync();

            if (_todayEntry != null)
            {
                var activity = new Activity
                {
                    DailyEntryId = _todayEntry.Id,
                    ActivityName = activityName,
                    Duration = duration,
                    EnergyImpact = impact,
                    Timestamp = DateTime.Now
                };

                await _database.SaveActivityAsync(activity);
                TodayActivities.Add(activity);
            }
        }

        // INotifyPropertyChanged
        public event PropertyChangedEventHandler? PropertyChanged;
        
        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    // Simple Command implementation
    public class Command : ICommand
    {
        private readonly Action _execute;
        private readonly Func<bool>? _canExecute;

        public Command(Action execute, Func<bool>? canExecute = null)
        {
            _execute = execute;
            _canExecute = canExecute;
        }

        public bool CanExecute(object? parameter) => _canExecute?.Invoke() ?? true;
        
        public void Execute(object? parameter) => _execute();

        public event EventHandler? CanExecuteChanged;
    }

    public class Command<T> : ICommand
    {
        private readonly Action<T> _execute;
        private readonly Func<T, bool>? _canExecute;

        public Command(Action<T> execute, Func<T, bool>? canExecute = null)
        {
            _execute = execute;
            _canExecute = canExecute;
        }

        public bool CanExecute(object? parameter) => 
            parameter is T t && (_canExecute?.Invoke(t) ?? true);
        
        public void Execute(object? parameter)
        {
            if (parameter is T t)
                _execute(t);
        }

        public event EventHandler? CanExecuteChanged;
    }
}
