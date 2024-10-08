﻿using System.Windows;

namespace ColourCodeBreaker
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            SplashScreen splash = new SplashScreen("SplashScreen.png");
            splash.Show(autoClose: false);
            Thread.Sleep(3000);
            splash.Close(TimeSpan.FromMilliseconds(500));
        }
    }

}
