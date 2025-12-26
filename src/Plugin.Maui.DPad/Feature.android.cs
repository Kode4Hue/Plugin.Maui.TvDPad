using Android.App;
using Android.Views;
using Android.Widget;
using Android.Util;
using System;

namespace Plugin.Maui.DPad;

partial class FeatureImplementation : IFeature
{
	private const string TAG = "Plugin.Maui.DPad";

	// Debounce recent toast emissions to avoid duplicates when events are delivered multiple times
	static readonly object toastLock = new object();
	static DPadKey? lastToastKey = null;
	static bool? lastToastIsKeyDown = null;
	static long lastToastTicks = 0; // ticks from DateTime.UtcNow.Ticks
	const int ToastDebounceMs = 300;

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

		// Ignore autorepeat key down events to avoid multiple toasts for one press
		if (keyEvent != null && keyEvent.RepeatCount > 0)
		{
			Log.Info(TAG, $"HandleKeyDown ignoring repeat for {keyCode} repeat={keyEvent.RepeatCount}");
			// Still raise the logical event but don't show the toast
			var repeatArgs = new DPadKeyEventArgs(dpadKey.Value, isKeyDown: true);
			OnKeyDown(repeatArgs);
			return repeatArgs.Handled;
		}

		try
		{
			Log.Info(TAG, $"HandleKeyDown mapped {keyCode} => {dpadKey}");
			// Use a human readable label for the toast instead of relying on enum ToString
			string label = GetDPadLabel(dpadKey.Value);

			// Show toast only on key down; skip on key up to avoid duplicate per press
			if (ShouldShowToast(dpadKey.Value, true))
			{
				Toast.MakeText(global::Android.App.Application.Context, $"DPad: {label}", ToastLength.Short).Show();
			}
		}
		catch { }

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

		// Some devices may emit multiple key up events; debounce using event time if provided
		if (keyEvent != null)
		{
			long eventMs = keyEvent.EventTime;
			lock (toastLock)
			{
				long lastMs = lastToastTicks / TimeSpan.TicksPerMillisecond;
				if (lastToastKey == dpadKey && lastToastIsKeyDown == false && Math.Abs(eventMs - lastMs) < ToastDebounceMs)
				{
					// Skip duplicate key-up handling
					Log.Info(TAG, $"HandleKeyUp skipping duplicate event for {keyCode} eventMs={eventMs} lastMs={lastMs}");
					var skipArgs = new DPadKeyEventArgs(dpadKey.Value, isKeyDown: false);
					OnKeyUp(skipArgs);
					return skipArgs.Handled;
				}
			}
		}

		// Do not show toasts on key up to avoid duplicate visual feedback; only raise logical event
		try
		{
			Log.Info(TAG, $"HandleKeyUp mapped {keyCode} => {dpadKey}");
		}
		catch { }

		var args = new DPadKeyEventArgs(dpadKey.Value, isKeyDown: false);
		OnKeyUp(args);
		return args.Handled;
	}

	static bool ShouldShowToast(DPadKey key, bool isKeyDown)
	{
		lock (toastLock)
		{
			long nowMs = DateTime.UtcNow.Ticks / TimeSpan.TicksPerMillisecond;
			if (lastToastKey == key && lastToastIsKeyDown == isKeyDown && (nowMs - (lastToastTicks / TimeSpan.TicksPerMillisecond)) < ToastDebounceMs)
			{
				// Duplicate within debounce window
				return false;
			}

			lastToastKey = key;
			lastToastIsKeyDown = isKeyDown;
			lastToastTicks = DateTime.UtcNow.Ticks;
			return true;
		}
	}

	static string GetDPadLabel(DPadKey key) => key switch
	{
		DPadKey.Up => "Up",
		DPadKey.Down => "Down",
		DPadKey.Left => "Left",
		DPadKey.Right => "Right",
		DPadKey.Center => "Center",
		DPadKey.Back => "Back",
		DPadKey.Menu => "Menu",
		DPadKey.Microphone => "Microphone",
		_ => key.ToString(),
	};

	/// <summary>
	/// Maps Android Keycode values to D-Pad keys.
	/// Supports DPAD_UP, DPAD_DOWN, DPAD_LEFT, DPAD_RIGHT, DPAD_CENTER,
	/// ENTER, BACK, MENU, and common keys.
	/// </summary>
	static DPadKey? MapAndroidKeyCode(Keycode keyCode)
	{
		switch (keyCode)
		{
			case Keycode.DpadUp:
				return DPadKey.Up;
			case Keycode.DpadDown:
				return DPadKey.Down;
			case Keycode.DpadLeft:
				return DPadKey.Left;
			case Keycode.DpadRight:
				return DPadKey.Right;
			case Keycode.DpadCenter:
			case Keycode.Enter:
			case Keycode.NumpadEnter:
			case Keycode.Space:
				return DPadKey.Center;
			case Keycode.Back:
				return DPadKey.Back;
			case Keycode.Menu:
				return DPadKey.Menu;
			case Keycode.VoiceAssist:
				return DPadKey.Microphone;
			default:
				return null;
		}
	}
}
