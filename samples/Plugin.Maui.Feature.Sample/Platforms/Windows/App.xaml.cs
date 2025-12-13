using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Input;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Plugin.Maui.Feature.Sample.WinUI;

/// <summary>
/// Provides application-specific behavior to supplement the default Application class.
/// </summary>
public partial class App : MauiWinUIApplication
{
	/// <summary>
	/// Initializes the singleton application object.  This is the first line of authored code
	/// executed, and as such is the logical equivalent of main() or WinMain().
	/// </summary>
	public App()
	{
		this.InitializeComponent();
	}

	protected override MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp();

	protected override void OnLaunched(Microsoft.UI.Xaml.LaunchActivatedEventArgs args)
	{
		base.OnLaunched(args);

		// Hook up keyboard event handlers after the window is created
		// Wait a bit for the window to be fully initialized
		System.Threading.Timer? timer = null;
		timer = new System.Threading.Timer(_ =>
		{
			Microsoft.Maui.ApplicationModel.MainThread.BeginInvokeOnMainThread(() =>
			{
				var window = Microsoft.Maui.Controls.Application.Current?.Windows[0]?.Handler?.PlatformView as Microsoft.UI.Xaml.Window;
				if (window?.Content is UIElement content)
				{
					content.KeyDown += OnKeyDown;
					content.KeyUp += OnKeyUp;
				}
				
				// Dispose timer after one-time execution
				timer?.Dispose();
			});
		}, null, 500, System.Threading.Timeout.Infinite);
	}

	private void OnKeyDown(object sender, KeyRoutedEventArgs e)
	{
		var feature = Plugin.Maui.Feature.Feature.Default as Plugin.Maui.Feature.FeatureImplementation;
		feature?.HandleKeyRoutedEvent(e, isKeyDown: true);
	}

	private void OnKeyUp(object sender, KeyRoutedEventArgs e)
	{
		var feature = Plugin.Maui.Feature.Feature.Default as Plugin.Maui.Feature.FeatureImplementation;
		feature?.HandleKeyRoutedEvent(e, isKeyDown: false);
	}
}


