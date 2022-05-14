using System.Runtime.InteropServices;
using System.Collections.ObjectModel;


namespace ClassLibrary
{
    public class SplinesData
    {
        public MeasuredData Data { get; set; }
        public SplineParameters Parameters { get; set; }
        public double? Integral { get; set; } = null;
        public double? DerivativeLeft { get; set; } = null;
        public double? DerivativeRight { get; set; } = null;

        public SplinesData(MeasuredData data, SplineParameters parameters)
        {
            Data = data;
            Parameters = parameters;
        }

        public SplinesData()
        {
            Data = new();
            Parameters = new();
        }

        [DllImport("\\..\\..\\..\\..\\x64\\Debug\\Splines.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern double Interpolate(int length, double[] points, double[] func, double[] res, double[] der, int gridlen, double[] grid, double[] limits, double[] integrals);


        public double[] Spline(ref double a, ref double[] Int) 
        {
            double[] spline = new double[3 * Parameters.NumberNodes];

            a = Interpolate(
                Data.NodesNumber, Data.Grid, Data.Values,
                spline, 
                new double[] { Parameters.DerivativeLeft, Parameters.DerivativeRight },
                Parameters.NumberNodes, new double[] { Data.Start, Data.End }, 
                new double[] { Data.IntegralStart, Data.IntegralEnd }, Int
            );

            Integral = Int[0];
            DerivativeLeft = spline[1];
            DerivativeRight = spline[^2];
            return spline;
        }

        public string IntegralInfo
        {
            get
            {
                if (Integral == null || DerivativeLeft == null || DerivativeRight == null)
                    return "Интеграл не посчитан";
                string res = $"Интеграл посчитан на отрезке [{Data.IntegralStart}; {Data.IntegralEnd}]\n";
                res += $"Значение: {Integral}\n";
                res += $"Производные на границах:\n\tСлева {DerivativeLeft}\n\tСправа {DerivativeRight}";
                return res;
            }
        }

        public ObservableCollection<string>? Str
        {
            get
            {
                if (!Data.IsZeros)
                {
                    Data._str = new();
                    for (int i = 0; i < Data.NodesNumber; i++)
                        Data._str.Add($"x[{i + 1}]: {Data.Grid[i]:f8}\t\ty[{i + 1}]: {Data.Values[i]:f8}");
                    return Data._str;
                }
                return null;
            }
            set
            {
                Data._str = value;
            }
        }
    }
}
