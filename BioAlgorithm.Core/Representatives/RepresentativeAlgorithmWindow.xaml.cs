using BioAlgorithmViewModel.Representatives;
using BioAlgorithmViewModel.Representatives.Messages;
using BioAlgorithm.Data.Representatives.Data;
using System.Windows;

namespace BioAlgorithm.Core.Representatives
{
    /// <summary>
    /// Interaction logic for RepresentativeAlgorithmWindow.xaml
    /// </summary>
    public partial class RepresentativeAlgorithmWindow : Window
    {
        private RepresentativePerformanceViewModel? viewModel;
        private AlgorithmGroupOpenWindowMessage message;
        public RepresentativeAlgorithmWindow(AlgorithmGroupOpenWindowMessage message)
        {
            InitializeComponent();
            this.message  = message;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            RepresentativesRepository representativesRepository = new RepresentativesRepository();
            viewModel = new RepresentativePerformanceViewModel(representativesRepository, message);
            DataContext = viewModel;
        }
    }
}
