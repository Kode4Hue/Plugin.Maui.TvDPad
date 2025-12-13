# D-Pad Navigation Plugin Implementation

This document describes the implementation of the D-Pad navigation plugin for .NET MAUI applications.

## Overview

The D-Pad Navigation Plugin (`Plugin.Maui.Feature`) provides comprehensive support for handling D-Pad and remote control input across .NET MAUI platforms. It enables developers to create TV apps, kiosk applications, and accessible interfaces with full directional navigation support.

## Supported Platforms

- ✅ Android (TV, mobile devices with D-Pad)
- ✅ iOS/tvOS (via game controllers and keyboards)
- ✅ macOS (via keyboards and game controllers)
- ✅ Windows (via keyboards and game controllers)
- ✅ .NET (for testing purposes)

## Framework Version

- **Target Framework**: .NET 10.0
- **MAUI Version**: Compatible with .NET MAUI 10.x

## Key Features

### 1. D-Pad Key Support

The plugin supports all common D-Pad keys:
- **Directional Keys**: Up, Down, Left, Right
- **Action Keys**: Center/OK/Select, Enter
- **Navigation Keys**: Back, Menu
- **Special Keys**: Microphone

### 2. Focus Navigation

Automatic focus navigation between UI elements when directional keys are pressed:
```csharp
var dpad = Feature.Default;
dpad.EnableFocusNavigation();
```

### 3. Event-Based Architecture

Three main event types:
- `KeyDown`: Triggered when a D-Pad key is pressed
- `KeyUp`: Triggered when a D-Pad key is released
- `FocusNavigationRequested`: Triggered when focus should move between elements

## API Usage

### Basic Setup

```csharp
using Plugin.Maui.Feature;

// Get the D-Pad feature instance
var dpad = Feature.Default;

// Start listening for D-Pad events
dpad.StartListening();

// Subscribe to key events
dpad.KeyDown += (sender, e) =>
{
    Console.WriteLine($"Key pressed: {e.Key}");
    
    // Handle specific keys
    if (e.Key == DPadKey.Back)
    {
        // Handle back navigation
        e.Handled = true;
    }
};
```

### Focus Navigation

```csharp
// Enable automatic focus navigation
dpad.EnableFocusNavigation();

// Handle focus navigation requests
dpad.FocusNavigationRequested += (sender, e) =>
{
    Console.WriteLine($"Focus moving: {e.Direction}");
    
    // Optionally override default navigation
    if (needsCustomBehavior)
    {
        e.NextFocusElement = customElement;
        e.Handled = true;
    }
};
```

### Dependency Injection

```csharp
// In MauiProgram.cs
builder.Services.AddSingleton<IFeature>(Feature.Default);

// In your page/view model
public class MyViewModel
{
    private readonly IFeature dpad;
    
    public MyViewModel(IFeature dpad)
    {
        this.dpad = dpad;
        dpad.StartListening();
        dpad.KeyDown += OnKeyDown;
    }
}
```

## Platform-Specific Implementation

### Android

On Android, key events are captured at the Activity level:

```csharp
public override bool OnKeyDown(Keycode keyCode, KeyEvent? e)
{
    var feature = Feature.Default as FeatureImplementation;
    if (feature != null && feature.HandleKeyDown(keyCode, e))
    {
        return true;
    }
    return base.OnKeyDown(keyCode, e);
}
```

### Windows

On Windows, keyboard events are handled through the main window:

```csharp
window.Content.KeyDown += (sender, e) =>
{
    var feature = Feature.Default as FeatureImplementation;
    feature?.HandleKeyRoutedEvent(e, isKeyDown: true);
};
```

### iOS/macOS

On iOS/macOS, use UIKeyCommand or the focus engine to capture input.

## Use Cases

### 1. Focus Navigation

Navigate between buttons, input fields, and other focusable elements:
```csharp
dpad.EnableFocusNavigation();
// Users can now navigate UI with D-Pad arrows
```

### 2. Item Selection

Select items from lists or collections:
```csharp
dpad.KeyDown += (sender, e) =>
{
    if (e.Key == DPadKey.Center || e.Key == DPadKey.Enter)
    {
        SelectCurrentItem();
        e.Handled = true;
    }
};
```

### 3. Back/Menu Navigation

Handle navigation and menu actions:
```csharp
dpad.KeyDown += (sender, e) =>
{
    if (e.Key == DPadKey.Back)
    {
        NavigateBack();
        e.Handled = true;
    }
    else if (e.Key == DPadKey.Menu)
    {
        ShowMenu();
        e.Handled = true;
    }
};
```

## Testing

The plugin includes 46 comprehensive tests covering:
- ✅ All D-Pad key events
- ✅ Focus navigation scenarios
- ✅ Item selection workflows
- ✅ Back/Menu navigation
- ✅ Event handling and propagation
- ✅ State management

Run tests:
```bash
# Build plugin first
dotnet build src/Plugin.Maui.Feature/Plugin.Maui.Feature.csproj /p:TargetFrameworks=net10.0 -c Debug

# Run tests
dotnet test tests/Plugin.Maui.Feature.Tests.sln
```

## Sample Application

A comprehensive sample application is included demonstrating:
- Status display (listening state, supported platform)
- Control buttons (start/stop listening, enable/disable focus navigation)
- Event log showing all D-Pad activity
- Focus navigation demo with 3x3 button grid
- Item selection demo with a list
- Back/Menu navigation handling

Run the sample:
```bash
dotnet build samples/Plugin.Maui.Feature.Sample.sln
```

## Architecture

### Core Components

1. **IFeature**: Main interface defining the plugin API
2. **DPadKey**: Enum defining all supported keys
3. **DPadKeyEventArgs**: Event arguments for key press/release events
4. **FocusNavigationEventArgs**: Event arguments for focus navigation requests
5. **FeatureImplementation**: Base implementation with platform-specific partials

### Platform-Specific Implementations

- `Feature.android.cs`: Android D-Pad key mapping
- `Feature.macios.cs`: iOS/macOS key command handling
- `Feature.windows.cs`: Windows keyboard/gamepad input
- `Feature.net.cs`: Generic .NET implementation for testing

## Best Practices

1. **Always stop listening**: Call `StopListening()` when navigation away from pages using D-Pad input
2. **Handle events appropriately**: Set `e.Handled = true` to prevent default behavior
3. **Enable focus navigation**: Use `EnableFocusNavigation()` for automatic UI navigation
4. **Test on real devices**: Test with actual remote controls or game controllers
5. **Provide visual feedback**: Show focus states clearly for TV/10-foot interfaces

## Performance

- Minimal overhead: Events only fire when listening is active
- No polling: Event-driven architecture
- Platform-native: Uses platform-specific APIs for best performance

## Limitations

- iOS/macOS builds require macOS with Xcode
- Focus navigation behavior may vary by platform
- Some keys may not be available on all devices

## Contributing

When adding new features:
1. Update all platform implementations
2. Add comprehensive tests
3. Update this documentation
4. Test on multiple platforms

## License

MIT License - See LICENSE file for details
