using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace ColourCodeBreaker
{
    /// <summary>
    /// Interaction logic for HistoryView.xaml
    /// </summary>
    public partial class HistoryView : Window
    {
        public HistoryView()
        {
            InitializeComponent();
            DisplayHistoryTest();
        }

        const int COLOUR_LABEL_SIZE = 25;
        const int FEEDBACK_LABEL_SIZE = 5;
        const int START_ROW_POS = 80;
        const int LINE_SPACING = 30;
        const int LABEL_SPACING = 40;
        const int CODE_SIZE = 4;

        static Brush[] Colours = [Brushes.Red, Brushes.Green, Brushes.Yellow, Brushes.Orange, Brushes.Blue, Brushes.White];
        static Brush[] Feedback = [Brushes.Black, Brushes.White];

        static int colourIndex = 0;
        static int feedbackIndex = 0;
        static double positionLeft = 40;
        static double positionTop = 100;
        static double width = 25;
        static double height = 25;
        static double currentLineNumber = 1;

        //<Label x:Name="label1" Content="1." HorizontalAlignment="Left" Margin="30,80,0,0" VerticalAlignment="Top" Foreground="LightGray" FontSize="14"/>
        Label lineNumbers = new Label
        {
            Content = currentLineNumber.ToString(),
            Margin = new Thickness(positionLeft, positionTop, 0, 0),
            HorizontalAlignment = HorizontalAlignment.Left,
            VerticalAlignment = VerticalAlignment.Top,
            FontSize = 14
        };

        Label colourLabel = new Label
        {
            Content = "",
            Margin = new Thickness(positionLeft, positionTop, 0, 0),
            HorizontalAlignment = HorizontalAlignment.Left,
            VerticalAlignment = VerticalAlignment.Top,
            FontSize = 14,
            Width = width,
            Height = height,
            Background = Colours[colourIndex]
        };

        // Colour values: 1 - Red, 2 - Green, 3 - Yellow, 4 - Orange, 5 - Blue, 6 - White
        void DisplayColourLabel(double top, double left, int colour)
        {
            // Set colour label size
            width = COLOUR_LABEL_SIZE;
            height = COLOUR_LABEL_SIZE;

            // Set label position
            positionTop = top;
            positionLeft = left;

            // Set label colour
            colourIndex = colour;

            // Display the label
            PlayerMovesHistory.Children.Add(colourLabel);
        }

        Label feedbackLabel = new Label
        {
            Content = "",
            Margin = new Thickness(positionLeft, positionTop, 0, 0),
            HorizontalAlignment = HorizontalAlignment.Left,
            VerticalAlignment = VerticalAlignment.Top,
            FontSize = 14,
            Width = width,
            Height = height,
            Background = Feedback[feedbackIndex]
        };

        // feedback variable: 0 = black (correct colour), 1 = white (correct colour, wrong place)
        void DisplayFeedbackLabel(double top, double left, int feedback)
        {
            // Set feedback label size
            width = FEEDBACK_LABEL_SIZE;
            height = FEEDBACK_LABEL_SIZE;

            // Set label position
            positionTop = top;
            positionLeft = left;

            // Set feedback type
            feedbackIndex = feedback;

            // Display the label
            PlayerMovesHistory.Children.Add(feedbackLabel);
        }

        void DisplayPlayerMove(double currentTurn, int colour, int feedback, double top, double left)
        {
            double tempTop = START_ROW_POS + LINE_SPACING * currentTurn;

            currentLineNumber = currentTurn;
            PlayerMovesHistory.Children.Add(lineNumbers);

            for (int i = 0; i < CODE_SIZE; i++)
            {
                // TODO:
                //   set position left and top
                //   set colourIndex

            }
        }

        void DisplayHistoryTest()
        {
            PlayerMovesHistory.Children.Add(colourLabel);
        }

    }
}
