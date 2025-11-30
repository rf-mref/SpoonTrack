using SpoonTrack.Views;

namespace SpoonTrack
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            // Set dark theme
            UserAppTheme = AppTheme.Dark;

            // Shell navigation
            MainPage = new AppShell();
        }
    }
}
