using ARMzalogApp.ViewModels;
using ARMzalogApp.Views;
using ARMzalogApp.Models;
using Microsoft.Extensions.Logging;
using Plugin.FirebasePushNotification;
using Plugin.LocalNotification;
using CommunityToolkit.Maui;
using ARMzalogApp.Sevices.Integrations;
using Microsoft.Extensions.DependencyInjection;
using ARMzalogApp.Constants;

namespace ARMzalogApp
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseMauiCommunityToolkit()
                .UseLocalNotification()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });
            builder.Services.AddSingleton<HomePage>();
            builder.Services.AddSingleton<LoginPage>();
            builder.Services.AddSingleton<ZavkrPage>();
            builder.Services.AddSingleton<AboutPage>();
            builder.Services.AddSingleton<ExchangeRatesPage>();
            builder.Services.AddSingleton<ChatPage>();
            builder.Services.AddSingleton<User>();

            builder.Services.AddSingleton<LoginPageViewModel>();


            builder.Services.AddHttpClient("PersonalAccountsApi", client =>
            {
                client.BaseAddress = new Uri(ServerConstants.SERVER_ROOT_URL);
                client.Timeout = TimeSpan.FromSeconds(60);
            });

            builder.Services.AddScoped<IKibIntegrationService, KibIntegrationService>();
            builder.Services.AddScoped<IGrsIkService, GrsIkService>();
            builder.Services.AddScoped<ISocialFundIntegrationService, SocialFundIntegrationService>();

            builder.Services.AddTransient<CheckClientViewModel>();
            builder.Services.AddTransient<CheckClientPage>();
#if DEBUG
            builder.Logging.AddDebug();
#endif
            // Инициализация Firebase Push Notification
            // При запуске через "Windows Machine" нижний код закомментировать
            CrossFirebasePushNotification.Current.OnNotificationReceived += (s, p) => // для выполнения определенных действия после того как получает уведомление
            {
                System.Diagnostics.Debug.WriteLine("Received");
                foreach (var data in p.Data)
                {
                    System.Diagnostics.Debug.WriteLine($"{data.Key} : {data.Value}");
                }
            };

            CrossFirebasePushNotification.Current.OnTokenRefresh += (s, p) => // используется когда устройство обновляется - для получения актуального токена
            {
                System.Diagnostics.Debug.WriteLine($"Token: {p.Token}");
                // send to our server
            };
            return builder.Build();
        }
    }
}
