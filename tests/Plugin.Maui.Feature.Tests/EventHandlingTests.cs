using Plugin.Maui.Feature;

namespace Plugin.Maui.Feature.Tests;

/// <summary>
/// Tests for D-Pad event handling
/// </summary>
public class EventHandlingTests
{
	[Fact]
	public void SimulateKeyPress_RaisesKeyDownAndKeyUpEvents()
	{
		// Arrange
		var feature = new FeatureImplementation();
		var keyDownRaised = false;
		var keyUpRaised = false;
		DPadKey? keyDownKey = null;
		DPadKey? keyUpKey = null;

		feature.KeyDown += (sender, e) =>
		{
			keyDownRaised = true;
			keyDownKey = e.Key;
		};

		feature.KeyUp += (sender, e) =>
		{
			keyUpRaised = true;
			keyUpKey = e.Key;
		};

		// Act
		feature.SimulateKeyPress(DPadKey.Up);

		// Assert
		Assert.True(keyDownRaised, "KeyDown event should be raised");
		Assert.True(keyUpRaised, "KeyUp event should be raised");
		Assert.Equal(DPadKey.Up, keyDownKey);
		Assert.Equal(DPadKey.Up, keyUpKey);
	}

	[Theory]
	[InlineData(DPadKey.Up)]
	[InlineData(DPadKey.Down)]
	[InlineData(DPadKey.Left)]
	[InlineData(DPadKey.Right)]
	[InlineData(DPadKey.Center)]
	[InlineData(DPadKey.Back)]
	[InlineData(DPadKey.Menu)]
	[InlineData(DPadKey.Enter)]
	[InlineData(DPadKey.Microphone)]
	public void SimulateKeyPress_WorksForAllKeys(DPadKey key)
	{
		// Arrange
		var feature = new FeatureImplementation();
		var eventRaised = false;
		DPadKey? raisedKey = null;

		feature.KeyDown += (sender, e) =>
		{
			eventRaised = true;
			raisedKey = e.Key;
		};

		// Act
		feature.SimulateKeyPress(key);

		// Assert
		Assert.True(eventRaised);
		Assert.Equal(key, raisedKey);
	}

	[Fact]
	public void KeyDown_CanBeHandled()
	{
		// Arrange
		var feature = new FeatureImplementation();
		var wasHandled = false;

		feature.KeyDown += (sender, e) =>
		{
			e.Handled = true;
			wasHandled = e.Handled;
		};

		// Act
		feature.SimulateKeyPress(DPadKey.Center);

		// Assert
		Assert.True(wasHandled);
	}

	[Fact]
	public void KeyUp_ProvidesCorrectEventArgs()
	{
		// Arrange
		var feature = new FeatureImplementation();
		DPadKeyEventArgs? capturedArgs = null;

		feature.KeyUp += (sender, e) =>
		{
			capturedArgs = e;
		};

		// Act
		feature.SimulateKeyPress(DPadKey.Enter);

		// Assert
		Assert.NotNull(capturedArgs);
		Assert.Equal(DPadKey.Enter, capturedArgs.Key);
		Assert.False(capturedArgs.IsKeyDown);
	}

	[Fact]
	public void KeyDown_ProvidesCorrectEventArgs()
	{
		// Arrange
		var feature = new FeatureImplementation();
		DPadKeyEventArgs? capturedArgs = null;

		feature.KeyDown += (sender, e) =>
		{
			capturedArgs = e;
		};

		// Act
		feature.SimulateKeyPress(DPadKey.Back);

		// Assert
		Assert.NotNull(capturedArgs);
		Assert.Equal(DPadKey.Back, capturedArgs.Key);
		Assert.True(capturedArgs.IsKeyDown);
	}
}
