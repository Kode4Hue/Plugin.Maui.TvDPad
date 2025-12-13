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

	protected override MauiApp CreateMauiApp()
	{
		var mauiApp = MauiProgram.CreateMauiApp();
		
		// Hook up keyboard event handlers for the main window
		var window = Microsoft.Maui.Controls.Application.Current?.Windows[0]?.Handler?.PlatformView as Microsoft.UI.Xaml.Window;
		if (window != null)
		{
			window.Content.KeyDown += OnKeyDown;
			window.Content.KeyUp += OnKeyUp;
		}

		return mauiApp;
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


