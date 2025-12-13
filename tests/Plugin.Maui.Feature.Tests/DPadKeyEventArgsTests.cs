using Plugin.Maui.Feature;

namespace Plugin.Maui.Feature.Tests;

/// <summary>
/// Tests for DPadKeyEventArgs
/// </summary>
public class DPadKeyEventArgsTests
{
	[Fact]
	public void Constructor_InitializesPropertiesCorrectly()
	{
		// Arrange
		var key = DPadKey.Up;
		var isKeyDown = true;
		var beforeTimestamp = DateTimeOffset.UtcNow;

		// Act
		var args = new DPadKeyEventArgs(key, isKeyDown);
		var afterTimestamp = DateTimeOffset.UtcNow;

		// Assert
		Assert.Equal(key, args.Key);
		Assert.Equal(isKeyDown, args.IsKeyDown);
		Assert.False(args.Handled);
		Assert.InRange(args.Timestamp, beforeTimestamp, afterTimestamp);
	}

	[Theory]
	[InlineData(DPadKey.Up, true)]
	[InlineData(DPadKey.Down, false)]
	[InlineData(DPadKey.Center, true)]
	[InlineData(DPadKey.Back, false)]
	public void Constructor_AcceptsDifferentKeysAndStates(DPadKey key, bool isKeyDown)
	{
		// Act
		var args = new DPadKeyEventArgs(key, isKeyDown);

		// Assert
		Assert.Equal(key, args.Key);
		Assert.Equal(isKeyDown, args.IsKeyDown);
	}

	[Fact]
	public void Handled_CanBeSetToTrue()
	{
		// Arrange
		var args = new DPadKeyEventArgs(DPadKey.Enter, true);

		// Act
		args.Handled = true;

		// Assert
		Assert.True(args.Handled);
	}

	[Fact]
	public void Handled_DefaultsToFalse()
	{
		// Arrange & Act
		var args = new DPadKeyEventArgs(DPadKey.Menu, false);

		// Assert
		Assert.False(args.Handled);
	}
}
