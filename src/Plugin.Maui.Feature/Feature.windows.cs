using Microsoft.UI.Xaml.Input;
using Windows.System;

namespace Plugin.Maui.Feature;

partial class FeatureImplementation : IFeature
{
	private partial bool GetIsSupported()
	{
		// D-Pad support is available on Windows via keyboard and game controllers
		return true;
	}

	private partial void StartListeningPlatform()
	{
		// On Windows, key events are handled through KeyDown/KeyUp event handlers
		// or accelerator key handlers
	}

	private partial void StopListeningPlatform()
	{
		// Platform-specific cleanup if needed
	}

	private partial void EnableFocusNavigationPlatform()
	{
		// Focus navigation on Windows is handled by the built-in focus manager
	}

	private partial void DisableFocusNavigationPlatform()
	{
		// Platform-specific cleanup if needed
	}

	/// <summary>
	/// Handles Windows keyboard events and converts them to D-Pad events.
	/// This should be called from KeyDown event handlers.
	/// </summary>
	public bool HandleKeyDown(VirtualKey key)
	{
		var dpadKey = MapVirtualKey(key);
		if (dpadKey == null)
			return false;

		var args = new DPadKeyEventArgs(dpadKey.Value, isKeyDown: true);
		OnKeyDown(args);
		return args.Handled;
	}

	/// <summary>
	/// Handles Windows keyboard up events and converts them to D-Pad events.
	/// This should be called from KeyUp event handlers.
	/// </summary>
	public bool HandleKeyUp(VirtualKey key)
	{
		var dpadKey = MapVirtualKey(key);
		if (dpadKey == null)
			return false;

		var args = new DPadKeyEventArgs(dpadKey.Value, isKeyDown: false);
		OnKeyUp(args);
		return args.Handled;
	}

	/// <summary>
	/// Handles Windows keyboard events from KeyRoutedEventArgs.
	/// </summary>
	public bool HandleKeyRoutedEvent(KeyRoutedEventArgs e, bool isKeyDown)
	{
		var dpadKey = MapVirtualKey(e.Key);
		if (dpadKey == null)
			return false;

		var args = new DPadKeyEventArgs(dpadKey.Value, isKeyDown);
		
		if (isKeyDown)
			OnKeyDown(args);
		else
			OnKeyUp(args);

		if (args.Handled)
			e.Handled = true;

		return args.Handled;
	}

	static DPadKey? MapVirtualKey(VirtualKey key)
	{
		return key switch
		{
			VirtualKey.Up or VirtualKey.GamepadDPadUp => DPadKey.Up,
			VirtualKey.Down or VirtualKey.GamepadDPadDown => DPadKey.Down,
			VirtualKey.Left or VirtualKey.GamepadDPadLeft => DPadKey.Left,
			VirtualKey.Right or VirtualKey.GamepadDPadRight => DPadKey.Right,
			VirtualKey.Enter or VirtualKey.GamepadA => DPadKey.Enter,
			VirtualKey.Space => DPadKey.Center,
			VirtualKey.Back or VirtualKey.Escape or VirtualKey.GamepadB => DPadKey.Back,
			VirtualKey.GamepadMenu or VirtualKey.Application => DPadKey.Menu,
			_ => null
		};
	}
}
