using System;

namespace SpoonTrack.Models
{
    public class Symptom
    {
        public int Id { get; set; }
        public int DailyEntryId { get; set; }
        public string SymptomType { get; set; } = string.Empty; // Fatigue, Pain, BrainFog, Dizziness, Nausea, PEM, etc
        public int Severity { get; set; } // 1-10
        public DateTime Timestamp { get; set; } = DateTime.Now;
        
        // Navigation
        public DailyEntry? DailyEntry { get; set; }
    }
    
    // Sintomas comuns predefinidos
    public static class SymptomTypes
    {
        public const string Fatigue = "Fatigue";
        public const string Pain = "Pain";
        public const string BrainFog = "BrainFog";
        public const string Dizziness = "Dizziness";
        public const string Nausea = "Nausea";
        public const string PEM = "PEM"; // Post-Exertional Malaise
        public const string Headache = "Headache";
        public const string SoreThroat = "SoreThroat";
        public const string Insomnia = "Insomnia";
        public const string MuscleWeakness = "MuscleWeakness";
    }
}
