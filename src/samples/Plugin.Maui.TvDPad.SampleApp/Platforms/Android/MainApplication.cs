using Android.App;
using Android.Runtime;
using Android.Util;

namespace Plugin.Maui.TvDPad.SampleApp;

[Application]
public class MainApplication : MauiApplication
{
    const string Tag = "TvDPad.SampleApp";

    public MainApplication(nint handle, JniHandleOwnership ownership)
        : base(handle, ownership)
    {
        AndroidEnvironment.UnhandledExceptionRaiser += (_, e) =>
        {
            Log.Error(Tag, "UnhandledException: " + e.Exception);
            e.Handled = false;
        };

        AppDomain.CurrentDomain.UnhandledException += (_, e) =>
        {
            Log.Error(Tag, "AppDomain UnhandledException: " + e.ExceptionObject);
        };

        TaskScheduler.UnobservedTaskException += (_, e) =>
        {
            Log.Error(Tag, "UnobservedTaskException: " + e.Exception);
        };
    }

    protected override MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp();
}
