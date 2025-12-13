using Plugin.Maui.Feature;

namespace Plugin.Maui.Feature.Tests;

/// <summary>
/// Tests for FeatureImplementation core functionality
/// </summary>
public class FeatureImplementationTests
{
	[Fact]
	public void Default_ReturnsNonNullInstance()
	{
		// Act
		var feature = Feature.Default;

		// Assert
		Assert.NotNull(feature);
	}

	[Fact]
	public void Default_ReturnsSameInstanceOnMultipleCalls()
	{
		// Act
		var feature1 = Feature.Default;
		var feature2 = Feature.Default;

		// Assert
		Assert.Same(feature1, feature2);
	}

	[Fact]
	public void IsSupported_ReturnsTrue()
	{
		// Arrange
		var feature = Feature.Default;

		// Act
		var isSupported = feature.IsSupported;

		// Assert
		Assert.True(isSupported);
	}

	[Fact]
	public void IsListening_DefaultsToFalse()
	{
		// Arrange
		var feature = new FeatureImplementation();

		// Act
		var isListening = feature.IsListening;

		// Assert
		Assert.False(isListening);
	}

	[Fact]
	public void IsFocusNavigationEnabled_DefaultsToFalse()
	{
		// Arrange
		var feature = new FeatureImplementation();

		// Act
		var isEnabled = feature.IsFocusNavigationEnabled;

		// Assert
		Assert.False(isEnabled);
	}

	[Fact]
	public void StartListening_SetsIsListeningToTrue()
	{
		// Arrange
		var feature = new FeatureImplementation();

		// Act
		feature.StartListening();

		// Assert
		Assert.True(feature.IsListening);
	}

	[Fact]
	public void StopListening_SetsIsListeningToFalse()
	{
		// Arrange
		var feature = new FeatureImplementation();
		feature.StartListening();

		// Act
		feature.StopListening();

		// Assert
		Assert.False(feature.IsListening);
	}

	[Fact]
	public void EnableFocusNavigation_SetsIsFocusNavigationEnabledToTrue()
	{
		// Arrange
		var feature = new FeatureImplementation();

		// Act
		feature.EnableFocusNavigation();

		// Assert
		Assert.True(feature.IsFocusNavigationEnabled);
	}

	[Fact]
	public void DisableFocusNavigation_SetsIsFocusNavigationEnabledToFalse()
	{
		// Arrange
		var feature = new FeatureImplementation();
		feature.EnableFocusNavigation();

		// Act
		feature.DisableFocusNavigation();

		// Assert
		Assert.False(feature.IsFocusNavigationEnabled);
	}

	[Fact]
	public void StartListening_CalledTwice_RemainsListening()
	{
		// Arrange
		var feature = new FeatureImplementation();

		// Act
		feature.StartListening();
		feature.StartListening();

		// Assert
		Assert.True(feature.IsListening);
	}

	[Fact]
	public void StopListening_CalledTwice_RemainsNotListening()
	{
		// Arrange
		var feature = new FeatureImplementation();
		feature.StartListening();

		// Act
		feature.StopListening();
		feature.StopListening();

		// Assert
		Assert.False(feature.IsListening);
	}
}
