using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;

namespace BioAlgorithm.Core.Representatives
{
    /// <summary>
    /// Interaction logic for HittingSetInputDataControl.xaml
    /// </summary>
    public partial class HittingSetInputDataControl : UserControl
    {
        public HittingSetInputDataControl()
        {
            InitializeComponent();
        }
    }

    //-------------------------------------------------------------------------------------------------------------------
    // class TaskTypeToBrushConverter
    //-------------------------------------------------------------------------------------------------------------------
    [ValueConversion(typeof(int), typeof(Brush))]
    public class TaskTypeToBrushConverter : IValueConverter
    {
        //-------------------------------------------------------------------------------------------------------------------
        public object Convert(object val, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            int regimValue = (int)val;
            //Color color = Color.FromArgb(150, 255, 255, 255);
            Color color = regimValue switch 
            {
                var value when (value & 1) != 0 => Color.FromArgb(255, 255, 100, 100),
                var value when (value & 2) != 0 => Color.FromArgb(255, 150, 190, 255),
                var value when (value & 4) != 0 => Color.FromArgb(255, 200, 50, 0),
                var value when (value & 8) != 0 => Color.FromArgb(255, 0, 255, 70),
                var value when (value & 16) != 0 => Color.FromArgb(255, 100, 255, 255),
                var value when (value & 32) != 0 => Color.FromArgb(255, 0, 255, 255),
                _ => Color.FromArgb(150, 255, 255, 255),
            }; 
            Brush brush = new SolidColorBrush(color);

            return brush;
        }
        //-------------------------------------------------------------------------------------------------------------------
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return null;
        }
        //-------------------------------------------------------------------------------------------------------------------
    }

}
