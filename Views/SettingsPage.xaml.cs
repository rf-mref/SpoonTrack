namespace SpoonTrack.Views
{
    public partial class SettingsPage : ContentPage
    {
        public SettingsPage()
        {
            InitializeComponent();
            LoadSettings();
        }

        private void LoadSettings()
        {
            // Load saved preferences
            DefaultSpoonsSlider.Value = Preferences.Get("DefaultSpoons", 5);
            var lang = Preferences.Get("Language", "pt");
            LanguagePicker.SelectedIndex = lang switch
            {
                "pt" => 0,
                "en" => 1,
                "fr" => 2,
                _ => 0
            };
            DarkModeSwitch.IsToggled = Preferences.Get("DarkMode", true);
        }

        private void DefaultSpoonsSlider_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            int value = (int)e.NewValue;
            SpoonsLabel.Text = $"{value} spoons";
            Preferences.Set("DefaultSpoons", value);
        }

        private void LanguagePicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            var lang = LanguagePicker.SelectedIndex switch
            {
                0 => "pt",
                1 => "en",
                2 => "fr",
                _ => "pt"
            };
            Preferences.Set("Language", lang);

            DisplayAlert("Language Changed", "Restart app to apply language.", "OK");
        }

        private void DarkModeSwitch_Toggled(object sender, ToggledEventArgs e)
        {
            Preferences.Set("DarkMode", e.Value);
            Application.Current.UserAppTheme = e.Value ? AppTheme.Dark : AppTheme.Light;
        }

        private async void ClearData_Clicked(object sender, EventArgs e)
        {
            bool confirm = await DisplayAlert(
                "⚠️ Clear All Data?",
                "This will delete ALL your entries, symptoms, and activities. This cannot be undone!",
                "Delete Everything",
                "Cancel");

            if (confirm)
            {
                // TODO: Implementar limpar database
                await DisplayAlert("✅ Cleared", "All data deleted.", "OK");
            }
        }
    }
}