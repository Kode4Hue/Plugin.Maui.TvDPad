using Microsoft.Maui.ApplicationModel;

namespace Plugin.Maui.TvDPad;

public static class Feature
{
	static IFeature? defaultImplementation;

	public static IFeature Default => defaultImplementation ??= new FeatureImplementation();

	internal static void SetDefault(IFeature? implementation) => defaultImplementation = implementation;
}

public partial class FeatureImplementation : IFeature
{
	bool isListening;
	bool isFocusNavigationEnabled;

	public event EventHandler<DPadKeyEventArgs>? KeyDown;
	public event EventHandler<DPadKeyEventArgs>? KeyUp;
	public event EventHandler<FocusNavigationEventArgs>? FocusNavigationRequested;

	public virtual bool IsSupported => GetIsSupported();
	public bool IsListening => isListening;
	public bool IsFocusNavigationEnabled => isFocusNavigationEnabled;

	public void StartListening()
	{
		if (isListening) return;
		isListening = true;
		StartListeningPlatform();
	}

	public void StopListening()
	{
		if (!isListening) return;
		isListening = false;
		StopListeningPlatform();
	}

	public void EnableFocusNavigation()
	{
		if (isFocusNavigationEnabled) return;
		isFocusNavigationEnabled = true;
		EnableFocusNavigationPlatform();
	}

	public void DisableFocusNavigation()
	{
		if (!isFocusNavigationEnabled) return;
		isFocusNavigationEnabled = false;
		DisableFocusNavigationPlatform();
	}

	protected virtual void OnKeyDown(DPadKeyEventArgs e)
	{
		if (isFocusNavigationEnabled && IsDirectionalKey(e.Key))
		{
			var focusArgs = new FocusNavigationEventArgs(e.Key);
			OnFocusNavigationRequested(focusArgs);
			if (focusArgs.Handled) e.Handled = true;
		}

		if (KeyDown == null) return;
		MainThread.BeginInvokeOnMainThread(() =>
		{
			try { KeyDown.Invoke(this, e); } catch { }
		});
	}

	protected virtual void OnKeyUp(DPadKeyEventArgs e)
	{
		if (KeyUp == null) return;
		MainThread.BeginInvokeOnMainThread(() =>
		{
			try { KeyUp.Invoke(this, e); } catch { }
		});
	}

	protected virtual void OnFocusNavigationRequested(FocusNavigationEventArgs e)
	{
		if (FocusNavigationRequested == null) return;
		MainThread.BeginInvokeOnMainThread(() =>
		{
			try { FocusNavigationRequested.Invoke(this, e); } catch { }
		});
	}

	protected static bool IsDirectionalKey(DPadKey key) => key == DPadKey.Up || key == DPadKey.Down || key == DPadKey.Left || key == DPadKey.Right;

	private partial void StartListeningPlatform();
	private partial void StopListeningPlatform();
	private partial void EnableFocusNavigationPlatform();
	private partial void DisableFocusNavigationPlatform();
	private partial bool GetIsSupported();
}
