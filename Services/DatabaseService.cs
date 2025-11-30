using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;
using SpoonTrack.Models;

namespace SpoonTrack.Services
{
    public class DatabaseService
    {
        private readonly string _dbPath;
        private readonly string _connectionString;

        public DatabaseService()
        {
            var documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            _dbPath = Path.Combine(documentsPath, "spoontrack.db");
            _connectionString = $"Data Source={_dbPath}";
            InitializeDatabase();
        }

        private void InitializeDatabase()
        {
            using var connection = new SqliteConnection(_connectionString);
            connection.Open();

            var command = connection.CreateCommand();
            command.CommandText = @"
                CREATE TABLE IF NOT EXISTS DailyEntries (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    Date TEXT NOT NULL,
                    EnergyLevel INTEGER NOT NULL,
                    SleepQuality INTEGER NOT NULL,
                    Notes TEXT
                );

                CREATE TABLE IF NOT EXISTS Symptoms (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    DailyEntryId INTEGER NOT NULL,
                    SymptomType TEXT NOT NULL,
                    Severity INTEGER NOT NULL,
                    Timestamp TEXT NOT NULL,
                    FOREIGN KEY (DailyEntryId) REFERENCES DailyEntries(Id) ON DELETE CASCADE
                );

                CREATE TABLE IF NOT EXISTS Activities (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    DailyEntryId INTEGER NOT NULL,
                    ActivityName TEXT NOT NULL,
                    Duration INTEGER NOT NULL,
                    EnergyImpact INTEGER NOT NULL,
                    Timestamp TEXT NOT NULL,
                    FOREIGN KEY (DailyEntryId) REFERENCES DailyEntries(Id) ON DELETE CASCADE
                );

                CREATE TABLE IF NOT EXISTS Triggers (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    DailyEntryId INTEGER NOT NULL,
                    TriggerType TEXT NOT NULL,
                    Notes TEXT,
                    Timestamp TEXT NOT NULL,
                    FOREIGN KEY (DailyEntryId) REFERENCES DailyEntries(Id) ON DELETE CASCADE
                );

                CREATE TABLE IF NOT EXISTS PlannedActivities (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    ActivityName TEXT NOT NULL,
                    SpoonCost INTEGER NOT NULL,
                    PlannedDate TEXT NOT NULL,
                    IsCompleted INTEGER NOT NULL DEFAULT 0,
                    Notes TEXT
                );

                CREATE INDEX IF NOT EXISTS idx_daily_date ON DailyEntries(Date);
                CREATE INDEX IF NOT EXISTS idx_symptoms_entry ON Symptoms(DailyEntryId);
                CREATE INDEX IF NOT EXISTS idx_activities_entry ON Activities(DailyEntryId);
                CREATE INDEX IF NOT EXISTS idx_triggers_entry ON Triggers(DailyEntryId);
                CREATE INDEX IF NOT EXISTS idx_planned_date ON PlannedActivities(PlannedDate);
            ";
            command.ExecuteNonQuery();
        }

        // DailyEntry CRUD
        public async Task<int> SaveDailyEntryAsync(DailyEntry entry)
        {
            using var connection = new SqliteConnection(_connectionString);
            await connection.OpenAsync();

            var command = connection.CreateCommand();
            if (entry.Id == 0)
            {
                command.CommandText = @"
                    INSERT INTO DailyEntries (Date, EnergyLevel, SleepQuality, Notes)
                    VALUES (@date, @energy, @sleep, @notes);
                    SELECT last_insert_rowid();
                ";
            }
            else
            {
                command.CommandText = @"
                    UPDATE DailyEntries 
                    SET Date = @date, EnergyLevel = @energy, SleepQuality = @sleep, Notes = @notes
                    WHERE Id = @id;
                    SELECT @id;
                ";
                command.Parameters.AddWithValue("@id", entry.Id);
            }

            command.Parameters.AddWithValue("@date", entry.Date.ToString("o"));
            command.Parameters.AddWithValue("@energy", entry.EnergyLevel);
            command.Parameters.AddWithValue("@sleep", entry.SleepQuality);
            command.Parameters.AddWithValue("@notes", entry.Notes ?? (object)DBNull.Value);

            var result = await command.ExecuteScalarAsync();
            return Convert.ToInt32(result);
        }

        public async Task<DailyEntry?> GetDailyEntryAsync(int id)
        {
            using var connection = new SqliteConnection(_connectionString);
            await connection.OpenAsync();

            var command = connection.CreateCommand();
            command.CommandText = "SELECT * FROM DailyEntries WHERE Id = @id";
            command.Parameters.AddWithValue("@id", id);

            using var reader = await command.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                var entry = new DailyEntry
                {
                    Id = reader.GetInt32(0),
                    Date = DateTime.Parse(reader.GetString(1)),
                    EnergyLevel = reader.GetInt32(2),
                    SleepQuality = reader.GetInt32(3),
                    Notes = reader.IsDBNull(4) ? null : reader.GetString(4)
                };

                // Load related data
                entry.Symptoms = await GetSymptomsForEntryAsync(id);
                entry.Activities = await GetActivitiesForEntryAsync(id);
                entry.Triggers = await GetTriggersForEntryAsync(id);

                return entry;
            }

            return null;
        }

        public async Task<List<DailyEntry>> GetAllDailyEntriesAsync(int limit = 30)
        {
            using var connection = new SqliteConnection(_connectionString);
            await connection.OpenAsync();

            var command = connection.CreateCommand();
            command.CommandText = "SELECT * FROM DailyEntries ORDER BY Date DESC LIMIT @limit";
            command.Parameters.AddWithValue("@limit", limit);

            var entries = new List<DailyEntry>();
            using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                var entry = new DailyEntry
                {
                    Id = reader.GetInt32(0),
                    Date = DateTime.Parse(reader.GetString(1)),
                    EnergyLevel = reader.GetInt32(2),
                    SleepQuality = reader.GetInt32(3),
                    Notes = reader.IsDBNull(4) ? null : reader.GetString(4)
                };
                entries.Add(entry);
            }

            // Load related data for each
            foreach (var entry in entries)
            {
                entry.Symptoms = await GetSymptomsForEntryAsync(entry.Id);
                entry.Activities = await GetActivitiesForEntryAsync(entry.Id);
                entry.Triggers = await GetTriggersForEntryAsync(entry.Id);
            }

            return entries;
        }

        public async Task<DailyEntry?> GetEntryByDateAsync(DateTime date)
        {
            using var connection = new SqliteConnection(_connectionString);
            await connection.OpenAsync();

            var command = connection.CreateCommand();
            command.CommandText = "SELECT * FROM DailyEntries WHERE DATE(Date) = DATE(@date)";
            command.Parameters.AddWithValue("@date", date.ToString("o"));

            using var reader = await command.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                var entry = new DailyEntry
                {
                    Id = reader.GetInt32(0),
                    Date = DateTime.Parse(reader.GetString(1)),
                    EnergyLevel = reader.GetInt32(2),
                    SleepQuality = reader.GetInt32(3),
                    Notes = reader.IsDBNull(4) ? null : reader.GetString(4)
                };

                entry.Symptoms = await GetSymptomsForEntryAsync(entry.Id);
                entry.Activities = await GetActivitiesForEntryAsync(entry.Id);
                entry.Triggers = await GetTriggersForEntryAsync(entry.Id);

                return entry;
            }

            return null;
        }

        public async Task DeleteDailyEntryAsync(int id)
        {
            using var connection = new SqliteConnection(_connectionString);
            await connection.OpenAsync();

            var command = connection.CreateCommand();
            command.CommandText = "DELETE FROM DailyEntries WHERE Id = @id";
            command.Parameters.AddWithValue("@id", id);
            await command.ExecuteNonQueryAsync();
        }

        // Symptom CRUD
        public async Task SaveSymptomAsync(Symptom symptom)
        {
            using var connection = new SqliteConnection(_connectionString);
            await connection.OpenAsync();

            var command = connection.CreateCommand();
            command.CommandText = @"
                INSERT INTO Symptoms (DailyEntryId, SymptomType, Severity, Timestamp)
                VALUES (@entryId, @type, @severity, @timestamp)
            ";
            command.Parameters.AddWithValue("@entryId", symptom.DailyEntryId);
            command.Parameters.AddWithValue("@type", symptom.SymptomType);
            command.Parameters.AddWithValue("@severity", symptom.Severity);
            command.Parameters.AddWithValue("@timestamp", symptom.Timestamp.ToString("o"));

            await command.ExecuteNonQueryAsync();
        }

        private async Task<List<Symptom>> GetSymptomsForEntryAsync(int entryId)
        {
            using var connection = new SqliteConnection(_connectionString);
            await connection.OpenAsync();

            var command = connection.CreateCommand();
            command.CommandText = "SELECT * FROM Symptoms WHERE DailyEntryId = @id";
            command.Parameters.AddWithValue("@id", entryId);

            var symptoms = new List<Symptom>();
            using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                symptoms.Add(new Symptom
                {
                    Id = reader.GetInt32(0),
                    DailyEntryId = reader.GetInt32(1),
                    SymptomType = reader.GetString(2),
                    Severity = reader.GetInt32(3),
                    Timestamp = DateTime.Parse(reader.GetString(4))
                });
            }

            return symptoms;
        }

        // Activity CRUD
        public async Task SaveActivityAsync(Activity activity)
        {
            using var connection = new SqliteConnection(_connectionString);
            await connection.OpenAsync();

            var command = connection.CreateCommand();
            command.CommandText = @"
                INSERT INTO Activities (DailyEntryId, ActivityName, Duration, EnergyImpact, Timestamp)
                VALUES (@entryId, @name, @duration, @impact, @timestamp)
            ";
            command.Parameters.AddWithValue("@entryId", activity.DailyEntryId);
            command.Parameters.AddWithValue("@name", activity.ActivityName);
            command.Parameters.AddWithValue("@duration", activity.Duration);
            command.Parameters.AddWithValue("@impact", activity.EnergyImpact);
            command.Parameters.AddWithValue("@timestamp", activity.Timestamp.ToString("o"));

            await command.ExecuteNonQueryAsync();
        }

        private async Task<List<Activity>> GetActivitiesForEntryAsync(int entryId)
        {
            using var connection = new SqliteConnection(_connectionString);
            await connection.OpenAsync();

            var command = connection.CreateCommand();
            command.CommandText = "SELECT * FROM Activities WHERE DailyEntryId = @id";
            command.Parameters.AddWithValue("@id", entryId);

            var activities = new List<Activity>();
            using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                activities.Add(new Activity
                {
                    Id = reader.GetInt32(0),
                    DailyEntryId = reader.GetInt32(1),
                    ActivityName = reader.GetString(2),
                    Duration = reader.GetInt32(3),
                    EnergyImpact = reader.GetInt32(4),
                    Timestamp = DateTime.Parse(reader.GetString(5))
                });
            }

            return activities;
        }

        // Trigger CRUD
        public async Task SaveTriggerAsync(SymptomTrigger trigger)
        {
            using var connection = new SqliteConnection(_connectionString);
            await connection.OpenAsync();

            var command = connection.CreateCommand();
            command.CommandText = @"
                INSERT INTO Triggers (DailyEntryId, TriggerType, Notes, Timestamp)
                VALUES (@entryId, @type, @notes, @timestamp)
            ";
            command.Parameters.AddWithValue("@entryId", trigger.DailyEntryId);
            command.Parameters.AddWithValue("@type", trigger.TriggerType);
            command.Parameters.AddWithValue("@notes", trigger.Notes ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@timestamp", trigger.Timestamp.ToString("o"));

            await command.ExecuteNonQueryAsync();
        }

        private async Task<List<SymptomTrigger>> GetTriggersForEntryAsync(int entryId)
        {
            using var connection = new SqliteConnection(_connectionString);
            await connection.OpenAsync();

            var command = connection.CreateCommand();
            command.CommandText = "SELECT * FROM Triggers WHERE DailyEntryId = @id";
            command.Parameters.AddWithValue("@id", entryId);

            var triggers = new List<SymptomTrigger>();
            using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                triggers.Add(new SymptomTrigger
                {
                    Id = reader.GetInt32(0),
                    DailyEntryId = reader.GetInt32(1),
                    TriggerType = reader.GetString(2),
                    Notes = reader.IsDBNull(3) ? null : reader.GetString(3),
                    Timestamp = DateTime.Parse(reader.GetString(4))
                });
            }

            return triggers;
        }

        // Analytics
        public async Task<double> GetAverageEnergyLast7DaysAsync()
        {
            using var connection = new SqliteConnection(_connectionString);
            await connection.OpenAsync();

            var command = connection.CreateCommand();
            command.CommandText = @"
                SELECT AVG(EnergyLevel) 
                FROM DailyEntries 
                WHERE Date >= DATE('now', '-7 days')
            ";

            var result = await command.ExecuteScalarAsync();
            return result == DBNull.Value ? 0 : Convert.ToDouble(result);
        }

        public async Task<Dictionary<string, int>> GetSymptomFrequencyAsync(int days = 30)
        {
            using var connection = new SqliteConnection(_connectionString);
            await connection.OpenAsync();

            var command = connection.CreateCommand();
            command.CommandText = @"
                SELECT s.SymptomType, COUNT(*) as Frequency
                FROM Symptoms s
                INNER JOIN DailyEntries de ON s.DailyEntryId = de.Id
                WHERE de.Date >= DATE('now', @days || ' days')
                GROUP BY s.SymptomType
                ORDER BY Frequency DESC
            ";
            command.Parameters.AddWithValue("@days", -days);

            var frequency = new Dictionary<string, int>();
            using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                frequency[reader.GetString(0)] = reader.GetInt32(1);
            }

            return frequency;
        }

        public string GetDatabasePath() => _dbPath;

        // PlannedActivity CRUD
        public async Task SavePlannedActivityAsync(PlannedActivity activity)
        {
            using var connection = new SqliteConnection(_connectionString);
            await connection.OpenAsync();

            var command = connection.CreateCommand();
            command.CommandText = @"
                INSERT INTO PlannedActivities (ActivityName, SpoonCost, PlannedDate, IsCompleted, Notes)
                VALUES (@name, @cost, @date, @completed, @notes)
            ";
            command.Parameters.AddWithValue("@name", activity.ActivityName);
            command.Parameters.AddWithValue("@cost", activity.SpoonCost);
            command.Parameters.AddWithValue("@date", activity.PlannedDate.ToString("o"));
            command.Parameters.AddWithValue("@completed", activity.IsCompleted ? 1 : 0);
            command.Parameters.AddWithValue("@notes", activity.Notes ?? (object)DBNull.Value);

            await command.ExecuteNonQueryAsync();
        }

        public async Task<List<PlannedActivity>> GetPlannedActivitiesAsync(DateTime date)
        {
            using var connection = new SqliteConnection(_connectionString);
            await connection.OpenAsync();

            var command = connection.CreateCommand();
            command.CommandText = "SELECT * FROM PlannedActivities WHERE DATE(PlannedDate) = DATE(@date) ORDER BY Id";
            command.Parameters.AddWithValue("@date", date.ToString("o"));

            var activities = new List<PlannedActivity>();
            using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                activities.Add(new PlannedActivity
                {
                    Id = reader.GetInt32(0),
                    ActivityName = reader.GetString(1),
                    SpoonCost = reader.GetInt32(2),
                    PlannedDate = DateTime.Parse(reader.GetString(3)),
                    IsCompleted = reader.GetInt32(4) == 1,
                    Notes = reader.IsDBNull(5) ? null : reader.GetString(5)
                });
            }

            return activities;
        }

        public async Task TogglePlannedActivityAsync(int id, bool completed)
        {
            using var connection = new SqliteConnection(_connectionString);
            await connection.OpenAsync();

            var command = connection.CreateCommand();
            command.CommandText = "UPDATE PlannedActivities SET IsCompleted = @completed WHERE Id = @id";
            command.Parameters.AddWithValue("@id", id);
            command.Parameters.AddWithValue("@completed", completed ? 1 : 0);

            await command.ExecuteNonQueryAsync();
        }

        public async Task DeletePlannedActivityAsync(int id)
        {
            using var connection = new SqliteConnection(_connectionString);
            await connection.OpenAsync();

            var command = connection.CreateCommand();
            command.CommandText = "DELETE FROM PlannedActivities WHERE Id = @id";
            command.Parameters.AddWithValue("@id", id);

            await command.ExecuteNonQueryAsync();
        }
    }
}
