namespace Plugin.Maui.TvDPad;

public interface IFeature
{
	/// <summary>
	/// Occurs when any D-Pad key is pressed down.
	/// </summary>
	event EventHandler<DPadKeyEventArgs>? KeyDown;

	/// <summary>
	/// Occurs when any D-Pad key is released.
	/// </summary>
	event EventHandler<DPadKeyEventArgs>? KeyUp;

	/// <summary>
	/// Occurs when focus navigation is requested via D-Pad directional keys.
	/// </summary>
	event EventHandler<FocusNavigationEventArgs>? FocusNavigationRequested;

	bool IsSupported { get; }
	bool IsListening { get; }
	void StartListening();
	void StopListening();
	void EnableFocusNavigation();
	void DisableFocusNavigation();
	bool IsFocusNavigationEnabled { get; }
}
