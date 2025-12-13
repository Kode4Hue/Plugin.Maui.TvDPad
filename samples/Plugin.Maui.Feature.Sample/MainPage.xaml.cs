using Plugin.Maui.Feature;
using System.Collections.ObjectModel;

namespace Plugin.Maui.Feature.Sample;

public partial class MainPage : ContentPage
{
	readonly IFeature feature;
	readonly ObservableCollection<string> eventLog = new();
	readonly List<string> items = new()
	{
		"Item 1 - First Choice",
		"Item 2 - Second Choice",
		"Item 3 - Third Choice",
		"Item 4 - Fourth Choice",
		"Item 5 - Fifth Choice"
	};

	public MainPage(IFeature feature)
	{
		InitializeComponent();
		
		this.feature = feature;

		// Subscribe to events
		feature.KeyDown += OnKeyDown;
		feature.KeyUp += OnKeyUp;
		feature.FocusNavigationRequested += OnFocusNavigationRequested;

		// Update status
		UpdateStatus();

		// Setup items collection
		ItemsCollectionView.ItemsSource = items;
	}

	void UpdateStatus()
	{
		StatusLabel.Text = feature.IsListening ? "Listening for D-Pad events" : "Not Listening";
		SupportedLabel.Text = $"D-Pad Supported: {feature.IsSupported}";
		
		StartButton.IsEnabled = !feature.IsListening;
		StopButton.IsEnabled = feature.IsListening;
		
		EnableFocusButton.IsEnabled = !feature.IsFocusNavigationEnabled;
		DisableFocusButton.IsEnabled = feature.IsFocusNavigationEnabled;
	}

	void OnStartClicked(object? sender, EventArgs e)
	{
		feature.StartListening();
		UpdateStatus();
		AddLogEntry("Started listening for D-Pad events");
	}

	void OnStopClicked(object? sender, EventArgs e)
	{
		feature.StopListening();
		UpdateStatus();
		AddLogEntry("Stopped listening for D-Pad events");
	}

	void OnEnableFocusClicked(object? sender, EventArgs e)
	{
		feature.EnableFocusNavigation();
		UpdateStatus();
		AddLogEntry("Enabled automatic focus navigation");
	}

	void OnDisableFocusClicked(object? sender, EventArgs e)
	{
		feature.DisableFocusNavigation();
		UpdateStatus();
		AddLogEntry("Disabled automatic focus navigation");
	}

	void OnClearLogClicked(object? sender, EventArgs e)
	{
		eventLog.Clear();
		EventLogLabel.Text = "Event log cleared...";
	}

	void OnKeyDown(object? sender, DPadKeyEventArgs e)
	{
		MainThread.BeginInvokeOnMainThread(() =>
		{
			AddLogEntry($"KEY DOWN: {e.Key} at {e.Timestamp:HH:mm:ss.fff}");

			// Handle special keys
			if (e.Key == DPadKey.Back || e.Key == DPadKey.Menu)
			{
				BackMenuLabel.Text = $"{e.Key} button pressed at {DateTime.Now:HH:mm:ss}";
				BackMenuLabel.TextColor = Colors.Red;
			}
		});
	}

	void OnKeyUp(object? sender, DPadKeyEventArgs e)
	{
		MainThread.BeginInvokeOnMainThread(() =>
		{
			AddLogEntry($"KEY UP: {e.Key} at {e.Timestamp:HH:mm:ss.fff}");
		});
	}

	void OnFocusNavigationRequested(object? sender, FocusNavigationEventArgs e)
	{
		MainThread.BeginInvokeOnMainThread(() =>
		{
			AddLogEntry($"FOCUS NAV: {e.Direction} direction");
		});
	}

	void OnItemSelected(object? sender, SelectionChangedEventArgs e)
	{
		if (e.CurrentSelection.Count > 0)
		{
			var selectedItem = e.CurrentSelection[0] as string;
			SelectedItemLabel.Text = $"Selected: {selectedItem}";
			SelectedItemLabel.TextColor = Colors.Green;
			AddLogEntry($"Item selected: {selectedItem}");
		}
	}

	void AddLogEntry(string message)
	{
		var timestamp = DateTime.Now.ToString("HH:mm:ss.fff");
		eventLog.Add($"[{timestamp}] {message}");
		
		// Keep only last 20 entries
		while (eventLog.Count > 20)
		{
			eventLog.RemoveAt(0);
		}

		// Update label
		EventLogLabel.Text = string.Join("\n", eventLog);
	}
}
