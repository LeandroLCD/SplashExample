namespace SplashExample.UI.StarApp
{
    public class PermissionService
    {
        public async Task<PermissionStatus> RequestCameraPermissionAsync(Grid myGrid)
        {
            var status = await Permissions.CheckStatusAsync<Permissions.Camera>();

            switch (status)
            {
                case PermissionStatus.Granted:
                    return status;

                case PermissionStatus.Denied when DeviceInfo.Platform == DevicePlatform.iOS:
                    // Prompt the user to turn on in settings
                    // On iOS once a permission has been denied it may not be requested again from the application
                    return status;

                case PermissionStatus.Denied:
                    if (Permissions.ShouldShowRationale<Permissions.Camera>())
                    {
                        // Determina si se debe mostrar una interfaz de usuario educativa que explique al usuario cómo
                        // el permiso se utilizará en la aplicación.
                    }
                    else
                    {
                        LoadPromptCamera(myGrid, async () => {

                            status = await Permissions.RequestAsync<Permissions.Camera>();
                        });
                    }
                    return status;                   

                default:
                    LoadPromptCamera(myGrid, async () => {

                        status = await Permissions.RequestAsync<Permissions.Camera>();
                    });            
                    return status;
            }
        }

        private void LoadPromptCamera(Grid myGrid, Action action)
        {
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
                        Text = "Necesitamos el permiso de la camara para poder capturar imagenes en nuestra app.", 
                        VerticalOptions = LayoutOptions.FillAndExpand},
                    new Button(){Text = "Aceptar", 
                        
                        Command = new Command(() =>
                        {
                            var animation = new Animation(d =>
                            {
                            frame.TranslationX = 500;
                            frame.TranslationY = 500;
                            }, 1, 0);

                            animation.Commit(frame, "SlideOut", length: 500, easing: Easing.SinInOut, finished: (d, b) =>
                            {
                                frame.IsVisible = false;
                                action();
                                myGrid.BackgroundColor = white;
                                
                            });

                        })},
                }
            };

            myGrid.Add(frame);

        }
    }

}