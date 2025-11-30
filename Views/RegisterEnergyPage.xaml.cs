using SpoonTrack.Models;
using SpoonTrack.Services;

namespace SpoonTrack.Views
{
    public partial class RegisterEnergyPage : ContentPage
    {
        private readonly DatabaseService _database;
        
        private readonly string[] _energyDescriptions = 
        {
            "Extremely low - bedridden",
            "Very low - minimal activity",
            "Low - basic self-care only",
            "Below average - limited activity",
            "Moderate low - some activities",
            "Moderate - normal with breaks",
            "Moderate high - most activities ok",
            "Good - active day possible",
            "Very good - very productive",
            "Excellent - peak energy"
        };

        public RegisterEnergyPage()
        {
            InitializeComponent();
            _database = new DatabaseService();
            
            UpdateEnergyDisplay((int)EnergySlider.Value);
        }

        private void EnergySlider_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            int value = (int)e.NewValue;
            UpdateEnergyDisplay(value);
        }

        private void UpdateEnergyDisplay(int value)
        {
            SpoonsLabel.Text = $"🥄 {value}/10 spoons";
            
            if (value >= 1 && value <= 10)
            {
                EnergyDescription.Text = _energyDescriptions[value - 1];
            }
        }

        private void SleepSlider_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            int value = (int)e.NewValue;
            SleepLabel.Text = $"😴 {value}/10";
        }

        private async void SaveButton_Clicked(object sender, EventArgs e)
        {
            try
            {
                // Create daily entry
                var entry = new DailyEntry
                {
                    Date = DateTime.Today,
                    EnergyLevel = (int)EnergySlider.Value,
                    SleepQuality = (int)SleepSlider.Value,
                    Notes = NotesEditor.Text
                };

                var entryId = await _database.SaveDailyEntryAsync(entry);

                // Save checked symptoms
                if (FatigueCheck.IsChecked)
                {
                    await _database.SaveSymptomAsync(new Symptom
                    {
                        DailyEntryId = entryId,
                        SymptomType = SymptomTypes.Fatigue,
                        Severity = (int)FatigueSeverity.Value
                    });
                }

                if (PainCheck.IsChecked)
                {
                    await _database.SaveSymptomAsync(new Symptom
                    {
                        DailyEntryId = entryId,
                        SymptomType = SymptomTypes.Pain,
                        Severity = (int)PainSeverity.Value
                    });
                }

                if (BrainFogCheck.IsChecked)
                {
                    await _database.SaveSymptomAsync(new Symptom
                    {
                        DailyEntryId = entryId,
                        SymptomType = SymptomTypes.BrainFog,
                        Severity = (int)BrainFogSeverity.Value
                    });
                }

                await DisplayAlert("✅ Saved", "Entry saved successfully!", "OK");
                
                // Navigate back
                await Shell.Current.GoToAsync("..");
            }
            catch (Exception ex)
            {
                await DisplayAlert("❌ Error", $"Failed to save: {ex.Message}", "OK");
            }
        }
    }
}
