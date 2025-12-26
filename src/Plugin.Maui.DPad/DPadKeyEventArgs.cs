namespace Plugin.Maui.DPad;

/// <summary>
/// Event arguments for D-Pad key events.
/// </summary>
public class DPadKeyEventArgs : EventArgs
{
	/// <summary>
	/// Gets the D-Pad key that was pressed.
	/// </summary>
	public DPadKey Key { get; }

	/// <summary>
	/// Gets a value indicating whether the key press was handled.
	/// Set this to true to prevent default behavior.
	/// </summary>
	public bool Handled { get; set; }

	/// <summary>
	/// Gets a value indicating whether this is a key down event (true) or key up event (false).
	/// </summary>
	public bool IsKeyDown { get; }

	/// <summary>
	/// Gets the timestamp when the key event occurred.
	/// </summary>
	public DateTimeOffset Timestamp { get; }

	/// <summary>
	/// Initializes a new instance of the DPadKeyEventArgs class.
	/// </summary>
	/// <param name="key">The D-Pad key that was pressed.</param>
	/// <param name="isKeyDown">Whether this is a key down event.</param>
	public DPadKeyEventArgs(DPadKey key, bool isKeyDown)
	{
		Key = key;
		IsKeyDown = isKeyDown;
		Timestamp = DateTimeOffset.UtcNow;
	}
}
