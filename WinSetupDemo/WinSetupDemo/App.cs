using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Plugin.Geolocator;
using Plugin.Media;
using Xamarin.Forms;
using Newtonsoft.Json;
using PCLStorage;

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
            Button btnSaveData = new Button() { Text = "Save Data" };
            Button btnLoadData = new Button() { Text = "Load Data" };
            Label lblLoadedData = new Label() { FontSize = 20 };

            // Page structure (root page of application)
            var content = new ContentPage
            {
                Title = "Xamarin Demo",
                Content = new StackLayout
                {
                    VerticalOptions = LayoutOptions.Start,
                    Children = {
                        new Label {
                            HorizontalTextAlignment = TextAlignment.Center,
                            Text = "Xamarin Demo"
                        },
                        btnLocation,
                        lblLocation,
                        btnTakePhoto,
                        btnGetPhoto,
                        btnSaveData,
                        btnLoadData,
                        lblLoadedData
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

            // Data
            // -------------------------------
            btnSaveData.Clicked += async (sender, args) =>
            {
                var test = new TestObj()
                {
                    Name = "John Doe",
                    Animal = "Cat",
                    Time = DateTime.Now,
                    Car = "Delorian"
                };

                string json = JsonConvert.SerializeObject(test);

                IFolder rootFolder = FileSystem.Current.LocalStorage;
                IFolder folder = await rootFolder.CreateFolderAsync("MySubFolder",
                    CreationCollisionOption.OpenIfExists);
                IFile file = await folder.CreateFileAsync("test.json",
                    CreationCollisionOption.ReplaceExisting);
                await file.WriteAllTextAsync(json);
            };


            btnLoadData.Clicked += async (sender, args) =>
            {
                IFolder rootFolder = FileSystem.Current.LocalStorage;
                IFolder folder = await rootFolder.CreateFolderAsync("MySubFolder",
                    CreationCollisionOption.OpenIfExists);
                IFile file = await folder.GetFileAsync("test.json");
                var jsonText = await file.ReadAllTextAsync();

                var test = JsonConvert.DeserializeObject<TestObj>(jsonText);

                lblLoadedData.Text = $"Name: { test.Name}, Time: {test.Time.TimeOfDay.ToString()}";
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

    public class TestObj
    {
        public DateTime Time { get; set; }
        public string Name { get; set; }
        public string Car { get; set; }
        public string Animal { get; set; }
    }
}
