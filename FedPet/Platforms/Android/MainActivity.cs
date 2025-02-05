using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Content.Res;
using Android.OS;

namespace FedPet;

[Activity(Theme = "@style/Maui.SplashTheme", MainLauncher = true,
    ConfigurationChanges = ConfigChanges.FontScale | ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode |
                           ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density, ScreenOrientation = ScreenOrientation.Portrait)]
public class MainActivity : MauiAppCompatActivity
{
    protected override void OnCreate(Bundle? bundle)
    {
        base.OnCreate(bundle);
        if (Resources?.Configuration != null)
        {
            Resources.Configuration.FontScale = 1.0f;
            Resources.Configuration.Orientation = (Orientation)ScreenOrientation.Portrait;
        }
    }
    
    // FOUND THIS ONLINE, STOPS ANDROID FONT SIZE FROM SCALING ALL FONTS
    protected override void AttachBaseContext(Context? @base)
    {
        Configuration configuration = new(@base?.Resources?.Configuration)
        {
            FontScale = 1.0f
        };
        ApplyOverrideConfiguration(configuration);
        base.AttachBaseContext(@base);
    }
    
}