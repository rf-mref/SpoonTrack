using System;
using System.Collections.Generic;

namespace SpoonTrack.Models
{
    public class DailyEntry
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public int EnergyLevel { get; set; } // 1-10 spoons
        public int SleepQuality { get; set; } // 1-10
        public string? Notes { get; set; }
        
        // Navigation properties
        public List<Symptom> Symptoms { get; set; } = new();
        public List<Activity> Activities { get; set; } = new();
        public List<SymptomTrigger> Triggers { get; set; } = new();
    }
}
