using Microsoft.Extensions.DependencyInjection;

namespace Plugin.Maui.TvDPad.SampleApp
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            // Keep startup simple for Android TV: no Shell routing.
            // If something goes wrong creating MainPage, show a visible fallback.
            try
            {
                MainPage = new NavigationPage(new MainPage());
            }
            catch (Exception ex)
            {
                MainPage = new ContentPage
                {
                    BackgroundColor = Colors.Black,
                    Content = new Label
                    {
                        Text = ex.ToString(),
                        TextColor = Colors.Red,
                        Margin = new Thickness(20)
                    }
                };
            }
        }
    }
}