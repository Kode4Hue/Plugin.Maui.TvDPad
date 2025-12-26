namespace Plugin.Maui.DPad;

/// <summary>
/// Event arguments for focus navigation events.
/// </summary>
public class FocusNavigationEventArgs : EventArgs
{
	/// <summary>
	/// Gets the direction of the focus navigation.
	/// </summary>
	public DPadKey Direction { get; }

	/// <summary>
	/// Gets or sets a value indicating whether the default focus navigation should be handled.
	/// Set to true to handle navigation manually.
	/// </summary>
	public bool Handled { get; set; }

	/// <summary>
	/// Gets or sets the element that should receive focus.
	/// Set this to manually control which element gets focused.
	/// </summary>
	public object? NextFocusElement { get; set; }

	/// <summary>
	/// Initializes a new instance of the FocusNavigationEventArgs class.
	/// </summary>
	/// <param name="direction">The direction of the navigation.</param>
	public FocusNavigationEventArgs(DPadKey direction)
	{
		Direction = direction;
	}
}
