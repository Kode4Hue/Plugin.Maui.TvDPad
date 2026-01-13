using Microsoft.Maui.Controls;
using Plugin.Maui.TvDPad;

namespace TestRunner;

public partial class MainPage : ContentPage
{
    public MainPage()
    {
        InitializeComponent();

        // Wire up DPad events from the library to update UI for tests
        DPad.Current.Up += () => StatusLabel.Text = "Up";
        DPad.Current.Down += () => StatusLabel.Text = "Down";
        DPad.Current.Left += () => StatusLabel.Text = "Left";
        DPad.Current.Right += () => StatusLabel.Text = "Right";
        DPad.Current.Center += () => StatusLabel.Text = "Center";

        UpButton.Clicked += (s, e) => DPad.Current.SimulateUp();
        DownButton.Clicked += (s, e) => DPad.Current.SimulateDown();
        LeftButton.Clicked += (s, e) => DPad.Current.SimulateLeft();
        RightButton.Clicked += (s, e) => DPad.Current.SimulateRight();
        CenterButton.Clicked += (s, e) => DPad.Current.SimulateCenter();
    }
}
