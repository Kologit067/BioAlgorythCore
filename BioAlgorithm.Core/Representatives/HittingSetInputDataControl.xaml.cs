using Microsoft.Extensions.Configuration;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using Newtonsoft.Json.Linq;

namespace BioAlgorithm.Core.Representatives
{
    /// <summary>
    /// Interaction logic for HittingSetInputDataControl.xaml
    /// </summary>
    public partial class HittingSetInputDataControl : UserControl
    {
        public static string TypeTaskColorSchema = "";
        public HittingSetInputDataControl()
        {
            InitializeComponent(); 
            ConfigurationManager configurationManager = new ConfigurationManager();
            var builder = new ConfigurationBuilder().AddXmlFile("app.config");

            var appConfiguration = builder.Build();
            TypeTaskColorSchema = appConfiguration.GetSection("ColorSchema:TypeTask").Value;
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
            Color color;
            if (HittingSetInputDataControl.TypeTaskColorSchema == "common")
            { 
                byte rPart = regimValue switch
                {
                    var value when ((value & 16) != 0 && (value & 32) != 0) => 200,
                    var value when (value & 16) != 0 => 140,
                    var value when (value & 32) != 0 => 70,
                    _ => 0,
                };
                byte gPart = regimValue switch
                {
                    var value when ((value & 2) != 0 && (value & 4) != 0) => 200,
                    var value when (value & 2) != 0 => 140,
                    var value when (value & 4) != 0 => 70,
                    _ => 0,
                };
                byte bPart = regimValue switch
                {
                    var value when ((value & 1) != 0 && (value & 8) != 0) => 200,
                    var value when (value & 8) != 0 => 140,
                    var value when (value & 1) != 0 => 70,
                    _ => 0,
                };
                color = Color.FromArgb(150, rPart, gPart, bPart);
            }
            else if (HittingSetInputDataControl.TypeTaskColorSchema == "reduce")
            {
                byte rPart = regimValue switch
                {
                    var value when (value & 16) != 0 => 190,
                    _ => 0,
                };
                byte gPart = regimValue switch
                {
                    var value when ((value & 2) != 0) => 180,
                    _ => 0,
                };
                byte bPart = regimValue switch
                {
                    var value when ((value & 1) != 0 && (value & 8) != 0 && (value & 32) != 0 && (value & 4) != 0) => 250,
                    var value when ((value & 1) != 0 && (value & 8) != 0 && (value & 32) != 0) => 230,
                    var value when ((value & 1) != 0 && (value & 8) != 0 && (value & 4) != 0) => 207,
                    var value when ((value & 1) != 0 && (value & 32) != 0 && (value & 4) != 0) => 189,
                    var value when ((value & 8) != 0 && (value & 32) != 0 && (value & 4) != 0) => 172,
                    var value when ((value & 1) != 0 && (value & 8) != 0) => 155,
                    var value when ((value & 1) != 0 && (value & 32) != 0) => 138,
                    var value when ((value & 1) != 0 && (value & 8) != 0) => 119,
                    var value when ((value & 1) != 0 && (value & 4) != 0) => 102,
                    var value when ((value & 32) != 0 && (value & 4) != 0) => 85,
                    var value when (value & 8) != 0 => 68,
                    var value when (value & 1) != 0 => 51,
                    var value when (value & 32) != 0 => 34,
                    var value when (value & 4) != 0 => 17,
                    _ => 0,
                };
                int sum = (int)rPart + (int)gPart + (int)bPart;
                if (sum > 200)
                    color = Color.FromArgb(255, 0, 0, 0);
                else
                    color = Color.FromArgb(255, 255, 255, 255);
            }
            else
            {
                color = regimValue switch
                {
                    var value when (value & 16) != 0 => Color.FromArgb(255, 100, 255, 255),
                    var value when (value & 32) != 0 => Color.FromArgb(255, 0, 255, 255),
                    var value when (value & 2) != 0 => Color.FromArgb(255, 150, 190, 255),
                    var value when (value & 4) != 0 => Color.FromArgb(255, 200, 50, 0),
                    var value when (value & 8) != 0 => Color.FromArgb(255, 0, 255, 70),
                    var value when (value & 1) != 0 => Color.FromArgb(255, 255, 150, 150),
                    _ => Color.FromArgb(150, 255, 255, 255),
                };
            }
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
    //-------------------------------------------------------------------------------------------------------------------
    // class TaskTypeToForeColorConverter
    //-------------------------------------------------------------------------------------------------------------------
    [ValueConversion(typeof(int), typeof(Brush))]
    public class TaskTypeToForeColorConverter : IValueConverter
    {
        //-------------------------------------------------------------------------------------------------------------------
        public object Convert(object val, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            int regimValue = (int)val;
            Color color;
            if (HittingSetInputDataControl.TypeTaskColorSchema == "common")
            {
                byte rPart = regimValue switch
                {
                    var value when ((value & 16) != 0 && (value & 32) != 0) => 200,
                    var value when (value & 16) != 0 => 140,
                    var value when (value & 32) != 0 => 70,
                    _ => 0,
                };
                byte gPart = regimValue switch
                {
                    var value when ((value & 2) != 0 && (value & 4) != 0) => 200,
                    var value when (value & 2) != 0 => 140,
                    var value when (value & 4) != 0 => 70,
                    _ => 0,
                };
                byte bPart = regimValue switch
                {
                    var value when ((value & 1) != 0 && (value & 8) != 0) => 200,
                    var value when (value & 8) != 0 => 140,
                    var value when (value & 1) != 0 => 70,
                    _ => 0,
                };
                int sum = (int)rPart + (int)gPart + (int)bPart;
                if (sum > 200)
                    color = Color.FromArgb(255, 0, 0, 0);
                else
                    color = Color.FromArgb(255, 255, 255, 255);
            }
            else if (HittingSetInputDataControl.TypeTaskColorSchema == "reduce")
            {
                byte rPart = regimValue switch
                {
                    var value when (value & 16) != 0 => 190,
                    _ => 0,
                };
                byte gPart = regimValue switch
                {
                    var value when ((value & 2) != 0) => 180,
                    _ => 0,
                };
                byte bPart = regimValue switch
                {
                    var value when ((value & 1) != 0 && (value & 8) != 0 && (value & 32) != 0 && (value & 4) != 0) => 250,
                    var value when ((value & 1) != 0 && (value & 8) != 0 && (value & 32) != 0 ) => 230,
                    var value when ((value & 1) != 0 && (value & 8) != 0 && (value & 4) != 0) => 207,
                    var value when ((value & 1) != 0 && (value & 32) != 0 && (value & 4) != 0) => 189,
                    var value when ((value & 8) != 0 && (value & 32) != 0 && (value & 4) != 0) => 172,
                    var value when ((value & 1) != 0 && (value & 8) != 0 ) => 155,
                    var value when ((value & 1) != 0 && (value & 32) != 0) => 138,
                    var value when ((value & 1) != 0 && (value & 8) != 0 ) => 119,
                    var value when ((value & 1) != 0 && (value & 4) != 0) => 102,
                    var value when ((value & 32) != 0 && (value & 4) != 0) => 85,
                    var value when (value & 8) != 0 => 68,
                    var value when (value & 1) != 0 => 51,
                    var value when (value & 32) != 0 => 34,
                    var value when (value & 4) != 0 => 17,
                    _ => 0,
                };
                int sum = (int)rPart + (int)gPart + (int)bPart;
                if (sum > 200)
                    color = Color.FromArgb(255, 0, 0, 0);
                else
                    color = Color.FromArgb(255, 255, 255, 255);
            }
            else
            {
                color = Color.FromArgb(255, 0, 0, 0);
            }
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
    //-------------------------------------------------------------------------------------------------------------------
    // class TaskTypeToBinaryConverter
    //-------------------------------------------------------------------------------------------------------------------
    [ValueConversion(typeof(int), typeof(string))]
    public class TaskTypeToBinaryConverter : IValueConverter
    {
        //-------------------------------------------------------------------------------------------------------------------
        public object Convert(object val, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if ((int)val < 0)
                return "";
            int typeValue = (int)val;
            string binary = System.Convert.ToString(typeValue, 2).PadLeft(6, '0');

            return binary;
        }
        //-------------------------------------------------------------------------------------------------------------------
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {

            return null;
        }
        //-------------------------------------------------------------------------------------------------------------------
    }

    //-------------------------------------------------------------------------------------------------------------------
    // class GreedyCompareToBrushConverter
    //-------------------------------------------------------------------------------------------------------------------
    [ValueConversion(typeof(int), typeof(Brush))]
    public class GreedyCompareToBrushConverter : IValueConverter
    {
        //-------------------------------------------------------------------------------------------------------------------
        public object Convert(object val, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            int regimValue = (int)val;
            //Color color = Color.FromArgb(150, 255, 255, 255);
            Color color;
            byte rPart = regimValue switch
            {
                var value when ((value & 1) != 0 && (value & 8) != 0) => 200,
                var value when (value & 1) != 0 => 140,
                var value when (value & 8) != 0 => 70,
                _ => 0,
            };
            byte gPart = regimValue switch
            {
                var value when ((value & 2) != 0) => 200,
                _ => 0,
            };
            byte bPart = regimValue switch
            {
                var value when ((value & 4) != 0) => 200,
                _ => 0,
            };
            color = Color.FromArgb(150, rPart, gPart, bPart);
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
    //-------------------------------------------------------------------------------------------------------------------
    // class GreedyCompareToForeColorConverter
    //-------------------------------------------------------------------------------------------------------------------
    [ValueConversion(typeof(int), typeof(Brush))]
    public class GreedyCompareToForeColorConverter : IValueConverter
    {
        //-------------------------------------------------------------------------------------------------------------------
        public object Convert(object val, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            int regimValue = (int)val;
            Color color;
            byte rPart = regimValue switch
            {
                var value when ((value & 1) != 0 && (value & 8) != 0) => 200,
                var value when (value & 1) != 0 => 140,
                var value when (value & 8) != 0 => 70,
                _ => 0,
            };
            byte gPart = regimValue switch
            {
                var value when ((value & 2) != 0) => 200,
                _ => 0,
            };
            byte bPart = regimValue switch
            {
                var value when ((value & 4) != 0) => 200,
                _ => 0,
            };
            int sum = (int)rPart + (int)gPart + (int)bPart;
            if (sum > 300)
                color = Color.FromArgb(255, 0, 0, 0);
            else
                color = Color.FromArgb(255, 255, 255, 255);
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
    //-------------------------------------------------------------------------------------------------------------------
    // class GreedyComparisonToBinaryConverter
    //-------------------------------------------------------------------------------------------------------------------
    [ValueConversion(typeof(int), typeof(string))]
    public class GreedyComparisonToBinaryConverter : IValueConverter
    {
        //-------------------------------------------------------------------------------------------------------------------
        public object Convert(object val, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if ((int)val < 0)
                return "";
            int typeValue = (int)val;
            string binary = System.Convert.ToString(typeValue, 2).PadLeft(4, '0');

            return binary;
        }
        //-------------------------------------------------------------------------------------------------------------------
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {

            return null;
        }
        //-------------------------------------------------------------------------------------------------------------------
    }

}
