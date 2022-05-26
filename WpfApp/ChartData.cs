using System;
using LiveCharts;
using LiveCharts.Wpf;
using LiveCharts.Defaults;
using ClassLibrary;


namespace WpfApp
{
    public class ChartData
    {
        public SeriesCollection Data { get; set; }
        public Func<double, string> Form { get; set; }
        public int CountPoints { get; set; } = 0;
        public int CountSplines { get; set; } = 0;

        public ChartData()
        {
            Data = new();
            Form = value => value.ToString("F4");
        }

        public void PlotPoints(MeasuredData points)
        {
            ChartValues<ObservablePoint> chart_values = new();
            for (int i = 0; i < points.NumberNodes; ++i)
                chart_values.Add(new(points.Grid[i], points.Values[i]));

            Data.Add(new ScatterSeries { 
                Title = $"Points{CountPoints}", 
                Values = chart_values 
            });
            CountPoints++;
        }

        public void PlotSpline(double[] grid, double[] values)
        {
            ChartValues<ObservablePoint> chart_values = new();
            for (int i = 0; i < values.Length; ++i)
                chart_values.Add(new(grid[i], values[i]));

            Data.Add(new LineSeries { 
                Title = $"Spline{CountSplines}", 
                Values = chart_values, 
                PointGeometry = null 
            });
            CountSplines++;
        }

        public void Clear()
        {
            Data.Clear();
            CountPoints = 0;
            CountSplines = 0;
        }
    }
}
