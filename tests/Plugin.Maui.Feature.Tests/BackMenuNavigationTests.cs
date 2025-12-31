using Plugin.Maui.Feature;

namespace Plugin.Maui.Feature.Tests;

/// <summary>
/// Tests for back/menu navigation use cases
/// </summary>
public class BackMenuNavigationTests
{
	[Fact]
	public void BackButton_CanBeHandledForNavigation()
	{
		// Arrange
		var feature = new FeatureImplementation();
		var backPressed = false;
		var navigationHandled = false;

		feature.KeyDown += (sender, e) =>
		{
			if (e.Key == DPadKey.Back)
			{
				backPressed = true;
				e.Handled = true;
				navigationHandled = true;
			}
		};

		// Act
		feature.SimulateKeyPress(DPadKey.Back);

		// Assert
		Assert.True(backPressed, "Back button should be pressed");
		Assert.True(navigationHandled, "Back navigation should be handleable");
	}

	[Fact]
	public void MenuButton_CanBeHandledForNavigation()
	{
		// Arrange
		var feature = new FeatureImplementation();
		var menuPressed = false;
		var navigationHandled = false;

		feature.KeyDown += (sender, e) =>
		{
			if (e.Key == DPadKey.Menu)
			{
				menuPressed = true;
				e.Handled = true;
				navigationHandled = true;
			}
		};

		// Act
		feature.SimulateKeyPress(DPadKey.Menu);

		// Assert
		Assert.True(menuPressed, "Menu button should be pressed");
		Assert.True(navigationHandled, "Menu navigation should be handleable");
	}

	[Fact]
	public void BackButton_RaisesEventBeforeFocusNavigation()
	{
		// Arrange
		var feature = new FeatureImplementation();
		feature.EnableFocusNavigation();
		var backPressed = false;
		var focusNavigationRequested = false;

		feature.KeyDown += (sender, e) =>
		{
			if (e.Key == DPadKey.Back)
				backPressed = true;
		};

		feature.FocusNavigationRequested += (sender, e) =>
		{
			focusNavigationRequested = true;
		};

		// Act
		feature.SimulateKeyPress(DPadKey.Back);

		// Assert
		Assert.True(backPressed, "Back button should trigger KeyDown");
		Assert.False(focusNavigationRequested, "Back should not trigger focus navigation");
	}

	[Fact]
	public void MenuButton_RaisesEventBeforeFocusNavigation()
	{
		// Arrange
		var feature = new FeatureImplementation();
		feature.EnableFocusNavigation();
		var menuPressed = false;
		var focusNavigationRequested = false;

		feature.KeyDown += (sender, e) =>
		{
			if (e.Key == DPadKey.Menu)
				menuPressed = true;
		};

		feature.FocusNavigationRequested += (sender, e) =>
		{
			focusNavigationRequested = true;
		};

		// Act
		feature.SimulateKeyPress(DPadKey.Menu);

		// Assert
		Assert.True(menuPressed, "Menu button should trigger KeyDown");
		Assert.False(focusNavigationRequested, "Menu should not trigger focus navigation");
	}

	[Fact]
	public void BackAndMenu_CanBothBeUsedInSameSession()
	{
		// Arrange
		var feature = new FeatureImplementation();
		var backPressed = false;
		var menuPressed = false;

		feature.KeyDown += (sender, e) =>
		{
			if (e.Key == DPadKey.Back) backPressed = true;
			if (e.Key == DPadKey.Menu) menuPressed = true;
		};

		// Act
		feature.SimulateKeyPress(DPadKey.Back);
		feature.SimulateKeyPress(DPadKey.Menu);

		// Assert
		Assert.True(backPressed, "Back button should work");
		Assert.True(menuPressed, "Menu button should work");
	}

	[Fact]
	public void NavigationButtons_ProvideSeparateEventsFromDirectionalKeys()
	{
		// Arrange
		var feature = new FeatureImplementation();
		feature.EnableFocusNavigation();
		
		var backPressed = false;
		var upPressed = false;
		var focusNavigationCount = 0;

		feature.KeyDown += (sender, e) =>
		{
			if (e.Key == DPadKey.Back) backPressed = true;
			if (e.Key == DPadKey.Up) upPressed = true;
		};

		feature.FocusNavigationRequested += (sender, e) =>
		{
			focusNavigationCount++;
		};

		// Act
		feature.SimulateKeyPress(DPadKey.Up);
		feature.SimulateKeyPress(DPadKey.Back);

		// Assert
		Assert.True(upPressed, "Up should be pressed");
		Assert.True(backPressed, "Back should be pressed");
		Assert.Equal(1, focusNavigationCount); // Only Up should trigger focus navigation
	}

	[Fact]
	public void CompleteNavigationScenario_WorksCorrectly()
	{
		// Arrange
		var feature = new FeatureImplementation();
		feature.EnableFocusNavigation();
		
		var navigationSequence = new List<DPadKey>();

		feature.KeyDown += (sender, e) =>
		{
			navigationSequence.Add(e.Key);
		};

		// Act - Simulate: navigate, select, then go back
		feature.SimulateKeyPress(DPadKey.Down);
		feature.SimulateKeyPress(DPadKey.Enter);
		feature.SimulateKeyPress(DPadKey.Back);

		// Assert
		Assert.Equal(3, navigationSequence.Count);
		Assert.Equal(DPadKey.Down, navigationSequence[0]);
		Assert.Equal(DPadKey.Enter, navigationSequence[1]);
		Assert.Equal(DPadKey.Back, navigationSequence[2]);
	}
}
