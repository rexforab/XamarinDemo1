using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Plugin.Geolocator;
using Plugin.Media;
using Xamarin.Forms;

namespace WinSetupDemo
{
    public class App : Application
    {
        public App()
        {
            // Page controls
            Label lblLocation = new Label() { FontSize = 20 };
            Button btnLocation = new Button() { Text = "Get Location" };
            Button btnTakePhoto = new Button() { Text = "Take Photo" };
            Button btnGetPhoto = new Button() { Text = "Pick Photo" };

            // Page structure (root page of application)
            var content = new ContentPage
            {
                Title = "Xamarin Demo",
                Content = new StackLayout
                {
                    VerticalOptions = LayoutOptions.Center,
                    Children = {
                        new Label {
                            HorizontalTextAlignment = TextAlignment.Center,
                            Text = "Xamarin Demo"
                        },
                        btnLocation,
                        lblLocation,
                        btnTakePhoto,
                        btnGetPhoto
                    }
                }
            };

            #region Button Events

            // Camera
            // --------------------------------

            btnTakePhoto.Clicked += async (sender, args) =>
            {
                await CrossMedia.Current.Initialize();

                if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
                {
                    content.DisplayAlert("No Camera", ":( No camera available.", "OK");
                    return;
                }

                var file = await CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions
                {
                    Directory = "Test",
                    Name = "test.jpg",
                    SaveToAlbum = true
                });

                if (file == null)
                    return;

                await content.DisplayAlert("File Location", file.Path, "OK");

                var image = new Image()
                {
                    Source = ImageSource.FromStream(() =>
                    {
                        var stream = file.GetStream();
                        file.Dispose();
                        return stream;
                    })
                };

                //or:
                //image.Source = ImageSource.FromFile(file.Path);
                //image.Dispose();
            };

            btnGetPhoto.Clicked += async (sender, args) => 
            { 
                var file = await CrossMedia.Current.PickPhotoAsync(new Plugin.Media.Abstractions.PickMediaOptions
                {
                    
                });
            };

            //---------------------------------

            // Location
            // ---------------------

            btnLocation.Clicked += async (sender, args) =>
            {
                var locator = CrossGeolocator.Current;

                locator.DesiredAccuracy = 100; //100 is new default

                var position = await locator.GetPositionAsync(timeoutMilliseconds: 10000);
                lblLocation.Text = $"Lat: {position.Latitude} Long: {position.Longitude}";
            };
            // -------------------------------

            #endregion

            MainPage = new NavigationPage(content);
        }

        #region App Events
        

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }

        #endregion
    }
}
