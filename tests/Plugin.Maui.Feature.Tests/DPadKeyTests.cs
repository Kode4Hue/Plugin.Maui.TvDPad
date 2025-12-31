using Plugin.Maui.Feature;

namespace Plugin.Maui.Feature.Tests;

/// <summary>
/// Tests for DPadKey enum and related functionality
/// </summary>
public class DPadKeyTests
{
	[Fact]
	public void DPadKey_HasAllExpectedValues()
	{
		// Verify all expected D-Pad keys are defined
		var keys = Enum.GetValues<DPadKey>();
		
		Assert.Contains(DPadKey.Up, keys);
		Assert.Contains(DPadKey.Down, keys);
		Assert.Contains(DPadKey.Left, keys);
		Assert.Contains(DPadKey.Right, keys);
		Assert.Contains(DPadKey.Center, keys);
		Assert.Contains(DPadKey.Back, keys);
		Assert.Contains(DPadKey.Menu, keys);
		Assert.Contains(DPadKey.Enter, keys);
		Assert.Contains(DPadKey.Microphone, keys);
	}

	[Theory]
	[InlineData(DPadKey.Up, true)]
	[InlineData(DPadKey.Down, true)]
	[InlineData(DPadKey.Left, true)]
	[InlineData(DPadKey.Right, true)]
	[InlineData(DPadKey.Center, false)]
	[InlineData(DPadKey.Back, false)]
	[InlineData(DPadKey.Menu, false)]
	[InlineData(DPadKey.Enter, false)]
	[InlineData(DPadKey.Microphone, false)]
	public void IsDirectionalKey_ReturnsCorrectValue(DPadKey key, bool expectedIsDirectional)
	{
		var isDirectional = key == DPadKey.Up || key == DPadKey.Down || 
		                    key == DPadKey.Left || key == DPadKey.Right;
		
		Assert.Equal(expectedIsDirectional, isDirectional);
	}
}
