using UIKit;

namespace Plugin.Maui.Feature;

partial class FeatureImplementation : IFeature
{
	private partial bool GetIsSupported()
	{
		// D-Pad support is available on iOS/tvOS via game controllers and keyboards
		// tvOS has native focus engine support
		return true;
	}

	private partial void StartListeningPlatform()
	{
		// On iOS/tvOS, key commands and focus events are handled through
		// UIKeyCommand or the focus engine
	}

	private partial void StopListeningPlatform()
	{
		// Platform-specific cleanup if needed
	}

	private partial void EnableFocusNavigationPlatform()
	{
		// Focus navigation on iOS/tvOS is handled by the native focus engine
	}

	private partial void DisableFocusNavigationPlatform()
	{
		// Platform-specific cleanup if needed
	}

	/// <summary>
	/// Handles iOS/tvOS key commands and converts them to D-Pad events.
	/// This should be called from UIKeyCommand handlers.
	/// </summary>
	public bool HandleKeyCommand(UIKeyCommand keyCommand)
	{
		var dpadKey = MapUIKeyCommand(keyCommand);
		if (dpadKey == null)
			return false;

		// Simulate key down and immediate key up for key commands
		var downArgs = new DPadKeyEventArgs(dpadKey.Value, isKeyDown: true);
		OnKeyDown(downArgs);

		var upArgs = new DPadKeyEventArgs(dpadKey.Value, isKeyDown: false);
		OnKeyUp(upArgs);

		return downArgs.Handled || upArgs.Handled;
	}

	/// <summary>
	/// Handles iOS/tvOS press events (for remote/game controller).
	/// </summary>
	public void HandlePress(DPadKey key)
	{
		var downArgs = new DPadKeyEventArgs(key, isKeyDown: true);
		OnKeyDown(downArgs);

		var upArgs = new DPadKeyEventArgs(key, isKeyDown: false);
		OnKeyUp(upArgs);
	}

	static DPadKey? MapUIKeyCommand(UIKeyCommand keyCommand)
	{
		if (keyCommand.Input == null)
			return null;

		return keyCommand.Input switch
		{
			UIKeyCommand.UpArrow => DPadKey.Up,
			UIKeyCommand.DownArrow => DPadKey.Down,
			UIKeyCommand.LeftArrow => DPadKey.Left,
			UIKeyCommand.RightArrow => DPadKey.Right,
			"\r" or "\n" => DPadKey.Enter,
			UIKeyCommand.Escape => DPadKey.Back,
			_ => null
		};
	}
}
