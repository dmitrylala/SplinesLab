using ClassLibrary;
using System.ComponentModel;
using System.Runtime.CompilerServices;


namespace WpfApp
{
    public class ViewData : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }

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
            Splines.Data.Generate();
            OnPropertyChanged(nameof(Splines));
        }

        public double[] Spline(ref double a, ref double[] Int)
        {
            double[] r = Splines.Spline(ref a, ref Int);
            OnPropertyChanged(nameof(Splines));
            return r;
        }

        public void Clear()
        {
            Chart.Clear();
            Splines.Clear();
            OnPropertyChanged(nameof(Splines));
        }
    }
}
