using System.Windows;
using System.Windows.Threading;

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
            splash.Show(false);

            DispatcherTimer timer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(2)
            };

            timer.Tick += (s, args) =>
            {
                timer.Stop();

                splash.Close(TimeSpan.FromMilliseconds(500));

                // Show the main window after the splash screen closes
                DispatcherTimer closeTimer = new DispatcherTimer
                {
                    Interval = TimeSpan.FromMilliseconds(500)
                };

                closeTimer.Tick += (s2, e2) =>
                {
                    closeTimer.Stop();

                    MainWindow mainWindow = new MainWindow();
                    mainWindow.Show();
                };

                closeTimer.Start();
            };

            timer.Start();
        }
    }

}
