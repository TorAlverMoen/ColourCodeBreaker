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
        int Difficulty = 0;   // 0 = easy (20 turns), 1 = medium (10 turns), 2 = hard (5 turns)
        bool IsNewGameStarted = false;
        bool AllowDuplicateColours = false;
        int[] CorrectCombinationIndexes = { 0, 1, 2, 3, 4, 5 };
        bool[] Pos = { false, false, false, false };
        Color[] Colours = new Color[]
        {
            Colors.Red, Colors.Green, Colors.Yellow, Colors.Orange, Colors.Blue, Colors.White
        };
        int[] solution = { 0, 0, 0, 0 };
        int[] playerGuess = { 0, 0, 0, 0 };

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            label_Title.Content = "Colour code breaker";
            label_Version.Content = "v" + Assembly.GetExecutingAssembly().GetName().Version;
            NewGame();
        }

        private void ResetColourPositions()
        {
            btnPG1.Background = new SolidColorBrush(Colors.DimGray);
            btnPG2.Background = new SolidColorBrush(Colors.DimGray);
            btnPG3.Background = new SolidColorBrush(Colors.DimGray);
            btnPG4.Background = new SolidColorBrush(Colors.DimGray);

            for (int i = 0; i < Pos.Length; i++)
            {
                Pos[i] = false;
            }
        }

        private void ResetPlayerGuessArray()
        {
            for (int i = 0; i < playerGuess.Length; i++)
            {
                playerGuess[i] = 0;
            }
        }

        private void GenerateCode()
        {
            Random random = new Random();

            if (AllowDuplicateColours)
            {
                for (int i = 0; i < solution.Length; i++)
                {
                    solution[i] = random.Next(1, 7);
                }
            }
            else
            {
                for (int i = 0; i < CorrectCombinationIndexes.Length; i++)
                {
                    int randomIndex = random.Next(CorrectCombinationIndexes.Length);
                    int tempIndex = CorrectCombinationIndexes[randomIndex];
                    CorrectCombinationIndexes[randomIndex] = CorrectCombinationIndexes[i];
                    CorrectCombinationIndexes[i] = tempIndex;
                }

                int tempStartIndex = random.Next(0, 2);
                for (int i = 0; i < solution.Length; ++i)
                {
                    solution[i] = CorrectCombinationIndexes[i + tempStartIndex];
                }
            }
            // DEBUG START
            label_Info.Content = solution[0].ToString() + ", " + solution[1].ToString() + ", " +
                solution[2].ToString() + ", " + solution[3].ToString();
            // DEBUG END
        }

        private void ChangeDifficulty()
        {
            Difficulty++;
            if (Difficulty > 2)
            {
                Difficulty = 0;
            }
            DisplayDifficulty();
        }

        private void DisplayDifficulty()
        {
            string tempDifficulty = "";
            switch (Difficulty)
            {
                case 0: tempDifficulty = "Easy"; break;
                case 1: tempDifficulty = "Medium"; break;
                case 2: tempDifficulty = "Hard"; break;
            }
            if (!IsNewGameStarted)
            {
                label_Info.Content = "Sorry but the difficulty cannot be changed mid-game!";
            }
            else
            {
                labelDisplayDiff.Content = tempDifficulty;
            }
        }


        private void NewGame()
        {
            ResetColourPositions();
            ResetPlayerGuessArray();
            GenerateCode();
            IsNewGameStarted = true;
            DisplayDifficulty();
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
                label_Info.Content = "You must place a colour in all four positions to proceed";
            }
            else
            {
                ResetColourPositions();
                // Check solution
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
            ChangeDifficulty();
        }

        private void btnNewGame_Click(object sender, RoutedEventArgs e)
        {
            NewGame();
        }

        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void checkBox_DuplicateColours_Checked(object sender, RoutedEventArgs e)
        {
            AllowDuplicateColours = true;
        }

        private void checkBox_DuplicateColours_Unchecked(object sender, RoutedEventArgs e)
        {
            AllowDuplicateColours = false;
        }
    }
}