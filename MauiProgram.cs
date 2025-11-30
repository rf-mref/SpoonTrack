using Microsoft.Extensions.Logging;
using SpoonTrack.Services;
using SpoonTrack.ViewModels;
using SpoonTrack.Views;
using Syncfusion.Maui.Core.Hosting;
using Syncfusion.Maui.Toolkit.Hosting;

namespace SpoonTrack
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("Ngo9BigBOggjHTQxAR8/V1NCaF1cXGNCf1FpRmJGdld5fUVHYVdUTXxaS00DNHVRdkdnWXlceHRXRmhcVEF0X0U=");

            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                 .ConfigureSyncfusionCore()
              .ConfigureSyncfusionToolkit()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });


            // Register Services
            builder.Services.AddSingleton<DatabaseService>();
            builder.Services.AddSingleton<ExportService>();

            // Register ViewModels
            builder.Services.AddTransient<MainViewModel>();
            builder.Services.AddTransient<HistoryViewModel>();

            // Register Views
            builder.Services.AddTransient<MainPage>();
            builder.Services.AddTransient<HistoryPage>();

#if DEBUG
            builder.Logging.AddDebug();
#endif


        

            return builder.Build();
        }
    }
}
