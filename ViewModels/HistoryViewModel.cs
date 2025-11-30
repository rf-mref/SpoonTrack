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
    public class HistoryViewModel : INotifyPropertyChanged
    {
        private readonly DatabaseService _database;
        private readonly ExportService _exportService;
        private DailyEntry? _selectedEntry;
        private string _filterSymptom = string.Empty;

        public HistoryViewModel()
        {
            _database = new DatabaseService();
            _exportService = new ExportService(_database);
            
            LoadHistoryCommand = new Command(async () => await LoadHistoryAsync());
            DeleteEntryCommand = new Command(async () => await DeleteSelectedEntryAsync());
            ExportCsvCommand = new Command(async () => await ExportCsvAsync());
            CreateBackupCommand = new Command(async () => await CreateBackupAsync());
            
            _ = LoadHistoryAsync();
        }

        // Properties
        public ObservableCollection<DailyEntry> Entries { get; } = new();
        
        public DailyEntry? SelectedEntry
        {
            get => _selectedEntry;
            set { _selectedEntry = value; OnPropertyChanged(); }
        }

        public string FilterSymptom
        {
            get => _filterSymptom;
            set 
            { 
                _filterSymptom = value; 
                OnPropertyChanged();
                _ = ApplyFilterAsync();
            }
        }

        // Commands
        public ICommand LoadHistoryCommand { get; }
        public ICommand DeleteEntryCommand { get; }
        public ICommand ExportCsvCommand { get; }
        public ICommand CreateBackupCommand { get; }

        // Methods
        private async Task LoadHistoryAsync()
        {
            var entries = await _database.GetAllDailyEntriesAsync(90); // últimos 90 dias
            
            Entries.Clear();
            foreach (var entry in entries)
                Entries.Add(entry);
        }

        private async Task ApplyFilterAsync()
        {
            var allEntries = await _database.GetAllDailyEntriesAsync(90);
            
            if (!string.IsNullOrWhiteSpace(FilterSymptom))
            {
                allEntries = allEntries.Where(e => 
                    e.Symptoms.Any(s => s.SymptomType.Contains(FilterSymptom, StringComparison.OrdinalIgnoreCase))
                ).ToList();
            }

            Entries.Clear();
            foreach (var entry in allEntries)
                Entries.Add(entry);
        }

        private async Task DeleteSelectedEntryAsync()
        {
            if (SelectedEntry != null)
            {
                await _database.DeleteDailyEntryAsync(SelectedEntry.Id);
                Entries.Remove(SelectedEntry);
                SelectedEntry = null;
            }
        }

        private async Task ExportCsvAsync()
        {
            var entries = Entries.ToList();
            var filePath = await _exportService.ExportToCsvAsync(entries);
            // TODO: mostrar mensagem de sucesso ao utilizador
        }

        private async Task CreateBackupAsync()
        {
            var backupPath = await _exportService.CreateBackupAsync();
            // TODO: mostrar mensagem de sucesso ao utilizador
        }

        // INotifyPropertyChanged
        public event PropertyChangedEventHandler? PropertyChanged;
        
        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
