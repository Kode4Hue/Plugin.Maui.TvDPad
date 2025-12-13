# Plugin.Maui.DPad - D-Pad Navigation for .NET MAUI

> ⚠️ **Current Focus: Android TV**  
> This plugin is currently optimized and tested specifically for **Android TV** applications with D-Pad remote control input. While the codebase includes multi-platform implementations, the primary target and support focus is Android TV at this time.

`Plugin.Maui.DPad` provides comprehensive D-Pad navigation support for .NET MAUI applications, enabling developers to create TV-optimized interfaces with full remote control support.

## Features

✅ Full D-Pad directional key support (Up, Down, Left, Right)  
✅ Action buttons (Center/OK/Select, Enter)  
✅ Navigation buttons (Back, Menu)  
✅ Automatic focus navigation between UI elements  
✅ Event-driven architecture with `KeyDown`, `KeyUp`, and `FocusNavigationRequested` events  
✅ Works with Android TV remotes and game controllers  
✅ Built for .NET 10 and .NET MAUI

## Installation

### NuGet Package (Coming Soon)

```bash
dotnet add package Plugin.Maui.DPad
```

### Manual Installation

1. Clone or download this repository
2. Add a project reference to `src/Plugin.Maui.Feature/Plugin.Maui.Feature.csproj` in your MAUI project

## Supported Platforms

| Platform | Support Status | Notes |
|----------|---------------|-------|
| Android TV | ✅ **Primary Target** | Fully tested with Android TV remotes |
| Android Mobile | ✅ Supported | Works with devices that have D-Pad hardware |
| iOS/tvOS | ⚠️ Experimental | Code included but not primary focus |
| Windows | ⚠️ Experimental | Code included but not primary focus |
| macOS | ⚠️ Experimental | Code included but not primary focus |

**Minimum Versions:**
- Android: API 21 (Android 5.0) or higher
- .NET: 10.0
- .NET MAUI: 10.x

## Quick Start for Android TV

### 1. Register the Plugin

In your `MauiProgram.cs`:

```csharp
using Plugin.Maui.Feature;

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

        // Register D-Pad feature
        builder.Services.AddSingleton<IFeature>(Feature.Default);
        
        return builder.Build();
    }
}
```

### 2. Hook Up Android Activity

In `Platforms/Android/MainActivity.cs`:

```csharp
using Android.App;
using Android.Content.PM;
using Android.Views;
using Plugin.Maui.Feature;

[Activity(Theme = "@style/Maui.SplashTheme", MainLauncher = true, 
    ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
public class MainActivity : MauiAppCompatActivity
{
    public override bool OnKeyDown(Keycode keyCode, KeyEvent? e)
    {
        // Handle D-Pad input
        var feature = Feature.Default as FeatureImplementation;
        if (feature != null && feature.HandleKeyDown(keyCode, e))
        {
            return true;
        }
        return base.OnKeyDown(keyCode, e);
    }

    public override bool OnKeyUp(Keycode keyCode, KeyEvent? e)
    {
        var feature = Feature.Default as FeatureImplementation;
        if (feature != null && feature.HandleKeyUp(keyCode, e))
        {
            return true;
        }
        return base.OnKeyUp(keyCode, e);
    }
}
```

### 3. Use in Your Page

```csharp
using Plugin.Maui.Feature;

public partial class MainPage : ContentPage
{
    private readonly IFeature dpad;

    public MainPage(IFeature dpad)
    {
        InitializeComponent();
        this.dpad = dpad;

        // Start listening for D-Pad events
        dpad.StartListening();
        
        // Enable automatic focus navigation
        dpad.EnableFocusNavigation();

        // Subscribe to key events
        dpad.KeyDown += OnDPadKeyDown;
    }

    private void OnDPadKeyDown(object? sender, DPadKeyEventArgs e)
    {
        switch (e.Key)
        {
            case DPadKey.Back:
                // Handle back button
                if (Navigation.NavigationStack.Count > 1)
                {
                    Navigation.PopAsync();
                    e.Handled = true;
                }
                break;
                
            case DPadKey.Center:
            case DPadKey.Enter:
                // Handle selection
                Console.WriteLine("Item selected!");
                break;
        }
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        
        // Clean up event handlers
        dpad.KeyDown -= OnDPadKeyDown;
    }
}
```

## Testing on Android TV

### Prerequisites

1. **Android TV Device or Emulator**
   - Physical Android TV device, or
   - Android TV emulator from Android Studio

2. **Development Tools**
   - Visual Studio 2022 (Windows) or Visual Studio for Mac
   - .NET 10 SDK
   - .NET MAUI workload installed

### Setting Up Android TV Emulator

1. **Open Android Studio** and navigate to Tools → Device Manager

2. **Create Android TV Virtual Device:**
   - Click "Create Device"
   - Select "TV" category
   - Choose a TV device profile (e.g., "Android TV (1080p)")
   - Select a system image (API 31+ recommended)
   - Finish the setup

3. **Start the emulator** from Device Manager

### Deploying Your App

#### From Visual Studio

1. Build your MAUI project for Android
2. Select the Android TV emulator or device from the device dropdown
3. Click "Run" or press F5
4. The app will deploy to the Android TV

#### From Command Line

```bash
# Build the project
dotnet build -f net10.0-android -c Release

# Install on connected device/emulator
adb install -r bin/Release/net10.0-android/YourApp.apk
```

### Testing D-Pad Input

#### On Android TV Emulator

The Android TV emulator includes a virtual remote control:

1. When the emulator is running, look for the remote control panel on the right side
2. Use the directional pad buttons to test navigation
3. Use the center button to test selection
4. Use the back button to test back navigation

**Keyboard Shortcuts for Emulator:**
- Arrow keys → D-Pad directional input
- Enter → D-Pad center button
- Backspace → Back button
- M → Menu button

#### On Physical Android TV

1. Use your Android TV remote control
2. Navigate using the directional buttons
3. Press OK/Select for the center button
4. Use the back button for navigation

### Debugging Tips

1. **Enable ADB Debugging on Android TV:**
   - Go to Settings → Device Preferences → About
   - Click "Build" 7 times to enable Developer Options
   - Go to Developer Options → Enable USB Debugging
   - Enable "USB Debugging" and "ADB over network"

2. **View Logs:**
   ```bash
   adb logcat | grep -i "dpad\|keyevent"
   ```

3. **Test Key Events:**
   - Add logging in your `OnKeyDown` handler to verify events are firing
   - Check that `feature.HandleKeyDown()` is being called
   - Verify `e.Handled` is being set correctly

## API Reference

### Main Interface: IFeature

```csharp
public interface IFeature
{
    // Events
    event EventHandler<DPadKeyEventArgs>? KeyDown;
    event EventHandler<DPadKeyEventArgs>? KeyUp;
    event EventHandler<FocusNavigationEventArgs>? FocusNavigationRequested;

    // Properties
    bool IsSupported { get; }
    bool IsListening { get; }
    bool IsFocusNavigationEnabled { get; }

    // Methods
    void StartListening();
    void StopListening();
    void EnableFocusNavigation();
    void DisableFocusNavigation();
}
```

### Supported D-Pad Keys

```csharp
public enum DPadKey
{
    Up,          // Directional up
    Down,        // Directional down
    Left,        // Directional left
    Right,       // Directional right
    Center,      // Center/OK/Select button
    Back,        // Back button
    Menu,        // Menu button
    Enter,       // Enter button
    Microphone   // Microphone button (some remotes)
}
```

### Event Arguments

**DPadKeyEventArgs:**
```csharp
public class DPadKeyEventArgs : EventArgs
{
    public DPadKey Key { get; }           // Which key was pressed
    public bool Handled { get; set; }      // Set to true to prevent default behavior
    public bool IsKeyDown { get; }         // True for KeyDown, false for KeyUp
    public DateTimeOffset Timestamp { get; } // When the event occurred
}
```

**FocusNavigationEventArgs:**
```csharp
public class FocusNavigationEventArgs : EventArgs
{
    public DPadKey Direction { get; }              // Navigation direction
    public bool Handled { get; set; }              // Set to true to handle manually
    public object? NextFocusElement { get; set; }  // Set to override focus target
}
```

## Common Use Cases

### Focus Navigation

Enable automatic navigation between focusable UI elements:

```csharp
dpad.EnableFocusNavigation();

dpad.FocusNavigationRequested += (sender, e) =>
{
    Console.WriteLine($"Moving focus {e.Direction}");
    // System handles focus automatically unless you set e.Handled = true
};
```

### Item Selection in Lists

```csharp
dpad.KeyDown += (sender, e) =>
{
    if (e.Key == DPadKey.Center || e.Key == DPadKey.Enter)
    {
        var selectedItem = myCollectionView.SelectedItem;
        if (selectedItem != null)
        {
            // Handle item selection
            ProcessSelection(selectedItem);
            e.Handled = true;
        }
    }
};
```

### Back Navigation

```csharp
dpad.KeyDown += (sender, e) =>
{
    if (e.Key == DPadKey.Back)
    {
        if (CanGoBack())
        {
            GoBack();
            e.Handled = true;
        }
    }
};
```

## Sample Application

A complete sample application demonstrating all features is included in the `samples/` directory. The sample shows:

- ✅ Focus navigation with a 3×3 button grid
- ✅ Item selection with scrollable lists
- ✅ Back/Menu button handling
- ✅ Real-time event logging
- ✅ Status indicators

To run the sample:

```bash
cd samples/Plugin.Maui.Feature.Sample
dotnet build -f net10.0-android
```

## Architecture

The plugin uses platform-specific implementations:

- **Android**: Maps Android `Keycode.Dpad*` constants to `DPadKey` enum
- **Other Platforms**: Experimental implementations included for future expansion

See [DPAD_IMPLEMENTATION.md](DPAD_IMPLEMENTATION.md) for detailed technical documentation.

## Troubleshooting

### D-Pad Events Not Firing

1. Ensure `StartListening()` has been called
2. Verify `OnKeyDown`/`OnKeyUp` are implemented in `MainActivity.cs`
3. Check that `feature.HandleKeyDown()` is being called
4. Confirm your Android TV remote is working (test with other apps)

### Focus Navigation Not Working

1. Ensure UI elements have `IsTabStop="True"` or `IsEnabled="True"`
2. Call `EnableFocusNavigation()` after `StartListening()`
3. Verify elements are actually focusable in your layout

### App Crashes on Key Press

1. Check for null references in event handlers
2. Ensure event handlers are unsubscribed in `OnDisappearing()`
3. Wrap event handler code in try-catch for debugging

## Contributing

This project is in active development with a focus on Android TV. Contributions are welcome, particularly:

- Android TV testing and bug reports
- Performance improvements
- Documentation enhancements
- Sample app improvements

## License

MIT License - See [LICENSE](LICENSE) file for details.

## Acknowledgements

Built using the [Plugin.Maui.Feature](https://github.com/jfversluis/Plugin.Maui.Feature) template.

## Support

For issues, questions, or feature requests related to Android TV D-Pad support, please open an issue on GitHub.

---

**Note:** While this plugin includes implementations for multiple platforms, the current development focus and testing is exclusively on Android TV. Other platform implementations are experimental and not guaranteed to work as expected.
