using Android.App;
using Android.Views;
using Android.Widget;
using Android.Util;
using System;

namespace Plugin.Maui.TvDPad;

partial class FeatureImplementation : IFeature
{
	private const string TAG = "Plugin.Maui.TvDPad";

	static readonly object toastLock = new object();
	static DPadKey? lastToastKey = null;
	static bool? lastToastIsKeyDown = null;
	static long lastToastTicks = 0;
	static Toast? activeToast = null;

	// Keep visual feedback snappy on TV remotes.
	const int ToastDebounceMs = 75;

	// Default to off for performance; callers/sample app can enable.
	public static bool EnableToastFeedback { get; set; } = false;
	public static bool EnableVerboseLogging { get; set; } = false;

	private partial bool GetIsSupported()
	{
		return true;
	}

	private partial void StartListeningPlatform() { }
	private partial void StopListeningPlatform() { }
	private partial void EnableFocusNavigationPlatform() { }
	private partial void DisableFocusNavigationPlatform() { }

	public bool HandleKeyDown(Keycode keyCode, KeyEvent? keyEvent)
	{
		var dpadKey = MapAndroidKeyCode(keyCode);
		if (dpadKey == null)
			return false;

		if (keyEvent != null && keyEvent.RepeatCount > 0)
		{
			if (EnableVerboseLogging)
				try { Log.Info(TAG, $"HandleKeyDown ignoring repeat for {keyCode} repeat={keyEvent.RepeatCount}"); } catch { }

			var repeatArgs = new DPadKeyEventArgs(dpadKey.Value, isKeyDown: true);
			OnKeyDown(repeatArgs);
			return repeatArgs.Handled;
		}

		if (EnableToastFeedback)
		{
			try
			{
				if (EnableVerboseLogging)
					try { Log.Info(TAG, $"HandleKeyDown mapped {keyCode} => {dpadKey}"); } catch { }

				string label = GetDPadLabel(dpadKey.Value);
				if (ShouldShowToast(dpadKey.Value, true))
				{
					lock (toastLock)
					{
						activeToast?.Cancel();
						activeToast = Toast.MakeText(global::Android.App.Application.Context, $"DPad: {label}", ToastLength.Short);
						activeToast.Show();
					}
				}
			}
			catch { }
		}

		var args = new DPadKeyEventArgs(dpadKey.Value, isKeyDown: true);
		OnKeyDown(args);
		return args.Handled;
	}

	public bool HandleKeyUp(Keycode keyCode, KeyEvent? keyEvent)
	{
		var dpadKey = MapAndroidKeyCode(keyCode);
		if (dpadKey == null)
			return false;

		// Some remotes/devices can emit duplicate Up events; keep the suppression window short
		// so navigation still feels immediate.
		if (keyEvent != null)
		{
			long eventMs = keyEvent.EventTime;
			lock (toastLock)
			{
				long lastMs = lastToastTicks / TimeSpan.TicksPerMillisecond;
				if (lastToastKey == dpadKey && lastToastIsKeyDown == false && Math.Abs(eventMs - lastMs) < ToastDebounceMs)
				{
					if (EnableVerboseLogging)
						try { Log.Info(TAG, $"HandleKeyUp skipping duplicate event for {keyCode} eventMs={eventMs} lastMs={lastMs}"); } catch { }

					var skipArgs = new DPadKeyEventArgs(dpadKey.Value, isKeyDown: false);
					OnKeyUp(skipArgs);
					return skipArgs.Handled;
				}
			}
		}

		if (EnableVerboseLogging)
			try { Log.Info(TAG, $"HandleKeyUp mapped {keyCode} => {dpadKey}"); } catch { }

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
