using Microsoft.Maui.Controls;

namespace TestRunner;

public partial class App : Application
{
    public App()
    {
        InitializeComponent();

        MainPage = new MainPage();
    }
}
