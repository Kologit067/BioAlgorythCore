using System.Windows;
using BioAlgorithmViewModel.Representatives;
using BioAlgorithmViewModel.Representatives.Messages;

namespace BioAlgorithm.Core.Representatives
{
    /// <summary>
    /// Interaction logic for DeleteAlgorithmInputWindow.xaml
    /// </summary>
    public partial class DeleteAlgorithmInputWindow : Window
    {
        private readonly DeleteAlgorithmInputMessage _message;
        public DeleteAlgorithmInputWindow(DeleteAlgorithmInputMessage message)
        {
            InitializeComponent();
            _message = message;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            DeleteAlgorithmInputViewModel context = new DeleteAlgorithmInputViewModel(_message);
            context.CloseAction += () =>
            {
                Close();
            };
            DataContext = context;
        }
    }
}
