using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using BioAlgorithmViewModel.Representatives.Messages;
using BioAlgorithmViewModel.Representatives;

namespace BioAlgorithm.Core.Representatives
{
    /// <summary>
    /// Interaction logic for ExecuteInputTaskWindow.xaml
    /// </summary>
    public partial class ExecuteInputTaskWindow : Window
    {
        private readonly StartInputTaskMessage _message;
        public ExecuteInputTaskWindow(StartInputTaskMessage message)
        {
            InitializeComponent();
            _message = message;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            DataContext = new ExecuteInputTaskViewModel(_message);
        }

    }
}
