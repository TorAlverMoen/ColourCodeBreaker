using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace ColourCodeBreaker
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Loaded += Window_Loaded;
        }

        int ColourButton = 0; // Button id: Which button was clicked: 0 = none, 1 = Read, 2 = Green, 3 = Yellow, 4 = Orange, 5 = Blue, 6 = White
        int Position = 0;     // Position Id: Which of the four positions to put the currently selected colour
        bool[] Pos = { false, false, false, false };
        Color[] Colours = new Color[]
        {
            Colors.Red, Colors.Green, Colors.Yellow, Colors.Orange, Colors.Blue, Colors.White
        };

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            label_Info.Content = "Current version is v" + Assembly.GetExecutingAssembly().GetName().Version;
            NewGame();
        }

        private void NewGame()
        {

        }

        private void ColourWasChosen()
        {
            btnConfirm.IsEnabled = false;
            label_Info.Content = "Please choose a position for the selected colour.";
            // Now wait the for player to choose location of the colour
        }

        private void PlaceColour()
        {
            btnConfirm.IsEnabled = true;
            Button tempButton = new Button();
            Pos[Position - 1] = true;

            switch (Position)
            {
                case 1: tempButton = btnPG1; break;
                case 2: tempButton = btnPG2; break;
                case 3: tempButton = btnPG3; break;
                case 4: tempButton = btnPG4; break;
            }

            if (ColourButton != 0)
            {
                tempButton.Background = new SolidColorBrush(Colours[ColourButton - 1]);
                label_Info.Content = "The colour was placed in position " + Position.ToString();
                ColourButton = 0;
            }
        }

        private void btnConfirm_Click(object sender, RoutedEventArgs e)
        {
            bool positionTemp = Array.Exists(Pos, element => element == false);
            label_Info.Content = positionTemp;
            if (positionTemp)
            {
                label_Info.Content = "You must choose a colour all four positions to proceed";
            }
        }

        private void btnPG1_Click(object sender, RoutedEventArgs e)
        {
            Position = 1;
            PlaceColour();
        }

        private void btnPG2_Click(object sender, RoutedEventArgs e)
        {
            Position = 2;
            PlaceColour();
        }

        private void btnPG3_Click(object sender, RoutedEventArgs e)
        {
            Position = 3;
            PlaceColour();
        }

        private void btnPG4_Click(object sender, RoutedEventArgs e)
        {
            Position = 4;
            PlaceColour();
        }

        private void btnRed_Click(object sender, RoutedEventArgs e)
        {
            ColourButton = 1;
            ColourWasChosen();
        }

        private void btnGreen_Click(object sender, RoutedEventArgs e)
        {
            ColourButton = 2;
            ColourWasChosen();
        }

        private void btnYellow_Click(object sender, RoutedEventArgs e)
        {
            ColourButton = 3;
            ColourWasChosen();
        }

        private void btnOrange_Click(object sender, RoutedEventArgs e)
        {
            ColourButton = 4;
            ColourWasChosen();
        }

        private void btnBlue_Click(object sender, RoutedEventArgs e)
        {
            ColourButton = 5;
            ColourWasChosen();
        }

        private void btnWhite_Click(object sender, RoutedEventArgs e)
        {
            ColourButton = 6;
            ColourWasChosen();
        }

        private void btnDifficulty_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnNewGame_Click(object sender, RoutedEventArgs e)
        {
            NewGame();
        }

        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}