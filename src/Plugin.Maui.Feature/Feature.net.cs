namespace Plugin.Maui.Feature;

partial class FeatureImplementation : IFeature
{
	private partial bool GetIsSupported()
	{
		// Generic .NET target - typically used for unit testing
		// Report as supported to allow testing
		return true;
	}

	private partial void StartListeningPlatform()
	{
		// No platform-specific implementation for generic .NET
	}

	private partial void StopListeningPlatform()
	{
		// No platform-specific implementation for generic .NET
	}

	private partial void EnableFocusNavigationPlatform()
	{
		// No platform-specific implementation for generic .NET
	}

	private partial void DisableFocusNavigationPlatform()
	{
		// No platform-specific implementation for generic .NET
	}

	/// <summary>
	/// Simulates a key press for testing purposes.
	/// </summary>
	public void SimulateKeyPress(DPadKey key)
	{
		var downArgs = new DPadKeyEventArgs(key, isKeyDown: true);
		OnKeyDown(downArgs);

		var upArgs = new DPadKeyEventArgs(key, isKeyDown: false);
		OnKeyUp(upArgs);
	}
}
