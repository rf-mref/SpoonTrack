using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using SpoonTrack.Models;
using SpoonTrack.Services;

namespace SpoonTrack.ViewModels
{
    public class ChartsViewModel : INotifyPropertyChanged
    {
        private readonly DatabaseService _database;

        public ChartsViewModel()
        {
            _database = new DatabaseService();
            _ = LoadChartDataAsync();
        }

        public ObservableCollection<ChartDataPoint> EnergyData { get; } = new();

        private async Task LoadChartDataAsync()
        {
            var entries = await _database.GetAllDailyEntriesAsync(7);

            EnergyData.Clear();
            foreach (var entry in entries.OrderBy(e => e.Date))
            {
                EnergyData.Add(new ChartDataPoint
                {
                    Date = entry.Date,
                    Value = entry.EnergyLevel
                });
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class ChartDataPoint
    {
        public DateTime Date { get; set; }
        public int Value { get; set; }
        public double BarWidth => Value * 20; // 1 spoon = 20px
    }
}