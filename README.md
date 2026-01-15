# Plugin.Maui.TvDPad - D-Pad Navigation for .NET MAUI

> ⚠️ **Current Focus: Android TV and Amazon Fire Stick**

`Plugin.Maui.TvDPad` provides D-Pad navigation support for .NET MAUI applications, enabling developers to build TV-optimized interfaces with full remote control support.

## Features

- Full D-Pad directional key support (Up, Down, Left, Right)
- Action buttons (Center/OK/Select, Enter)
- Navigation buttons (Back, Menu)
- Automatic focus navigation between UI elements
- Event-driven API with `KeyDown`, `KeyUp`, and `FocusNavigationRequested` events
- Works with Android TV remotes and common controllers
- Built for .NET 10 and .NET MAUI

## Installation

### Install via NuGet (recommended)

NuGet package: http://www.nuget.org/packages/Plugin.Maui.TvDPad  
Current version: `1.0.0-alpha.1`

In your .NET MAUI Android project (`net10.0-android`):

- **Visual Studio**: right-click your MAUI project -> `Manage NuGet Packages...` -> search for `Plugin.Maui.TvDPad` -> install.
- **CLI**:

```bash
dotnet add <YourMauiProject>.csproj package Plugin.Maui.TvDPad --version 1.0.0-alpha.1
```

After installing, build once so MAUI restores the package and generates platform resources.

### Manual installation (project reference)

1. Clone or download this repository
2. Add a project reference to `src/Plugin.Maui.TvDPad/Plugin.Maui.TvDPad.csproj` in your MAUI project

## Quick Start for Android TV

### 1. Register the plugin (optional DI)

In `MauiProgram.cs` register the feature if you want to consume it via DI (optional):

```csharp
using Plugin.Maui.TvDPad;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
            });

#if DEBUG
        builder.Logging.AddDebug();
#endif

        // Optional: register shared feature in DI
        builder.Services.AddSingleton<IFeature>(Feature.Default);

        return builder.Build();
    }
}
```

> Note: You can also use the static `Feature.Default` instance directly without registering it in DI.

### 2. Forward key events from Android Activity

In `Platforms/Android/MainActivity.cs` forward key events to the plugin so it can map and raise events:

```csharp
using Android.App;
using Android.Content.PM;
using Android.Views;
using Plugin.Maui.TvDPad;

[Activity(...)]
public class MainActivity : MauiAppCompatActivity
{
    readonly IFeature dpad = Feature.Default;

    public override bool DispatchKeyEvent(KeyEvent e)
    {
        if (dpad is FeatureImplementation impl)
        {
            if (e.Action == KeyEventActions.Down && impl.HandleKeyDown((Keycode)e.KeyCode, e))
                return true;
            if (e.Action == KeyEventActions.Up && impl.HandleKeyUp((Keycode)e.KeyCode, e))
                return true;
        }

        return base.DispatchKeyEvent(e);
    }
}
```

### 3. Use in a Page

Subscribe to events and update UI safely (invoke on main thread):

```csharp
using Plugin.Maui.TvDPad;

public partial class MainPage : ContentPage
{
    readonly IFeature dpad = Feature.Default;

    public MainPage()
    {
        InitializeComponent();
        dpad.KeyDown += OnKeyDown;
        dpad.KeyUp += OnKeyUp;
        dpad.StartListening();
        dpad.EnableFocusNavigation();
    }

    void OnKeyDown(object? s, DPadKeyEventArgs e)
    {
        MainThread.BeginInvokeOnMainThread(() => StatusLabel.Text = $"Key Down: {e.Key}");
        e.Handled = true;
    }
}
```

## Testing on Android TV

- Use an Android TV emulator image or a physical Android TV device
- Emulator keyboard mappings: arrow keys → D-Pad, Enter → Center, Backspace → Back
- Ensure `MainActivity` forwards key events to the plugin

## Troubleshooting

- If events don't fire: ensure `StartListening()` was called and `DispatchKeyEvent` forwards to `Feature.Default`
- If UI updates crash: ensure you update UI on the main thread (use `MainThread.BeginInvokeOnMainThread`)

## Project layout

- `src/Plugin.Maui.TvDPad` — plugin library (multi-targeted)
- `samples/` — sample apps (if included)

## License

MIT
