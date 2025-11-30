using System;

namespace SpoonTrack.Models
{
    public class SymptomTrigger
    {
        public int Id { get; set; }
        public int DailyEntryId { get; set; }
        public string TriggerType { get; set; } = string.Empty;
        public string? Notes { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.Now;
        
        // Navigation
        public DailyEntry? DailyEntry { get; set; }
    }
    
    // Triggers comuns
    public static class TriggerTypes
    {
        public const string Stress = "Stress";
        public const string Weather = "Weather";
        public const string Food = "Food";
        public const string Overexertion = "Overexertion";
        public const string PoorSleep = "PoorSleep";
        public const string Noise = "Noise";
        public const string Light = "BrightLight";
        public const string Chemical = "Chemical";
        public const string Social = "SocialOverload";
        public const string Travel = "Travel";
    }
}
