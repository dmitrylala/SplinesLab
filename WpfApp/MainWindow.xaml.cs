using System;
using System.Windows;
using System.Windows.Input;


namespace WpfApp
{
    public partial class MainWindow : Window
    {
        public class MessageBoxErrorReporter
        {
            public void ReportError(string message) => MessageBox.Show(message);
        }

        public ViewData VData { get; set; } = new();
        public MessageBoxErrorReporter Reporter { get; set; } = new();

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            DataContext = VData;
        }

        private void TextBox(object sender, TextCompositionEventArgs e)
        {
            double val;
            if (!Double.TryParse(e.Text, out val))
                e.Handled = true;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            VData.Clear();
        }

        private void MeasuredData_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = !VData.Splines.Data.ErrorOccured;
        }

        private void MeasuredData_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            VData.Splines.Data.Function = VData.SpfList.selectedFunc.Function;
            VData.Changed = true;
            VData.MdSetGrid();
            VData.Chart.PlotPoints(VData.Splines.Data);
        }

        private void Splines_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            bool is_inside = VData.Splines.Data.HasInside(VData.Splines.Parameters);
            bool is_zeros = VData.Splines.Data.IsZeros;
            e.CanExecute = !VData.Splines.Parameters.ErrorOccured && is_inside && !is_zeros;
        }

        private void Splines_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            double a = 213123;
            double[] Int = new double[1];
            double[] r = VData.Spline(ref a, ref Int);

            int length = VData.Splines.Parameters.NumberNodes;
            double[] res = new double[length];
            for (int i = 0; i < length; ++i)
                res[i] = r[3 * i];
            double[] grid = new double[length];
            for (int i = 0; i < length; ++i)
                grid[i] = VData.Splines.Data.Start + i * (VData.Splines.Data.End - VData.Splines.Data.Start) / (length - 1);

            VData.Chart.PlotSpline(grid, res);
        }

        private void TextBox1_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            int val;
            if (!Int32.TryParse(e.Text, out val))
                e.Handled = true;
        }

        private void TextBox2_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            int val;
            if (!Int32.TryParse(e.Text, out val) && e.Text != ",")
                e.Handled = true;
        }  
    }


    public static class Cmd
    {
        public static readonly RoutedUICommand MeasuredData = new
        (
            "MeasuredData",
            "MeasuredData",
            typeof(Cmd),
            new InputGestureCollection()
            {
                new KeyGesture(Key.D1, ModifierKeys.Control)
            }
        );

        public static readonly RoutedUICommand Splines = new
        (
            "Splines",
            "Splines",
            typeof(Cmd),
            new InputGestureCollection()
            {
                new KeyGesture(Key.D2, ModifierKeys.Control)
            }
        );
    }
}
