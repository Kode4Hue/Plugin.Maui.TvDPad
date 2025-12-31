using Plugin.Maui.Feature;

namespace Plugin.Maui.Feature.Tests;

/// <summary>
/// Tests for item selection use cases
/// </summary>
public class ItemSelectionTests
{
	[Fact]
	public void NavigateAndSelect_SimulatesUserBehavior()
	{
		// Arrange
		var feature = new FeatureImplementation();
		feature.EnableFocusNavigation();
		
		var upPressed = false;
		var downPressed = false;
		var centerPressed = false;
		var focusChanges = 0;

		feature.KeyDown += (sender, e) =>
		{
			if (e.Key == DPadKey.Up) upPressed = true;
			if (e.Key == DPadKey.Down) downPressed = true;
			if (e.Key == DPadKey.Center) centerPressed = true;
		};

		feature.FocusNavigationRequested += (sender, e) =>
		{
			focusChanges++;
		};

		// Act - Simulate navigating through a list and selecting
		feature.SimulateKeyPress(DPadKey.Down);  // Move down
		feature.SimulateKeyPress(DPadKey.Down);  // Move down
		feature.SimulateKeyPress(DPadKey.Up);    // Move up
		feature.SimulateKeyPress(DPadKey.Center); // Select

		// Assert
		Assert.True(upPressed, "Up key should be pressed");
		Assert.True(downPressed, "Down key should be pressed");
		Assert.True(centerPressed, "Center key should be pressed for selection");
		Assert.Equal(3, focusChanges); // Up, Down, Down should trigger focus changes (Center shouldn't)
	}

	[Fact]
	public void EnterKey_CanAlsoSelectItems()
	{
		// Arrange
		var feature = new FeatureImplementation();
		var enterPressed = false;

		feature.KeyDown += (sender, e) =>
		{
			if (e.Key == DPadKey.Enter)
				enterPressed = true;
		};

		// Act
		feature.SimulateKeyPress(DPadKey.Enter);

		// Assert
		Assert.True(enterPressed, "Enter key can be used for item selection");
	}

	[Fact]
	public void HorizontalNavigation_WorksForItemSelection()
	{
		// Arrange
		var feature = new FeatureImplementation();
		feature.EnableFocusNavigation();
		
		var leftPressed = false;
		var rightPressed = false;
		var focusChanges = 0;

		feature.KeyDown += (sender, e) =>
		{
			if (e.Key == DPadKey.Left) leftPressed = true;
			if (e.Key == DPadKey.Right) rightPressed = true;
		};

		feature.FocusNavigationRequested += (sender, e) =>
		{
			focusChanges++;
		};

		// Act
		feature.SimulateKeyPress(DPadKey.Right);
		feature.SimulateKeyPress(DPadKey.Left);

		// Assert
		Assert.True(leftPressed, "Left key should be pressed");
		Assert.True(rightPressed, "Right key should be pressed");
		Assert.Equal(2, focusChanges);
	}

	[Fact]
	public void SelectionWithoutFocusNavigation_StillWorksForButtons()
	{
		// Arrange
		var feature = new FeatureImplementation();
		var centerPressed = false;

		feature.KeyDown += (sender, e) =>
		{
			if (e.Key == DPadKey.Center)
				centerPressed = true;
		};

		// Act
		feature.SimulateKeyPress(DPadKey.Center);

		// Assert
		Assert.True(centerPressed, "Selection buttons work even without focus navigation enabled");
	}

	[Fact]
	public void ComplexSelectionScenario_AllKeysWork()
	{
		// Arrange
		var feature = new FeatureImplementation();
		feature.EnableFocusNavigation();
		
		var keysPressed = new List<DPadKey>();

		feature.KeyDown += (sender, e) =>
		{
			keysPressed.Add(e.Key);
		};

		// Act - Simulate a complex navigation and selection
		feature.SimulateKeyPress(DPadKey.Down);
		feature.SimulateKeyPress(DPadKey.Down);
		feature.SimulateKeyPress(DPadKey.Right);
		feature.SimulateKeyPress(DPadKey.Up);
		feature.SimulateKeyPress(DPadKey.Enter);

		// Assert
		Assert.Equal(5, keysPressed.Count);
		Assert.Contains(DPadKey.Down, keysPressed);
		Assert.Contains(DPadKey.Right, keysPressed);
		Assert.Contains(DPadKey.Up, keysPressed);
		Assert.Contains(DPadKey.Enter, keysPressed);
	}
}
