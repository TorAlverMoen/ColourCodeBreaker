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
        int CurrentTurn = 20;
        bool AllowDuplicateColours = false;
        string appVersion = "v" + Assembly.GetExecutingAssembly().GetName().Version?.ToString() ?? "-----";
        int CorrectPlacement = 0;
        int CorrectColour = 0;
        bool bIsGameOngoing = false;
        bool bDidThePlayerWin = false;

        bool[] Pos = { false, false, false, false };
        int[] CorrectCombinationIndexes = { 0, 1, 2, 3, 4, 5 };
        Button[] buttons = new Button[6];
        Button[] pgbuttons = new Button[4];
        Color[] Colours = [ Colors.Red, Colors.Green, Colors.Yellow, Colors.Orange, Colors.Blue, Colors.White ];
        Color[] DimColours = [ Colors.DarkRed, Colors.DarkGreen, Color.FromRgb(139, 128, 0), Colors.DarkOrange, Colors.DarkBlue, Color.FromRgb(225, 217, 209) ];
        int[] solution = { 0, 0, 0, 0 };
        int[] playerGuess = { 0, 0, 0, 0 };
        Label[] feedbackLabels = new Label[4];

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            label_Title.Content = "Colour code breaker";
            label_Version.Content = appVersion;
            buttons = [btnRed, btnGreen, btnYellow, btnOrange, btnBlue, btnWhite];   // Init buttons array
            pgbuttons = [btnPG1, btnPG2, btnPG3, btnPG4];   // Init position buttons array
            feedbackLabels = [label_Feedback1, label_Feedback2, label_Feedback3, label_Feedback4]; // Init feedback array
            NewGame();
            SetTurns();
            DisplayTurns();
            DisplayDifficulty();
        }

        private void ResetColourPositions()
        {
            foreach (var button in pgbuttons)
            {
                button.Background = new SolidColorBrush(Colors.DimGray);
            }

            for (int i = 0; i < Pos.Length; i++)
            {
                Pos[i] = false;
            }
        }

        private void SetDimButtonColours()
        {
            if (ColourButton >= 1 && ColourButton <= buttons.Length)
            {
                buttons[ColourButton - 1].Background = new SolidColorBrush(DimColours[ColourButton - 1]);
            }
        }

        private void SetBrightButtonColours()
        {
            if (ColourButton >= 1 && ColourButton <= buttons.Length)
            {
                buttons[ColourButton - 1].Background = new SolidColorBrush(Colours[ColourButton - 1]);
            }
        }

        private void SetTurns()
        {
            switch (Difficulty)
            {
                case 0: CurrentTurn = 20; break;   // Easy
                case 1: CurrentTurn = 10; break;   // Medium
                case 2: CurrentTurn = 5; break;    // Hard
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

                int tempStartIndex = random.Next(3);
                for (int i = 0; i < solution.Length; ++i)
                {
                    solution[i] = CorrectCombinationIndexes[i + tempStartIndex];
                }
            }
            // DEBUG START
            //label_Info.Content = solution[0].ToString() + ", " + solution[1].ToString() + ", " + solution[2].ToString() + ", " + solution[3].ToString();
            // DEBUG END
        }

        private void NewGame()
        {
            ResetColourPositions();
            ResetPlayerGuessArray();
            GenerateCode();
            ResetFeedbackLabels();
            bIsGameOngoing = false;
            bDidThePlayerWin = false;
            btnDifficulty.IsEnabled = true;
            SetTurns();
            DisplayTurns();
        }

        private void ColourWasChosen()
        {
            btnConfirm.IsEnabled = false;
            label_Info.Content = "Please choose a position for the selected colour.";
            SetDimButtonColours();
            // Now wait the for player to choose location of the colour
        }

        private void PlaceColour()
        {
            btnConfirm.IsEnabled = true;
            SetBrightButtonColours();
            ResetFeedbackLabels();
            Pos[Position - 1] = true;

            if (!bIsGameOngoing)
            {
                bIsGameOngoing = true;
                btnDifficulty.IsEnabled = false;   // This must be set to true at 'New game', 'player win' or 'player lose'
            }

            if (ColourButton != 0 && Position >= 1 && Position <= pgbuttons.Length)
            {
                pgbuttons[Position - 1].Background = new SolidColorBrush(Colours[ColourButton - 1]);
                label_Info.Content = "The colour was placed in position " + Position.ToString();
                playerGuess[Position - 1] = ColourButton;
                ColourButton = 0;
            }
        }

        private void CheckCorrectColoursPlacedWrong()
        {
            CorrectColour = 0;
            bool[] matchedCorrectCombinations = new bool[pgbuttons.Length];
            bool[] matchedPlayerGuess = new bool[pgbuttons.Length];

            for (int i = 0; i < pgbuttons.Length; i++)
            {
                if (playerGuess[i] == solution[i])
                {
                    matchedCorrectCombinations[i] = true;
                    matchedPlayerGuess[i] = true;
                }
            }

            for (int i = 0; i < pgbuttons.Length; i++)   // Nest-o-rama
            {
                if (!matchedPlayerGuess[i])
                {
                    for (int j = 0; j < pgbuttons.Length; j++)
                    {
                        if (!matchedCorrectCombinations[j] && playerGuess[i] == solution[j])
                        {
                            CorrectColour++;
                            matchedCorrectCombinations[j] = true;
                            break;
                        }
                    }
                }
            }
        }

        private void CheckSolution()
        {
            // I asked ChatGPT for an alternative way to find the number of correct colours and it came up with this:
            ///    CorrectPlacement = playerGuess.Zip(solution, (guess, correct) => guess == correct).Count(match => match);
            // It uses LINQ which creates a performance overhead but it is only one line and could be considered easier to
            // read if you know and are comfortable with LINQ. The code below is doing the same thing but it is more verbose
            // but at the same time easier for the compiler to optimize!
            // Is performance in anyway critical here? No! But I still prefer to write the code as it is below!

            // Check correct colours in the correct place
            CorrectPlacement = 0;
            for (int i = 0; i < pgbuttons.Length; i++)
            {
                if (playerGuess[i] == solution[i])
                {
                    CorrectPlacement++;
                }
            }

            if (CorrectPlacement < pgbuttons.Length)
            {
                // TEST: Check correct colours in the wrong place
                CheckCorrectColoursPlacedWrong();
            }
            if (CorrectPlacement == pgbuttons.Length)
            {
                // The player wins
                bDidThePlayerWin = true;
                label_Info.Content = "You win! The correct code was found!";
            }
        }

        private void DisplayTurns()
        {
            if (CurrentTurn <= 0)
            {
                // Player loses
                label_Info.Content = "You lose! The correct colour code was not found in the selected number of turns!";
                MessageBox.Show("The correct colour code was not found in the selected number of turns!", "You lose!", MessageBoxButton.OK);
                NewGame();
            }
            else
            {
                labelTurnsDisplay.Content = CurrentTurn.ToString();
            }
        }

        private void DisplayDifficulty()
        {
            switch (Difficulty)
            {
                case 0: labelDisplayDiff.Content = "Easy"; break;
                case 1: labelDisplayDiff.Content = "Medium"; break;
                case 2: labelDisplayDiff.Content = "Hard"; break;
            }
        }

        private void UpdateDifficulty()
        {
            Difficulty++;
            if (Difficulty > 2)
            {
                Difficulty = 0;
            }
            DisplayDifficulty();
            SetTurns();
            DisplayTurns();
        }

        private void ResetFeedbackLabels()
        {
            for (int i = 0; i < feedbackLabels.Length; i++)
            {
                feedbackLabels[i].Background = new SolidColorBrush(Colors.Gray);
            }
        }

        private void DisplayFeedback()
        {
            // Black is correct colour in the correct position
            // White is correct colour in the wrong position

            // Display black
            for (int i = 0; i < CorrectPlacement; i++)
            {
                feedbackLabels[i].Background = new SolidColorBrush(Colors.Black);
            }

            // Display white
            if (CorrectPlacement < pgbuttons.Length)
            {
                int j = CorrectPlacement;
                for (int i = 0; i < CorrectColour; i++)
                {
                    feedbackLabels[j + i].Background = new SolidColorBrush(Colors.White);
                }
            }
        }

        private void btnConfirm_Click(object sender, RoutedEventArgs e)
        {
            bool positionTemp = Array.Exists(Pos, element => element == false);
            if (positionTemp)
            {
                label_Info.Content = "You must place a colour in all four positions to proceed";
            }
            else
            {
                CurrentTurn--;
                DisplayTurns();
                CheckSolution();
                DisplayFeedback();
                if (bDidThePlayerWin)
                {
                    MessageBox.Show("You did it! The correct code was found!", "You win!", MessageBoxButton.OK);
                    NewGame();
                }
            }
        }

        private void btnDifficulty_Click(object sender, RoutedEventArgs e)
        {
            UpdateDifficulty();
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

        private void btnAbout_Click(object sender, RoutedEventArgs e)
        {
            AboutBox aboutBox = new AboutBox();
            aboutBox.ShowDialog();
        }
    }
}