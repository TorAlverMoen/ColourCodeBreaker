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
        }

        static Brush[] Colours = [Brushes.Red, Brushes.Green, Brushes.Yellow, Brushes.Orange, Brushes.Blue, Brushes.White];
        static Brush[] Feedback = [Brushes.Black, Brushes.White];
        static int colourIndex = 0;
        static int feedbackIndex = 0;
        static double positionLeft = 40;
        static double positionTop = 100;
        static double width = 25;
        static double height = 25;

        Label historyLabel = new Label
        {
            Content = "",
            Margin = new Thickness(positionLeft, positionTop, 0, 0),
            HorizontalAlignment = HorizontalAlignment.Left,
            VerticalAlignment = VerticalAlignment.Top,
            Width = width,
            Height = height,
            Background = Colours[colourIndex]
        };
    }
}
