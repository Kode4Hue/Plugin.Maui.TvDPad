using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Views;

namespace Plugin.Maui.Feature.Sample;

[Activity(Theme = "@style/Maui.SplashTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density)]
public class MainActivity : MauiAppCompatActivity
{
	public override bool OnKeyDown(Keycode keyCode, KeyEvent? e)
	{
		// Get the feature implementation and handle the key
		var feature = Feature.Default as FeatureImplementation;
		if (feature != null && feature.HandleKeyDown(keyCode, e))
		{
			return true;
		}

		return base.OnKeyDown(keyCode, e);
	}

	public override bool OnKeyUp(Keycode keyCode, KeyEvent? e)
	{
		// Get the feature implementation and handle the key
		var feature = Feature.Default as FeatureImplementation;
		if (feature != null && feature.HandleKeyUp(keyCode, e))
		{
			return true;
		}

		return base.OnKeyUp(keyCode, e);
	}
}