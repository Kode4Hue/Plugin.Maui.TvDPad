using Plugin.Maui.Feature;

namespace Plugin.Maui.Feature.Tests;

/// <summary>
/// Tests for focus navigation functionality
/// </summary>
public class FocusNavigationTests
{
	[Theory]
	[InlineData(DPadKey.Up)]
	[InlineData(DPadKey.Down)]
	[InlineData(DPadKey.Left)]
	[InlineData(DPadKey.Right)]
	public void DirectionalKey_WithFocusNavigationEnabled_RaisesFocusNavigationEvent(DPadKey direction)
	{
		// Arrange
		var feature = new FeatureImplementation();
		feature.EnableFocusNavigation();
		var focusEventRaised = false;
		DPadKey? capturedDirection = null;

		feature.FocusNavigationRequested += (sender, e) =>
		{
			focusEventRaised = true;
			capturedDirection = e.Direction;
		};

		// Act
		feature.SimulateKeyPress(direction);

		// Assert
		Assert.True(focusEventRaised);
		Assert.Equal(direction, capturedDirection);
	}

	[Theory]
	[InlineData(DPadKey.Center)]
	[InlineData(DPadKey.Back)]
	[InlineData(DPadKey.Menu)]
	[InlineData(DPadKey.Enter)]
	[InlineData(DPadKey.Microphone)]
	public void NonDirectionalKey_WithFocusNavigationEnabled_DoesNotRaiseFocusNavigationEvent(DPadKey key)
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
		feature.SimulateKeyPress(key);

		// Assert
		Assert.False(focusEventRaised);
	}

	[Fact]
	public void DirectionalKey_WithFocusNavigationDisabled_DoesNotRaiseFocusNavigationEvent()
	{
		// Arrange
		var feature = new FeatureImplementation();
		var focusEventRaised = false;

		feature.FocusNavigationRequested += (sender, e) =>
		{
			focusEventRaised = true;
		};

		// Act
		feature.SimulateKeyPress(DPadKey.Up);

		// Assert
		Assert.False(focusEventRaised);
	}

	[Fact]
	public void FocusNavigationEvent_CanBeHandled()
	{
		// Arrange
		var feature = new FeatureImplementation();
		feature.EnableFocusNavigation();
		var wasHandled = false;

		feature.FocusNavigationRequested += (sender, e) =>
		{
			e.Handled = true;
			wasHandled = e.Handled;
		};

		// Act
		feature.SimulateKeyPress(DPadKey.Down);

		// Assert
		Assert.True(wasHandled);
	}

	[Fact]
	public void FocusNavigationEvent_CanSetNextFocusElement()
	{
		// Arrange
		var feature = new FeatureImplementation();
		feature.EnableFocusNavigation();
		var customElement = new object();
		object? capturedElement = null;

		feature.FocusNavigationRequested += (sender, e) =>
		{
			e.NextFocusElement = customElement;
			capturedElement = e.NextFocusElement;
		};

		// Act
		feature.SimulateKeyPress(DPadKey.Right);

		// Assert
		Assert.Same(customElement, capturedElement);
	}

	[Fact]
	public void FocusNavigation_EnabledThenDisabled_NoLongerRaisesEvents()
	{
		// Arrange
		var feature = new FeatureImplementation();
		feature.EnableFocusNavigation();
		feature.DisableFocusNavigation();
		var focusEventRaised = false;

		feature.FocusNavigationRequested += (sender, e) =>
		{
			focusEventRaised = true;
		};

		// Act
		feature.SimulateKeyPress(DPadKey.Left);

		// Assert
		Assert.False(focusEventRaised);
	}

	[Fact]
	public void HandledFocusNavigation_SetsKeyEventHandledToTrue()
	{
		// Arrange
		var feature = new FeatureImplementation();
		feature.EnableFocusNavigation();
		var keyEventHandled = false;

		feature.FocusNavigationRequested += (sender, e) =>
		{
			e.Handled = true;
		};

		feature.KeyDown += (sender, e) =>
		{
			keyEventHandled = e.Handled;
		};

		// Act
		feature.SimulateKeyPress(DPadKey.Up);

		// Assert
		Assert.True(keyEventHandled);
	}
}
