﻿using System.Reflection;
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

        int ColourButton = 0; // Button id: Which button was clicked: 0 = none, 1 = Red, 2 = Green, 3 = Yellow, 4 = Orange, 5 = Blue, 6 = White
        int Position = 0;     // Position Id: Which of the four positions to put the currently selected colour
        int Difficulty = 0;   // 0 = easy (20 turns), 1 = medium (10 turns), 2 = hard (5 turns)
        int CurrentTurn = 20;
        bool bAllowDuplicateColours = false;
        string appVersion = "v" + Assembly.GetExecutingAssembly().GetName().Version?.ToString() ?? "-----";
        int CorrectPlacement = 0;
        int CorrectColour = 0;
        bool bIsGameOngoing = false;
        bool bDidThePlayerWin = false;
        int HistoryIndex = 0;

        bool[] Pos = { false, false, false, false };
        int[] CorrectCombinationIndexes = { 0, 1, 2, 3, 4, 5 };
        Button[] buttons = new Button[6];
        Button[] pgbuttons = new Button[4];
        Color[] Colours = [ Colors.Red, Colors.Green, Colors.Yellow, Colors.Orange, Colors.Blue, Colors.White ];
        Color[] DimColours = [ Colors.DarkRed, Colors.DarkGreen, Color.FromRgb(139, 128, 0), Color.FromRgb(195, 80, 0), Colors.DarkBlue, Color.FromRgb(225, 217, 209) ];
        int[] solution = { 0, 0, 0, 0 };
        int[] playerGuess = { 0, 0, 0, 0 };
        Label[] feedbackLabels = new Label[4];
        int[,] HistoryMoves = new int[20, 6];   // Array to hold the moves history

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            label_Title.Content = "Colour code breaker";
            label_Version.Content = appVersion;
            label_Info.Content = "Welcome to Colour Code Breaker " + appVersion + " by Tor Alver Moen";
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
                button.Foreground = new SolidColorBrush(Colors.LightGray);
            }

            for (int i = 0; i < Pos.Length; i++)
            {
                Pos[i] = false;
            }
        }

        private void SetButtonColour(bool bColourIsDim)
        {
            if (ColourButton >= 1 && ColourButton <= buttons.Length)
            {
                if (!bColourIsDim)
                {
                    buttons[ColourButton - 1].Background = new SolidColorBrush(Colours[ColourButton - 1]);
                }
                else
                {
                    buttons[ColourButton - 1].Background = new SolidColorBrush(DimColours[ColourButton - 1]);
                }
            }
        }

        private void SetBrightAllButtonColours()
        {
            for (int i = 0; i < buttons.Length; i++)
            {
                buttons[i].Background = new SolidColorBrush(Colours[i]);
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

            if (bAllowDuplicateColours)
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
            label_Info.Content = "Welcome to Colour Code Breaker " + appVersion + " by Tor Alver Moen";
            bIsGameOngoing = false;
            bDidThePlayerWin = false;
            btnDifficulty.IsEnabled = true;
            checkBox_DuplicateColours.IsEnabled = true;
            SetTurns();
            DisplayTurns();
            HistoryIndex = 0;
            Array.Clear(HistoryMoves, 0, HistoryMoves.Length);
        }

        private void ColourWasChosen()
        {
            btnConfirm.IsEnabled = false;
            label_Info.Content = "Please choose a position for the selected colour.";
            SetBrightAllButtonColours();
            SetButtonColour(true);
            // Now wait the for player to choose location of the colour
        }

        private void PlaceColour()
        {
            btnConfirm.IsEnabled = true;
            SetButtonColour(false);
            ResetFeedbackLabels();
            Pos[Position - 1] = true;

            if (!bIsGameOngoing)
            {
                bIsGameOngoing = true;
                btnDifficulty.IsEnabled = false;
                checkBox_DuplicateColours.IsEnabled = false;
            }

            if (ColourButton != 0 && Position >= 1 && Position <= pgbuttons.Length)
            {
                pgbuttons[Position - 1].Background = new SolidColorBrush(Colours[ColourButton - 1]);

                if (ColourButton == 3 || ColourButton == 4 || ColourButton == 6)
                {
                    pgbuttons[Position - 1].Foreground = new SolidColorBrush(Colors.Black);
                }
                else
                {
                    pgbuttons[Position - 1].Foreground = new SolidColorBrush(Colors.LightGray);
                }

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
                // Check correct colours in the wrong place
                CheckCorrectColoursPlacedWrong();
            }
            if (CorrectPlacement == pgbuttons.Length)
            {
                // The player wins
                bDidThePlayerWin = true;
                label_Info.Content = "You win! The correct colour code was found!";
            }
        }

        private void StoreMovesInHistory()
        {
            // A move is stored with 6 numbers: positions 0-3 = colours, 4-5 = feedback
            // The colours use the following numbers: 0 = none, 1 = Red, 2 = Green, 3 = Yellow, 4 = Orange, 5 = Blue, 6 = White
            // The feedback are stored with the number of correct placements first (black) and the number of incorrectly placed colours second (white)

            for (int i = 0; i < pgbuttons.Length; i++)
            {
                HistoryMoves[HistoryIndex, i] = playerGuess[i];                 // Store the player guess colour values
            }
            HistoryMoves[HistoryIndex, pgbuttons.Length] = CorrectPlacement;    // Store the number of correctly placed colours (black)
            HistoryMoves[HistoryIndex, pgbuttons.Length + 1] = CorrectColour;   // Store the number of correct colours in the wrong place (white)
        }

        private void DisplayTurns()
        {
            if (CurrentTurn <= 0)
            {
                // Player loses
                label_Info.Content = "You lose! The correct colour code was not found!";
                MessageBox.Show("The correct colour code was not found!", "You lose!", MessageBoxButton.OK);
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


        private void btnConfirm_Click(object sender, RoutedEventArgs e)     // This is the game loop
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
                StoreMovesInHistory();
                DisplayFeedback();
                if (bDidThePlayerWin)
                {
                    MessageBox.Show("You did it! The correct code was found!", "You win!", MessageBoxButton.OK);
                    NewGame();
                }
                // DEBUG START (History)
                //label_Info.Content = HistoryMoves[HistoryIndex, 0] + ", " + HistoryMoves[HistoryIndex, 1] + ", " + HistoryMoves[HistoryIndex, 2] + ", " + HistoryMoves[HistoryIndex, 3] + ", " + HistoryMoves[HistoryIndex, 4] + ", " + HistoryMoves[HistoryIndex, 5];
                // DEBUG END
                HistoryIndex++;
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
            bAllowDuplicateColours = true;
            GenerateCode();
        }

        private void checkBox_DuplicateColours_Unchecked(object sender, RoutedEventArgs e)
        {
            bAllowDuplicateColours = false;
            GenerateCode();
        }

        private void btnAbout_Click(object sender, RoutedEventArgs e)
        {
            AboutBox aboutBox = new AboutBox();
            aboutBox.ShowDialog();
        }

        private void btnHowToPlay_Click(object sender, RoutedEventArgs e)
        {
            HowToPlay gameRules = new HowToPlay();
            gameRules.Show();
        }

        private void btnHistory_Click(object sender, RoutedEventArgs e)
        {
            HistoryView viewMoves = new HistoryView();
            viewMoves.Show();
        }
    }
}