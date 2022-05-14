using System;
using LiveCharts;
using LiveCharts.Wpf;
using LiveCharts.Defaults;


namespace WpfApp
{
    public class ChartData
    {
        public SeriesCollection Data { get; set; }
        public Func<double, string> Form { get; set; }

        public ChartData()
        {
            Data = new();
            Form = value => value.ToString("F4");
        }

        public void AddPlot(double[] grid, double[] values, int mode, string title)
        {
            ChartValues<ObservablePoint> chart_values = new();
            for (int i = 0; i < values.Length; ++i)
                chart_values.Add(new(grid[i], values[i]));

            if (mode == 1) // spline mode
                Data.Add(new LineSeries { Title = title, Values = chart_values, PointGeometry = null });
            else
                Data.Add(new ScatterSeries { Title = title, Values = chart_values });
        }

        public void Clear()
        {
            Data.Clear();
        }
    }
}
