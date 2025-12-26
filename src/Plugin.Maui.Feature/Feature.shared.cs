using Microsoft.Maui.ApplicationModel;

namespace Plugin.Maui.Feature;

/// <summary>
/// Provides static access to the D-Pad navigation feature.
/// </summary>
public static class Feature
{
	static IFeature? defaultImplementation;

	/// <summary>
	/// Provides the default implementation for static usage of this API.
	/// </summary>
	public static IFeature Default =>
		defaultImplementation ??= new FeatureImplementation();

	internal static void SetDefault(IFeature? implementation) =>
		defaultImplementation = implementation;
}

/// <summary>
/// Base implementation of the D-Pad feature interface.
/// </summary>
public partial class FeatureImplementation : IFeature
{
	bool isListening;
	bool isFocusNavigationEnabled;

	/// <inheritdoc/>
	public event EventHandler<DPadKeyEventArgs>? KeyDown;

	/// <inheritdoc/>
	public event EventHandler<DPadKeyEventArgs>? KeyUp;

	/// <inheritdoc/>
	public event EventHandler<FocusNavigationEventArgs>? FocusNavigationRequested;

	/// <inheritdoc/>
	public virtual bool IsSupported => GetIsSupported();

	/// <inheritdoc/>
	public bool IsListening => isListening;

	/// <inheritdoc/>
	public bool IsFocusNavigationEnabled => isFocusNavigationEnabled;

	/// <inheritdoc/>
	public void StartListening()
	{
		if (isListening)
			return;

		isListening = true;
		StartListeningPlatform();
	}

	/// <inheritdoc/>
	public void StopListening()
	{
		if (!isListening)
			return;

		isListening = false;
		StopListeningPlatform();
	}

	/// <inheritdoc/>
	public void EnableFocusNavigation()
	{
		if (isFocusNavigationEnabled)
			return;

		isFocusNavigationEnabled = true;
		EnableFocusNavigationPlatform();
	}

	/// <inheritdoc/>
	public void DisableFocusNavigation()
	{
		if (!isFocusNavigationEnabled)
			return;

		isFocusNavigationEnabled = false;
		DisableFocusNavigationPlatform();
	}

	/// <summary>
	/// Raises the KeyDown event.
	/// </summary>
	protected virtual void OnKeyDown(DPadKeyEventArgs e)
	{
		// If focus navigation is enabled and it's a directional key, trigger focus navigation first
		if (isFocusNavigationEnabled && IsDirectionalKey(e.Key))
		{
			var focusArgs = new FocusNavigationEventArgs(e.Key);
			OnFocusNavigationRequested(focusArgs);
			
			if (focusArgs.Handled)
			{
				e.Handled = true;
			}
		}

		if (KeyDown == null)
			return;

		// Invoke handlers on the main thread asynchronously to avoid deadlocks and cross-thread issues
		MainThread.BeginInvokeOnMainThread(() =>
		{
			try
			{
				KeyDown.Invoke(this, e);
			}
			catch
			{
				// Swallow to prevent UI crashes from handler exceptions
			}
		});
	}

	/// <summary>
	/// Raises the KeyUp event.
	/// </summary>
	protected virtual void OnKeyUp(DPadKeyEventArgs e)
	{
		if (KeyUp == null)
			return;

		MainThread.BeginInvokeOnMainThread(() =>
		{
			try
			{
				KeyUp.Invoke(this, e);
			}
			catch { }
		});
	}

	/// <summary>
	/// Raises the FocusNavigationRequested event.
	/// </summary>
	protected virtual void OnFocusNavigationRequested(FocusNavigationEventArgs e)
	{
		if (FocusNavigationRequested == null)
			return;

		MainThread.BeginInvokeOnMainThread(() =>
		{
			try { FocusNavigationRequested.Invoke(this, e); } catch { }
		});
	}

	/// <summary>
	/// Determines if a key is a directional key.
	/// </summary>
	protected static bool IsDirectionalKey(DPadKey key)
	{
		return key == DPadKey.Up || key == DPadKey.Down || 
		       key == DPadKey.Left || key == DPadKey.Right;
	}

	// Platform-specific methods to be implemented
	private partial void StartListeningPlatform();
	private partial void StopListeningPlatform();
	private partial void EnableFocusNavigationPlatform();
	private partial void DisableFocusNavigationPlatform();
	private partial bool GetIsSupported();
}
