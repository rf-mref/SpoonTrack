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
    public class PlanningViewModel : INotifyPropertyChanged
    {
        private readonly DatabaseService _database;
        private int _todayEnergy;
        private int _spoonsRemaining;

        public PlanningViewModel()
        {
            _database = new DatabaseService();
            
            AddActivityCommand = new Command<string>(async (activity) => await AddActivityAsync(activity));
            ToggleActivityCommand = new Command<PlannedActivity>(async (activity) => await ToggleActivityAsync(activity));
            DeleteActivityCommand = new Command<PlannedActivity>(async (activity) => await DeleteActivityAsync(activity));
            
            _ = LoadDataAsync();
        }

        public int TodayEnergy
        {
            get => _todayEnergy;
            set { _todayEnergy = value; OnPropertyChanged(); CalculateRemaining(); }
        }

        public int SpoonsRemaining
        {
            get => _spoonsRemaining;
            set { _spoonsRemaining = value; OnPropertyChanged(); }
        }

        public ObservableCollection<PlannedActivity> PlannedActivities { get; } = new();
        public ObservableCollection<string> CommonActivityNames { get; } = new();

        public ICommand AddActivityCommand { get; }
        public ICommand ToggleActivityCommand { get; }
        public ICommand DeleteActivityCommand { get; }

        private async Task LoadDataAsync()
        {
            // Carregar energia de hoje
            var today = await _database.GetEntryByDateAsync(DateTime.Today);
            TodayEnergy = today?.EnergyLevel ?? 5;

            // Carregar atividades planeadas
            var activities = await _database.GetPlannedActivitiesAsync(DateTime.Today);
            PlannedActivities.Clear();
            foreach (var activity in activities)
                PlannedActivities.Add(activity);

            // Carregar atividades comuns
            CommonActivityNames.Clear();
            foreach (var (name, _) in CommonActivities.Activities)
                CommonActivityNames.Add(name);

            CalculateRemaining();
        }

        private async Task AddActivityAsync(string activityName)
        {
            var commonActivity = CommonActivities.Activities.FirstOrDefault(a => a.Name == activityName);
            if (commonActivity.Name == null) return;

            var activity = new PlannedActivity
            {
                ActivityName = commonActivity.Name,
                SpoonCost = commonActivity.Cost,
                PlannedDate = DateTime.Today,
                IsCompleted = false
            };

            await _database.SavePlannedActivityAsync(activity);
            await LoadDataAsync();
        }

        private async Task ToggleActivityAsync(PlannedActivity activity)
        {
            activity.IsCompleted = !activity.IsCompleted;
            await _database.TogglePlannedActivityAsync(activity.Id, activity.IsCompleted);
            CalculateRemaining();
        }

        private async Task DeleteActivityAsync(PlannedActivity activity)
        {
            await _database.DeletePlannedActivityAsync(activity.Id);
            PlannedActivities.Remove(activity);
            CalculateRemaining();
        }

        private void CalculateRemaining()
        {
            var spent = PlannedActivities.Where(a => !a.IsCompleted).Sum(a => a.SpoonCost);
            SpoonsRemaining = TodayEnergy - spent;
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        
        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
