namespace Plugin.Maui.Feature;

/// <summary>
/// Provides D-Pad navigation support for .NET MAUI applications.
/// Handles directional buttons (Up, Down, Left, Right) and common buttons
/// (OK/Center/Select, Back, Menu, Enter, Microphone).
/// </summary>
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

	/// <summary>
	/// Gets a value indicating whether D-Pad input is supported on this device.
	/// </summary>
	bool IsSupported { get; }

	/// <summary>
	/// Gets a value indicating whether the D-Pad listener is currently active.
	/// </summary>
	bool IsListening { get; }

	/// <summary>
	/// Starts listening for D-Pad key events.
	/// </summary>
	void StartListening();

	/// <summary>
	/// Stops listening for D-Pad key events.
	/// </summary>
	void StopListening();

	/// <summary>
	/// Enables automatic focus navigation when D-Pad directional keys are pressed.
	/// When enabled, the system will automatically move focus between focusable elements.
	/// </summary>
	void EnableFocusNavigation();

	/// <summary>
	/// Disables automatic focus navigation.
	/// </summary>
	void DisableFocusNavigation();

	/// <summary>
	/// Gets a value indicating whether automatic focus navigation is enabled.
	/// </summary>
	bool IsFocusNavigationEnabled { get; }
}