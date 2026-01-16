using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Plugin.Maui.TvDPad;
using System.Reflection;

namespace Plugin.Maui.TvDPad.SampleApp;

[Activity(
    Theme = "@style/Maui.SplashTheme",
    MainLauncher = true,
    Exported = true,
    LaunchMode = LaunchMode.SingleTask,
    ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density)]
[IntentFilter(
    new[] { Intent.ActionMain },
    Categories = new[] { Intent.CategoryLauncher, Intent.CategoryLeanbackLauncher })]
public class MainActivity : MauiAppCompatActivity
{
    const string Tag = "TvDPad.SampleApp";

    static readonly Lazy<MethodInfo?> HandleDown = new(() =>
        Feature.Default.GetType().GetMethod("HandleKeyDown", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic));

    static readonly Lazy<MethodInfo?> HandleUp = new(() =>
        Feature.Default.GetType().GetMethod("HandleKeyUp", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic));

    protected override void OnCreate(Bundle? savedInstanceState)
    {
        base.OnCreate(savedInstanceState);
        Log.Info(Tag, "OnCreate");

        var decor = Window?.DecorView;
        if (decor != null)
        {
            decor.FocusableInTouchMode = true;
            decor.RequestFocus();
        }
    }

    protected override void OnResume()
    {
        base.OnResume();
        Log.Info(Tag, "OnResume");
    }

    protected override void OnPause()
    {
        Log.Info(Tag, "OnPause");
        base.OnPause();
    }

    protected override void OnDestroy()
    {
        Log.Info(Tag, "OnDestroy");
        base.OnDestroy();
    }

    public override bool DispatchKeyEvent(KeyEvent? e)
    {
        if (e != null && TryHandle(e.KeyCode, e))
            return true;

        return base.DispatchKeyEvent(e);
    }

    public override bool OnKeyDown(Keycode keyCode, KeyEvent? e)
    {
        if (TryHandle(keyCode, e, isDown: true))
            return true;

        return base.OnKeyDown(keyCode, e);
    }

    public override bool OnKeyUp(Keycode keyCode, KeyEvent? e)
    {
        if (TryHandle(keyCode, e, isDown: false))
            return true;

        return base.OnKeyUp(keyCode, e);
    }

    static bool TryHandle(Keycode keyCode, KeyEvent? e, bool? isDown = null)
    {
        var feature = Feature.Default;
        if (feature?.IsSupported != true)
            return false;

        try
        {
            bool down = isDown ?? (e?.Action == KeyEventActions.Down);
            var mi = down ? HandleDown.Value : HandleUp.Value;
            if (mi == null)
                return false;

            return (bool)(mi.Invoke(feature, new object?[] { keyCode, e }) ?? false);
        }
        catch
        {
            return false;
        }
    }
}
