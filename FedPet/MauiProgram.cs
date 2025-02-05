using FedPet.Data;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using Plugin.LocalNotification;
using Plugin.LocalNotification.EventArgs;

namespace FedPet;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .UseLocalNotification()
            .ConfigureFonts(fonts => { fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular"); });

        builder.Services.AddMauiBlazorWebView();

#if DEBUG
        builder.Services.AddBlazorWebViewDeveloperTools();
        builder.Logging.AddDebug();
        LocalNotificationCenter.LogLevel = LogLevel.Debug;
#endif
        
        builder.Services.AddSingleton<DbService>();

        LocalNotificationCenter.Current.NotificationActionTapped += OnNotificationTapped;
        
        return builder.Build();
    }
    
    private static void OnNotificationTapped(NotificationActionEventArgs e)
    {
        // MainThread.BeginInvokeOnMainThread(async () =>
        //     await Shell.Current.GoToAsync(e.Request.ReturningData));
        MainThread.BeginInvokeOnMainThread(async () =>
            {
                Console.WriteLine("OnNotificationTapped");
                Console.WriteLine(e.Request.ReturningData);
                NotificationHandler.NavigateAfterNotification = true;
                NotificationHandler.NavigateAfterNotificationRoute = e.Request.ReturningData;
            }
            );
    }
}