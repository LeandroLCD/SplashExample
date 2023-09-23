using SplashExample.UI.StarApp;

namespace SplashExample;

public partial class App : Application
{
    public App(PermissionService viewModel)
    {
        InitializeComponent();

        MainPage = new StarScreen(viewModel);
    }
}
