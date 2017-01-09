using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;

namespace WinSetupDemo
{
    public class App : Application
    {
        public App()
        {
            Button btn = new Button();
            btn.Text = "Hit me";

            btn.Clicked += (sender, args) =>
            {
                btn.Text = "ARG YOU HIT ME";
            };

            // The root page of your application
            var content = new ContentPage
            {
                Title = "WinSetupDemo",
                Content = new StackLayout
                {
                    VerticalOptions = LayoutOptions.Center,
                    Children = {
                        new Label {
                            HorizontalTextAlignment = TextAlignment.Center,
                            Text = "Welcome to Xamarin Forms!"
                        },
                        btn
                    }
                }
            };

            MainPage = new NavigationPage(content);
        }

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
    }
}
