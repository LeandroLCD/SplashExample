using SkiaSharp.Extended.UI.Controls;

namespace SplashExample.UI.StarApp
{
    public class StarScreen : ContentPage
    {
        private Grid myGrid;

        [Obsolete]
        public StarScreen(PermissionService permissionService)
        {

            Content = myGrid = new Grid
            {
                Children =
                {
                    LoadAnimationAsync(permissionService).Result
                }
            };
            
        }

        [Obsolete]
        private async void RequestLogin() //inyectar caso de uso o servicio que valida las credenciales del usuario
        {
            //si es valido 0> navego al home App.Current.MainPage = new ShellPage();
            // de lo contrario navego al login //App.Current.MainPage = new LoginPage();
            try
            {
                if (await RequiesVersionApp(myGrid))
                {
                    App.Current.MainPage = new MainPage();
                }


            }
            catch (Exception)
            {

                throw;
            }
            

            
        }

        [Obsolete]
        private Task<bool> RequiesVersionApp(Grid myGrid) // este metodo se utiliza para validar la version minima de la app.
        {
            bool isValid = false;
            var white = Color.FromRgb(255, 255, 255);

            var gray = Color.FromRgb(180, 181, 189);

            myGrid.BackgroundColor = gray;

            Frame frame = new Frame()
            {
                WidthRequest = 250,
                HeightRequest = 200,
                CornerRadius = 15,
                BackgroundColor = white,
                BorderColor = Color.FromRgb(0, 0, 0),

            };


            frame.Content = new StackLayout()
            {
                Children =
                {
                    new Label(){
                        Text = "Tenemos una nueva Actualización, por favor para continuar es necesario actualizar.",
                        VerticalOptions = LayoutOptions.FillAndExpand},
                    new Button(){Text = "Aceptar",

                        Command = new Command(async () =>
                        {
                            
                                frame.IsVisible = false;
                                 try
                                {
                                //https://play.google.com/store/apps/details?id=com.leandrolcd.doganalyzer
                                    //Uri uri = new Uri("https://www.youtube.com/@BlipBlipCode");
                                    Uri uri = new Uri("https://play.google.com/store/apps/details?id=com.leandrolcd.doganalyzer");
                                    await Browser.Default.OpenAsync(uri, BrowserLaunchMode.SystemPreferred);
                                    isValid = false;
                                }
                                catch (Exception ex)
                                {
                                    isValid = true;
                                    // An unexpected error occurred. No browser may be installed on the device.
                                }
                                myGrid.BackgroundColor = white;

                            

                        })},
                }
            };

            myGrid.Add(frame);
            //isValid = true;
            return Task.FromResult(isValid);
        }

        [Obsolete]
        private async Task<IView> LoadAnimationAsync(PermissionService permissionService)
        {

            var stream = await FileSystem.OpenAppPackageFileAsync("splash_animation.json");
            var animation = new SKLottieView()
            {
                RepeatMode = SKLottieRepeatMode.Restart,
                RepeatCount = -1,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center,
                WidthRequest = 200,
                HeightRequest = 200,
                Source = (SKLottieImageSource)SKLottieImageSource.FromStream(stream), 

            };
            animation.AnimationLoaded += async (s, e) => {

                await Task.Delay(1000);

                RequestLogin();

                await permissionService.RequestCameraPermissionAsync(myGrid);

                


            };

            return animation;
        }
    }
}