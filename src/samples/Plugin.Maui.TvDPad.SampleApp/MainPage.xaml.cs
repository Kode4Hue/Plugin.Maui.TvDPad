using Plugin.Maui.TvDPad;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace Plugin.Maui.TvDPad.SampleApp;

public partial class MainPage : ContentPage
{
    readonly IFeature _tvDPad = Feature.Default;

    static readonly Color RastaBackground = Color.FromArgb("#0B0F0B");
    static readonly Color RastaSurface = Color.FromArgb("#121A12");
    static readonly Color RastaBorder = Color.FromArgb("#2A3A2A");

    static readonly Color RastaText = Color.FromArgb("#F4F3E6");
    static readonly Color RastaMuted = Color.FromArgb("#B7B3A2");

    static readonly Color RastaGreen = Color.FromArgb("#1DB954");
    static readonly Color RastaYellow = Color.FromArgb("#F2C94C");
    static readonly Color RastaRed = Color.FromArgb("#EB5757");

    // Selection vs focus colors must be distinct.
    static readonly Color RastaSelected = Color.FromArgb("#F2C94C");
    static readonly Color RastaSelectedBorder = Color.FromArgb("#EB5757");

    readonly Label _status;
    readonly Label _last;
    readonly CollectionView _log;
    readonly ObservableCollection<string> _items = new();

    readonly Button?[,] _tiles = new Button?[3, 3];

    Button? _selectedTile;

    // Turn off focus/key spam by default. Enable when diagnosing.
    const bool EnableEventLog = false;

    // Lightweight timing: key-down -> focus changed.
    long _lastNavKeyStartTicks;
    DPadKey? _lastNavDirection;

    // Throttle focus logging/UI churn.
    long _lastFocusLogTicks;
    const int FocusLogMinIntervalMs = 150;

    public MainPage()
    {
        // Intentionally no InitializeComponent(); this page is fully programmatic to avoid
        // XAML/code-behind mismatches on TV builds.

        // No page Title: TV UI runs without a top navigation bar.

        _status = new Label { FontSize = 12, TextColor = RastaMuted, InputTransparent = true };
        _last = new Label { FontSize = 18, TextColor = RastaText, FontAttributes = FontAttributes.Bold, InputTransparent = true };
        _log = new CollectionView
        {
            ItemsSource = _items,
            ItemsLayout = new LinearItemsLayout(ItemsLayoutOrientation.Vertical) { ItemSpacing = 6 },
            ItemTemplate = new DataTemplate(() =>
            {
                var l = new Label { FontSize = 12, TextColor = RastaMuted };
                l.SetBinding(Label.TextProperty, ".");
                return l;
            })
        };

        var grid = BuildFocusGrid();

        var logFrame = new Frame
        {
            Margin = new Thickness(0, 16, 0, 0),
            Padding = new Thickness(12),
            BorderColor = RastaBorder,
            BackgroundColor = RastaSurface,
            Content = _log,
            IsVisible = EnableEventLog
        };

        var host = new Grid
        {
            RowDefinitions = new RowDefinitionCollection
            {
                new RowDefinition(GridLength.Auto),
                new RowDefinition(GridLength.Star)
            }
        };

        host.Children.Add(grid);
        host.Children.Add(logFrame);
        Microsoft.Maui.Controls.Grid.SetRow(grid, 0);
        Microsoft.Maui.Controls.Grid.SetRow(logFrame, 1);

        var header = new Label
        {
            Text = "TV D-Pad / Remote test",
            FontSize = 22,
            FontAttributes = FontAttributes.Bold,
            TextColor = RastaText,
            InputTransparent = true
        };

        var instructions = new Label
        {
            Text = "1) Use the D-Pad to move focus around the 3×3 grid.\n" +
                   "2) Press OK / Enter to click a tile.\n" +
                   "3) Press Back to confirm Back handling.\n" +
                   "The last action will appear below.",
            FontSize = 14,
            TextColor = RastaMuted,
            InputTransparent = true
        };

        var root = new Grid
        {
            RowDefinitions = new RowDefinitionCollection
            {
                new RowDefinition(GridLength.Auto),
                new RowDefinition(GridLength.Auto),
                new RowDefinition(GridLength.Auto),
                new RowDefinition(GridLength.Auto),
                new RowDefinition(GridLength.Star)
            },
            Padding = new Thickness(24)
        };

        root.Children.Add(header);
        root.Children.Add(instructions);
        root.Children.Add(_status);
        root.Children.Add(_last);
        root.Children.Add(host);

        Microsoft.Maui.Controls.Grid.SetRow(header, 0);
        Microsoft.Maui.Controls.Grid.SetRow(instructions, 1);
        Microsoft.Maui.Controls.Grid.SetRow(_status, 2);
        Microsoft.Maui.Controls.Grid.SetRow(_last, 3);
        Microsoft.Maui.Controls.Grid.SetRow(host, 4);

        Content = root;

        BackgroundColor = RastaBackground;

        Appearing += OnAppearing;
        Disappearing += OnDisappearing;
    }

    void OnAppearing(object? sender, EventArgs e)
    {
        _status.Text = $"Plugin: Supported={_tvDPad.IsSupported} Listening={_tvDPad.IsListening} FocusNav={_tvDPad.IsFocusNavigationEnabled}";

        _tvDPad.KeyDown += OnKeyDown;
        _tvDPad.KeyUp += OnKeyUp;
        _tvDPad.FocusNavigationRequested += OnFocusNav;

        if (_tvDPad.IsSupported && !_tvDPad.IsListening)
            _tvDPad.StartListening();

        if (_tvDPad.IsSupported && !_tvDPad.IsFocusNavigationEnabled)
            _tvDPad.EnableFocusNavigation();

        _status.Text = $"Plugin: Supported={_tvDPad.IsSupported} Listening={_tvDPad.IsListening} FocusNav={_tvDPad.IsFocusNavigationEnabled}";

        FocusDefaultTile();
    }

    void OnDisappearing(object? sender, EventArgs e)
    {
        _tvDPad.KeyDown -= OnKeyDown;
        _tvDPad.KeyUp -= OnKeyUp;
        _tvDPad.FocusNavigationRequested -= OnFocusNav;
    }

    void OnKeyDown(object? sender, DPadKeyEventArgs e)
    {
        if (e.Key is DPadKey.Left or DPadKey.Right or DPadKey.Up or DPadKey.Down)
        {
            _lastNavDirection = e.Key;
            _lastNavKeyStartTicks = Stopwatch.GetTimestamp();
        }

        if (EnableEventLog)
            Log($"Pressed: {ToHumanButtonName(e.Key)}");
        else
            _last.Text = $"Last: Pressed: {ToHumanButtonName(e.Key)}";
    }

    void OnKeyUp(object? sender, DPadKeyEventArgs e)
    {
        if (EnableEventLog)
            Log($"Released: {ToHumanButtonName(e.Key)}");
    }

    void OnFocusNav(object? sender, FocusNavigationEventArgs e)
    {
        // Make navigation deterministic and fast by explicitly moving focus in the grid.
        if (TryMoveFocusInGrid(e.Direction))
        {
            e.Handled = true;
            return;
        }

        if (EnableEventLog)
            Log($"Navigate: {e.Direction}");
    }

    bool TryMoveFocusInGrid(DPadKey direction)
    {
        if (direction is not (DPadKey.Left or DPadKey.Right or DPadKey.Up or DPadKey.Down))
            return false;

        var focused = FindFocusedTile();
        if (focused == null)
            return false;

        var (r, c) = focused.Value;
        var (nr, nc) = direction switch
        {
            DPadKey.Left => (r, c - 1),
            DPadKey.Right => (r, c + 1),
            DPadKey.Up => (r - 1, c),
            DPadKey.Down => (r + 1, c),
            _ => (r, c)
        };

        if (nr < 0 || nr > 2 || nc < 0 || nc > 2)
            return true; // handled, but no-op at edges

        var next = _tiles[nr, nc];
        if (next == null)
            return false;

        return next.Focus();
    }

    (int r, int c)? FindFocusedTile()
    {
        for (var r = 0; r < 3; r++)
        {
            for (var c = 0; c < 3; c++)
            {
                if (_tiles[r, c]?.IsFocused == true)
                    return (r, c);
            }
        }

        return null;
    }

    static string ToHumanButtonName(DPadKey key) => key switch
    {
        DPadKey.Left => "Left",
        DPadKey.Right => "Right",
        DPadKey.Up => "Up",
        DPadKey.Down => "Down",
        DPadKey.Back => "Back",
        DPadKey.Center => "OK / Enter (Center)",
        DPadKey.Enter => "OK / Enter (Center)",
        _ => key.ToString()
    };

    void Log(string message)
    {
        var line = $"{DateTime.Now:HH:mm:ss.fff} {message}";
        _items.Insert(0, line);
        _last.Text = $"Last: {message}";

        if (_items.Count > 100)
            _items.RemoveAt(_items.Count - 1);
    }

    Grid BuildFocusGrid()
    {
        var g = new Grid
        {
            RowDefinitions = new RowDefinitionCollection
            {
                new RowDefinition(GridLength.Auto),
                new RowDefinition(GridLength.Auto),
                new RowDefinition(GridLength.Auto)
            },
            ColumnDefinitions = new ColumnDefinitionCollection
            {
                new ColumnDefinition(GridLength.Auto),
                new ColumnDefinition(GridLength.Auto),
                new ColumnDefinition(GridLength.Auto)
            },
            ColumnSpacing = 12,
            RowSpacing = 12,
            Margin = new Thickness(0, 16, 0, 0)
        };

        for (var r = 0; r < 3; r++)
        {
            for (var c = 0; c < 3; c++)
            {
                var index = r * 3 + c;
                var isCenter = index == 4;

                var b = new Button
                {
                    Text = $"Tile {index + 1}",
                    WidthRequest = 180,
                    HeightRequest = 80,
                    BackgroundColor = RastaSurface,
                    TextColor = RastaText,
                    CornerRadius = 12,
                    BorderColor = RastaBorder,
                    BorderWidth = 1
                };

                _tiles[r, c] = b;

                b.Focused += (_, __) =>
                {
                    ApplyTileVisualState(b);

                    if (_lastNavDirection is DPadKey.Left or DPadKey.Right or DPadKey.Up or DPadKey.Down)
                    {
                        var elapsed = Stopwatch.GetElapsedTime(_lastNavKeyStartTicks);
                        _status.Text = $"Focus move ({_lastNavDirection}) took {elapsed.TotalMilliseconds:0.0}ms";
                        _lastNavDirection = null;
                    }

                    if (EnableEventLog)
                    {
                        var now = Stopwatch.GetTimestamp();
                        var minTicks = (long)(FocusLogMinIntervalMs / 1000.0 * Stopwatch.Frequency);
                        if (now - _lastFocusLogTicks >= minTicks)
                        {
                            _lastFocusLogTicks = now;
                            Log(isCenter ? "Focus: OK / Center" : $"Focus: Tile {index + 1}");
                        }
                    }
                };

                b.Unfocused += (_, __) => ApplyTileVisualState(b);

                b.Pressed += (_, __) =>
                {
                    if (!b.IsFocused && !ReferenceEquals(b, _selectedTile))
                        b.BackgroundColor = Color.FromArgb("#1A231A");
                };
                b.Released += (_, __) =>
                {
                    if (!b.IsFocused)
                        ApplyTileVisualState(b);
                };

                b.Clicked += (_, __) =>
                {
                    SetSelectedTile(b);

                    var label = $"Tile {index + 1}";
                    if (EnableEventLog)
                        Log($"Selected: {label}");
                    else
                        _last.Text = $"Last: Selected: {label}";
                };

                g.Add(b, c, r);

                if (isCenter)
                    b.AutomationId = "DefaultFocus";
            }
        }

        return g;
    }

    void FocusDefaultTile()
    {
        if (Content is not Layout layout)
            return;

        var target = FindByAutomationId(layout, "DefaultFocus") as VisualElement;
        target?.Focus();
    }

    static IView? FindByAutomationId(IView view, string automationId)
    {
        if (view is VisualElement ve && ve.AutomationId == automationId)
            return view;

        if (view is Microsoft.Maui.Controls.Layout layout)
        {
            foreach (var child in layout.Children)
            {
                var found = FindByAutomationId(child, automationId);
                if (found != null)
                    return found;
            }
        }

        return null;
    }

    void SetSelectedTile(Button? tile)
    {
        if (ReferenceEquals(_selectedTile, tile))
            return;

        var previous = _selectedTile;
        _selectedTile = tile;

        if (previous != null)
            ApplyTileVisualState(previous);

        if (tile != null)
            ApplyTileVisualState(tile);
    }

    void ApplyTileVisualState(Button tile)
    {
        var isFocused = tile.IsFocused;
        var isSelected = ReferenceEquals(tile, _selectedTile);

        if (isFocused)
        {
            // Focus state wins while navigating.
            tile.BackgroundColor = RastaGreen;
            tile.TextColor = Colors.Black;
            tile.BorderColor = RastaYellow;
            tile.BorderWidth = 2;
            return;
        }

        if (isSelected)
        {
            tile.BackgroundColor = RastaSelected;
            tile.TextColor = Colors.White;
            tile.BorderColor = RastaSelectedBorder;
            tile.BorderWidth = 2;
            return;
        }

        tile.BackgroundColor = RastaSurface;
        tile.TextColor = RastaText;
        tile.BorderColor = RastaBorder;
        tile.BorderWidth = 1;
    }
}
