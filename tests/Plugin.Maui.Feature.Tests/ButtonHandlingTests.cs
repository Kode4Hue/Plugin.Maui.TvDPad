using Plugin.Maui.Feature;

namespace Plugin.Maui.Feature.Tests;

/// <summary>
/// Tests for common button handling (OK/Center/Select, Back, Menu, Enter, Microphone)
/// </summary>
public class ButtonHandlingTests
{
	[Fact]
	public void CenterButton_RaisesKeyEvents()
	{
		// Arrange
		var feature = new FeatureImplementation();
		var keyDownRaised = false;
		var keyUpRaised = false;

		feature.KeyDown += (sender, e) =>
		{
			if (e.Key == DPadKey.Center)
				keyDownRaised = true;
		};

		feature.KeyUp += (sender, e) =>
		{
			if (e.Key == DPadKey.Center)
				keyUpRaised = true;
		};

		// Act
		feature.SimulateKeyPress(DPadKey.Center);

		// Assert
		Assert.True(keyDownRaised, "Center button KeyDown should be raised");
		Assert.True(keyUpRaised, "Center button KeyUp should be raised");
	}

	[Fact]
	public void BackButton_RaisesKeyEvents()
	{
		// Arrange
		var feature = new FeatureImplementation();
		var keyDownRaised = false;

		feature.KeyDown += (sender, e) =>
		{
			if (e.Key == DPadKey.Back)
				keyDownRaised = true;
		};

		// Act
		feature.SimulateKeyPress(DPadKey.Back);

		// Assert
		Assert.True(keyDownRaised, "Back button should raise KeyDown event");
	}

	[Fact]
	public void MenuButton_RaisesKeyEvents()
	{
		// Arrange
		var feature = new FeatureImplementation();
		var keyDownRaised = false;

		feature.KeyDown += (sender, e) =>
		{
			if (e.Key == DPadKey.Menu)
				keyDownRaised = true;
		};

		// Act
		feature.SimulateKeyPress(DPadKey.Menu);

		// Assert
		Assert.True(keyDownRaised, "Menu button should raise KeyDown event");
	}

	[Fact]
	public void EnterButton_RaisesKeyEvents()
	{
		// Arrange
		var feature = new FeatureImplementation();
		var keyDownRaised = false;

		feature.KeyDown += (sender, e) =>
		{
			if (e.Key == DPadKey.Enter)
				keyDownRaised = true;
		};

		// Act
		feature.SimulateKeyPress(DPadKey.Enter);

		// Assert
		Assert.True(keyDownRaised, "Enter button should raise KeyDown event");
	}

	[Fact]
	public void MicrophoneButton_RaisesKeyEvents()
	{
		// Arrange
		var feature = new FeatureImplementation();
		var keyDownRaised = false;

		feature.KeyDown += (sender, e) =>
		{
			if (e.Key == DPadKey.Microphone)
				keyDownRaised = true;
		};

		// Act
		feature.SimulateKeyPress(DPadKey.Microphone);

		// Assert
		Assert.True(keyDownRaised, "Microphone button should raise KeyDown event");
	}

	[Theory]
	[InlineData(DPadKey.Center)]
	[InlineData(DPadKey.Back)]
	[InlineData(DPadKey.Menu)]
	[InlineData(DPadKey.Enter)]
	public void CommonButtons_CanBeHandled(DPadKey button)
	{
		// Arrange
		var feature = new FeatureImplementation();
		var wasHandled = false;

		feature.KeyDown += (sender, e) =>
		{
			if (e.Key == button)
			{
				e.Handled = true;
				wasHandled = e.Handled;
			}
		};

		// Act
		feature.SimulateKeyPress(button);

		// Assert
		Assert.True(wasHandled, $"{button} button should be handleable");
	}

	[Fact]
	public void CommonButtons_DoNotTriggerFocusNavigation()
	{
		// Arrange
		var feature = new FeatureImplementation();
		feature.EnableFocusNavigation();
		var focusEventRaised = false;

		feature.FocusNavigationRequested += (sender, e) =>
		{
			focusEventRaised = true;
		};

		// Act
		feature.SimulateKeyPress(DPadKey.Center);
		feature.SimulateKeyPress(DPadKey.Back);
		feature.SimulateKeyPress(DPadKey.Menu);
		feature.SimulateKeyPress(DPadKey.Enter);
		feature.SimulateKeyPress(DPadKey.Microphone);

		// Assert
		Assert.False(focusEventRaised, "Common buttons should not trigger focus navigation");
	}

	[Fact]
	public void MultipleButtonPresses_AllHandledIndependently()
	{
		// Arrange
		var feature = new FeatureImplementation();
		var centerPressed = false;
		var backPressed = false;
		var menuPressed = false;

		feature.KeyDown += (sender, e) =>
		{
			if (e.Key == DPadKey.Center) centerPressed = true;
			if (e.Key == DPadKey.Back) backPressed = true;
			if (e.Key == DPadKey.Menu) menuPressed = true;
		};

		// Act
		feature.SimulateKeyPress(DPadKey.Center);
		feature.SimulateKeyPress(DPadKey.Back);
		feature.SimulateKeyPress(DPadKey.Menu);

		// Assert
		Assert.True(centerPressed, "Center should be pressed");
		Assert.True(backPressed, "Back should be pressed");
		Assert.True(menuPressed, "Menu should be pressed");
	}
}
