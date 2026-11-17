using Android.App;
using Android.Service.QuickSettings;
using VpnHood.AppLogic.App.Android.Activities;
using VpnHood.AppLogic.App.Android.Constants;
using VpnHood.AppHosting.Maui;
using VpnHood.AppHosting.WebView;
using VpnHood.Net.Toolkit.Graphics;

// ReSharper disable once CheckNamespace
namespace AppUiSample.Spa.Maui;

[Activity(
    Theme = "@style/Maui.SplashTheme",
    MainLauncher = true,
    LaunchMode = AndroidMainActivityConstants.LaunchMode,
    Exported = AndroidMainActivityConstants.Exported,
    WindowSoftInputMode = AndroidMainActivityConstants.WindowSoftInputMode,
    ScreenOrientation = AndroidMainActivityConstants.ScreenOrientation,
    ConfigurationChanges = AndroidMainActivityConstants.ConfigChanges)]

[IntentFilter([TileService.ActionQsTilePreferences])]
public class MainActivity : VpnHoodMauiMainActivity
{
    protected override AndroidAppMainActivityHandler CreateMainActivityHandler()
    {
        return new AndroidAppMainActivityHandler(this, new AndroidMainActivityOptions(), WindowBackgroundColor());
    }

    // the web UI's theme's window background, for the system bars and the ground behind them
    private static async Task<VhColor?> WindowBackgroundColor()
    {
        return (await WebViewTheme.Colors).WindowBackgroundColor;
    }
}
