using ClassLibrary;
using System.Runtime.CompilerServices;
using System.ComponentModel;
using System.Collections.ObjectModel;


namespace WpfApp
{
    public class List : INotifyPropertyChanged
    {
        public SPf Function { get; set; }
        public string? FunctionStr { get; set; }
        public event PropertyChangedEventHandler? PropertyChanged;

        public void OnPropertyChanged([CallerMemberName] string prop="")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }

    public class SPfList : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        public List selectedFunc;
        public ObservableCollection<List> Funcs { get; set; }

        public SPfList()
        {
            Funcs = new ObservableCollection<List>
            {
                new List { Function = SPf.Linear, FunctionStr = "Linear" },
                new List { Function = SPf.Cubic, FunctionStr = "Cubic" },
                new List { Function = SPf.Random, FunctionStr = "Random" }
            };
            selectedFunc = Funcs[0];
        }
        public List SelectedFunc
        {
            get { return selectedFunc; }
            set
            {
                selectedFunc = value;
                OnPropertyChanged(nameof(SelectedFunc));
            }
        }

        public void OnPropertyChanged([CallerMemberName] string prop="")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}
