namespace Plugin.Maui.TvDPad;

partial class FeatureImplementation : IFeature
{
	private partial bool GetIsSupported() => false;

	private partial void StartListeningPlatform() { }

	private partial void StopListeningPlatform() { }

	private partial void EnableFocusNavigationPlatform() { }

	private partial void DisableFocusNavigationPlatform() { }

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
