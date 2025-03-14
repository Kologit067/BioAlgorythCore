using BioAlgorithmViewModel;
using BioAlgorithmViewModel.Representatives.Messages;
using BioAlgorithmViewModel.Representatives.Utility;
using BioAlgorithm.Core.Representatives;
using FindingRegulatoryMotifs.Enumeration;
using StatisticsStorage.Accumulators;
using System.Windows;
using System.Windows.Data;

namespace BioAlgorithm
{
    /// <summary>
    /// Логика взаимодействия для BioAlgorithmView.xaml
    /// </summary>
    public partial class BioAlgorithmView : Window
    {
        public BioAlgorithmView()
        {
            InitializeComponent();
            Messenger.Default.Register<AlgorithmGroupOpenWindowMessage>(this, OnAlgorithmGroupOpenWindowMessageReceived, typeof(AlgorithmGroupOpenWindowMessage));
            Messenger.Default.Register<StartTaskMessage>(this, OnStartTaskMessageMessageReceived, typeof(StartTaskMessage));

        }

        private void OnAlgorithmGroupOpenWindowMessageReceived(AlgorithmGroupOpenWindowMessage message)
        {
            RepresentativeAlgorithmWindow window = new(message);
            window.Show();
        }

        private void OnStartTaskMessageMessageReceived(StartTaskMessage message)
        {
            PrepareAndExecuteWindow window = new(message);
            window.Show();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            // arrange
            string excpectedMotif = "agcgt";
            string expectedSolutionStartPosition = "3,5,1";
            int expectedResult = 5;

            char[][] charSets = [ ['a','g','t','a','g','c','g','t','a','a'],
            ['t','g','t','g','c','a','g','c','g','t'],
            ['a','a','g','c','g','t','t','a','c','c']];
            char[] alphabet = ['a', 'c', 'g', 't'];
            int substringLength = 5;
#pragma warning disable IDE0090 // Use 'new(...)'
            RegulatoryMotifsSubSequencesEnumeration enumeration = new RegulatoryMotifsSubSequencesEnumeration(charSets, alphabet, substringLength)
            {
                StatisticAccumulator = new FakeRegulatoryMotifsStatisticAccumulator()
            };
#pragma warning restore IDE0090 // Use 'new(...)'
                               // act
            enumeration.Execute();
            // assert
            string solutionStartPosition = string.Join(",", enumeration.SolutionStartPositionList);
            string motif = string.Join("", enumeration.Motif);
            if (motif != excpectedMotif)
                MessageBox.Show($"Motif is wrong.");
            if (solutionStartPosition != expectedSolutionStartPosition)
            MessageBox.Show($"Positions are wrong.");
            if (enumeration.OptimalValue != expectedResult)
                MessageBox.Show($"Result is wrong.");

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var viewModel = new AlgorithmViewModel();
            DataContext = viewModel;
        }
    }
    //-------------------------------------------------------------------------------------------------------------------
    // class VisibilityConverter
    //-------------------------------------------------------------------------------------------------------------------
    [ValueConversion(typeof(bool), typeof(Visibility))]
    public class VisibilityConverter : IValueConverter
    {
        //-------------------------------------------------------------------------------------------------------------------
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            bool visibility = (bool)value;
            if (visibility)
                return Visibility.Visible;
            return Visibility.Collapsed;
        }
        //-------------------------------------------------------------------------------------------------------------------
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return null;
        }
        //-------------------------------------------------------------------------------------------------------------------
    }
    //-------------------------------------------------------------------------------------------------------------------

}
