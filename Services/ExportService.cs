using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpoonTrack.Models;

namespace SpoonTrack.Services
{
    public class ExportService
    {
        private readonly DatabaseService _database;

        public ExportService(DatabaseService database)
        {
            _database = database;
        }

        public async Task<string> ExportToCsvAsync(List<DailyEntry> entries)
        {
            var csv = new StringBuilder();
            
            // Header
            csv.AppendLine("Date,EnergyLevel,SleepQuality,Notes,Symptoms,Activities,Triggers");

            foreach (var entry in entries)
            {
                var symptoms = string.Join("; ", entry.Symptoms.Select(s => $"{s.SymptomType}({s.Severity})"));
                var activities = string.Join("; ", entry.Activities.Select(a => $"{a.ActivityName}({a.Duration}min,{a.EnergyImpact})"));
                var triggers = string.Join("; ", entry.Triggers.Select(t => t.TriggerType));

                csv.AppendLine($"{entry.Date:yyyy-MM-dd}," +
                              $"{entry.EnergyLevel}," +
                              $"{entry.SleepQuality}," +
                              $"\"{entry.Notes?.Replace("\"", "\"\"")}\"," +
                              $"\"{symptoms}\"," +
                              $"\"{activities}\"," +
                              $"\"{triggers}\"");
            }

            var documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            var fileName = $"SpoonTrack_Export_{DateTime.Now:yyyyMMdd_HHmmss}.csv";
            var filePath = Path.Combine(documentsPath, fileName);

            await File.WriteAllTextAsync(filePath, csv.ToString(), Encoding.UTF8);
            return filePath;
        }

        public async Task<string> CreateBackupAsync()
        {
            var documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            var backupFolder = Path.Combine(documentsPath, "SpoonTrack_Backups");
            Directory.CreateDirectory(backupFolder);

            var timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
            var zipPath = Path.Combine(backupFolder, $"SpoonTrack_Backup_{timestamp}.zip");

            using (var archive = ZipFile.Open(zipPath, ZipArchiveMode.Create))
            {
                // Add database
                var dbPath = _database.GetDatabasePath();
                if (File.Exists(dbPath))
                {
                    archive.CreateEntryFromFile(dbPath, "spoontrack.db");
                }

                // Export CSV for easy viewing
                var entries = await _database.GetAllDailyEntriesAsync(999);
                var csvContent = await ExportToCsvContentAsync(entries);
                
                var csvEntry = archive.CreateEntry("export.csv");
                using (var writer = new StreamWriter(csvEntry.Open()))
                {
                    await writer.WriteAsync(csvContent);
                }
            }

            return zipPath;
        }

        private async Task<string> ExportToCsvContentAsync(List<DailyEntry> entries)
        {
            var csv = new StringBuilder();
            csv.AppendLine("Date,EnergyLevel,SleepQuality,Notes,Symptoms,Activities,Triggers");

            foreach (var entry in entries)
            {
                var symptoms = string.Join("; ", entry.Symptoms.Select(s => $"{s.SymptomType}({s.Severity})"));
                var activities = string.Join("; ", entry.Activities.Select(a => $"{a.ActivityName}({a.Duration}min,{a.EnergyImpact})"));
                var triggers = string.Join("; ", entry.Triggers.Select(t => t.TriggerType));

                csv.AppendLine($"{entry.Date:yyyy-MM-dd}," +
                              $"{entry.EnergyLevel}," +
                              $"{entry.SleepQuality}," +
                              $"\"{entry.Notes?.Replace("\"", "\"\"")}\"," +
                              $"\"{symptoms}\"," +
                              $"\"{activities}\"," +
                              $"\"{triggers}\"");
            }

            return csv.ToString();
        }
    }
}
