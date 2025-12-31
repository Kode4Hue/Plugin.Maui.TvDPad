using Plugin.Maui.Feature;

namespace Plugin.Maui.Feature.Tests;

/// <summary>
/// Tests for FocusNavigationEventArgs
/// </summary>
public class FocusNavigationEventArgsTests
{
	[Theory]
	[InlineData(DPadKey.Up)]
	[InlineData(DPadKey.Down)]
	[InlineData(DPadKey.Left)]
	[InlineData(DPadKey.Right)]
	public void Constructor_InitializesDirectionCorrectly(DPadKey direction)
	{
		// Act
		var args = new FocusNavigationEventArgs(direction);

		// Assert
		Assert.Equal(direction, args.Direction);
		Assert.False(args.Handled);
		Assert.Null(args.NextFocusElement);
	}

	[Fact]
	public void Handled_CanBeSetToTrue()
	{
		// Arrange
		var args = new FocusNavigationEventArgs(DPadKey.Up);

		// Act
		args.Handled = true;

		// Assert
		Assert.True(args.Handled);
	}

	[Fact]
	public void NextFocusElement_CanBeSet()
	{
		// Arrange
		var args = new FocusNavigationEventArgs(DPadKey.Down);
		var element = new object();

		// Act
		args.NextFocusElement = element;

		// Assert
		Assert.Same(element, args.NextFocusElement);
	}

	[Fact]
	public void NextFocusElement_DefaultsToNull()
	{
		// Arrange & Act
		var args = new FocusNavigationEventArgs(DPadKey.Left);

		// Assert
		Assert.Null(args.NextFocusElement);
	}
}
