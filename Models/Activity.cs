using System;

namespace SpoonTrack.Models
{
    public class Activity
    {
        public int Id { get; set; }
        public int DailyEntryId { get; set; }
        public string ActivityName { get; set; } = string.Empty;
        public int Duration { get; set; } // minutos
        public int EnergyImpact { get; set; } // -10 (drena muito) a +10 (restaura muito)
        public DateTime Timestamp { get; set; } = DateTime.Now;
        
        // Navigation
        public DailyEntry? DailyEntry { get; set; }
    }
    
    // Atividades comuns
    public static class ActivityTypes
    {
        public const string Shower = "Shower";
        public const string Cooking = "Cooking";
        public const string Walking = "Walking";
        public const string SocialEvent = "SocialEvent";
        public const string Work = "Work";
        public const string Rest = "Rest";
        public const string LightExercise = "LightExercise";
        public const string Reading = "Reading";
        public const string Screen = "ScreenTime";
        public const string Errands = "Errands";
    }
}
