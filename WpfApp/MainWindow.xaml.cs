using System;
using System.Windows;
using System.Windows.Input;


namespace WpfApp
{
    public partial class MainWindow : Window
    {
        public ViewData VData { get; set; } = new();

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
            VData.Changed = true;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            VData.Clear();
        }

        private void MeasuredData_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = !VData.Splines.Data.SetErr() && !VData.Splines.Parameters.SetErr();
        }

        private void MeasuredData_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            VData.Splines.Data.Function = VData.SpfList.selectedFunc.Function;
            VData.Changed = true;
            VData.MdSetGrid();
            VData.Chart.AddPlot(VData.Splines.Data.Grid, VData.Splines.Data.Values, 2, "Points");
        }

        private void Splines_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = !VData.Splines.Data.SetErr() && !VData.Splines.Parameters.SetErr();
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

            VData.Chart.AddPlot(grid, res, 1, "Spline");
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
