using ClassLibrary;
using System.ComponentModel;
using System.Runtime.CompilerServices;


namespace WpfApp
{
    public class ViewData : INotifyPropertyChanged
    {
        public SplinesData Splines { get; set; }
        public ChartData Chart { get; set; }
        public SPfList SpfList { get; set; }

        public ViewData()
        {
            Splines = new SplinesData();
            Chart = new ChartData();
            SpfList = new SPfList();
        }
        public bool Changed
        {
            get { return Splines.Data.Changed; }
            set { Splines.Data.Changed = value; }
        }

        public void MdSetGrid()
        {
            Splines.Data.SetGrid();
            OnPropertyChanged(nameof(Splines));
        }

        public double[] Spline(ref double a, ref double[] Int)
        {
            double[] r = Splines.Spline(ref a, ref Int);
            OnPropertyChanged(nameof(Splines));
            return r;
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop="")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }

        internal void Clear()
        {
            Chart.Clear();
            Splines.Integral = null; 
            Splines.DerivativeLeft = null; 
            Splines.DerivativeRight = null;

            for (int i = 0; i < Splines.Data.NodesNumber; ++i)
            {
                Splines.Data.Grid[i] = 0;
                Splines.Data.Values[i] = 0;
            }

            OnPropertyChanged(nameof(Splines));
        }
    }
}
