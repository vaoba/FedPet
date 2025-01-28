using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Content.Res;
using Android.OS;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.WebView.Maui;
using Microsoft.Maui.Controls.PlatformConfiguration.AndroidSpecific;

namespace FedPet;

[Activity(Theme = "@style/Maui.SplashTheme", MainLauncher = true,
    ConfigurationChanges = ConfigChanges.FontScale | ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode |
                           ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density)]
public class MainActivity : MauiAppCompatActivity
{
    protected override void OnCreate(Bundle bundle)
    {
        base.OnCreate(bundle);
        Resources.Configuration.FontScale = 1.0f;
    }
    
    // FOUND THIS ONLINE, STOPS ANDROID FONT SIZE FROM SCALING ALL FONTS
    protected override void AttachBaseContext(Context @base)
    {
        Configuration configuration = new(@base.Resources.Configuration)
        {
            FontScale = 1.0f
        };
        ApplyOverrideConfiguration(configuration);
        base.AttachBaseContext(@base);
    }
    
}