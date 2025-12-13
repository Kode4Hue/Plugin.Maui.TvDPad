using Android.App;
using Android.Views;

namespace Plugin.Maui.Feature;

partial class FeatureImplementation : IFeature
{
	private partial bool GetIsSupported()
	{
		// D-Pad is widely supported on Android devices and TV platforms
		return true;
	}

	private partial void StartListeningPlatform()
	{
		// On Android, key events are typically handled at the Activity level
		// through OnKeyDown/OnKeyUp overrides or key event listeners.
		// The actual listening is implemented when activities process key events.
	}

	private partial void StopListeningPlatform()
	{
		// Platform-specific cleanup if needed
	}

	private partial void EnableFocusNavigationPlatform()
	{
		// Focus navigation on Android is handled natively by the system
		// when focusable views are properly configured
	}

	private partial void DisableFocusNavigationPlatform()
	{
		// Platform-specific cleanup if needed
	}

	/// <summary>
	/// Handles Android key events and converts them to D-Pad events.
	/// This should be called from Activity.OnKeyDown or similar.
	/// </summary>
	public bool HandleKeyDown(Keycode keyCode, KeyEvent? keyEvent)
	{
		var dpadKey = MapAndroidKeyCode(keyCode);
		if (dpadKey == null)
			return false;

		var args = new DPadKeyEventArgs(dpadKey.Value, isKeyDown: true);
		OnKeyDown(args);
		return args.Handled;
	}

	/// <summary>
	/// Handles Android key up events and converts them to D-Pad events.
	/// This should be called from Activity.OnKeyUp or similar.
	/// </summary>
	public bool HandleKeyUp(Keycode keyCode, KeyEvent? keyEvent)
	{
		var dpadKey = MapAndroidKeyCode(keyCode);
		if (dpadKey == null)
			return false;

		var args = new DPadKeyEventArgs(dpadKey.Value, isKeyDown: false);
		OnKeyUp(args);
		return args.Handled;
	}

	static DPadKey? MapAndroidKeyCode(Keycode keyCode)
	{
		return keyCode switch
		{
			Keycode.DpadUp => DPadKey.Up,
			Keycode.DpadDown => DPadKey.Down,
			Keycode.DpadLeft => DPadKey.Left,
			Keycode.DpadRight => DPadKey.Right,
			Keycode.DpadCenter => DPadKey.Center,
			Keycode.Enter or Keycode.NumpadEnter => DPadKey.Enter,
			Keycode.Back => DPadKey.Back,
			Keycode.Menu => DPadKey.Menu,
			_ => null
		};
	}
}
