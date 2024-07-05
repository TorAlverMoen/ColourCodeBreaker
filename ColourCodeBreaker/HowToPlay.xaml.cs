using System.Windows;

namespace ColourCodeBreaker
{
    /// <summary>
    /// Interaction logic for HowToPlay.xaml
    /// </summary>
    public partial class HowToPlay : Window
    {
        public HowToPlay()
        {
            InitializeComponent();
        }

        private void btnCloseHowToPlay_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
