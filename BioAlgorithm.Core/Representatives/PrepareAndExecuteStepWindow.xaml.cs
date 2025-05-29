using System.Windows;
using BioAlgorithmViewModel.Representatives;
using BioAlgorithmViewModel.Representatives.Messages;

namespace BioAlgorithm.Core.Representatives
{
    /// <summary>
    /// Interaction logic for PrepareAndExecuteStepWindow.xaml
    /// </summary>
    public partial class PrepareAndExecuteStepWindow : Window
    {
        private readonly StartTaskStepMessage _message;
        public PrepareAndExecuteStepWindow(StartTaskStepMessage message)
        {
            InitializeComponent();
            _message = message;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            DataContext = new PrepareAndExecuteStepViewModel(_message);
        }
    }
}
