using Microsoft.Maui;
using Microsoft.Maui.Hosting;

namespace TestRunner;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts => { });

        return builder.Build();
    }
}
