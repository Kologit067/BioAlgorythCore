using System.Windows;
using BioAlgorithmViewModel.Representatives;
using BioAlgorithmViewModel.Representatives.Messages;

namespace BioAlgorithm.Core.Representatives
{
    /// <summary>
    /// Interaction logic for PrepareAndExecuteWindow.xaml
    /// </summary>
    public partial class PrepareAndExecuteWindow : Window
    {
        private readonly StartTaskMessage _message;
        public PrepareAndExecuteWindow(StartTaskMessage message)
        {
            InitializeComponent();
            _message = message;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            DataContext = new PrepareAndExecuteViewModel(_message);
        }
    }
}
