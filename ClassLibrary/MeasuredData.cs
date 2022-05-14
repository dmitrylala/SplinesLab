using System.ComponentModel;
using System.Collections.ObjectModel;


namespace ClassLibrary
{
    public enum SPf
    {
        Linear,
        Cubic,
        Random
    }

    public class MeasuredData : IDataErrorInfo
    {
        public int NodesNumber { get; set; } = 10;
        public double Start { get; set; } = 0;
        public double End { get; set; } = 1;
        public SPf Function { get; set; } = SPf.Linear;
        public double[] Grid { get; set; }
        public double[] Values { get; set; }
        public double IntegralStart { get; set; } = 0;
        public double IntegralEnd { get; set; } = 1;


        public bool Changed { get; set; } = true;
        public bool Err { get; set; } = false;
        public bool IsZeros
        {
            get
            {
                bool grid_is_zero = Grid.All(elem => elem == 0);
                bool values_are_zero = Values.All(elem => elem == 0);
                return grid_is_zero && values_are_zero;
            }
        }

        public MeasuredData()
        {
            Grid = new double[NodesNumber]; 
            Values = new double[NodesNumber]; 
        }

        public MeasuredData(int n_nodes, double start, double end, SPf function)
        {
            NodesNumber = n_nodes;
            Grid = new double[NodesNumber];
            Values = new double[NodesNumber];

            Start = start; 
            End = end; 
            Function = function;
        }

        public void RandomGrid()
        {
            Random rnd = new();

            Grid = new double[NodesNumber];
            Grid[0] = Start;
            for (int i = 1; i < NodesNumber - 1; ++i)
                Grid[i] = rnd.Next((int)Start, (int)End) + rnd.NextDouble();
            Grid[NodesNumber - 1] = End;

            Array.Sort(Grid);
        }

        public void SetGrid()
        {
            if (!Changed) 
                return;

            RandomGrid();

            Values = new double[NodesNumber];
            switch (Function)
            {
                case SPf.Random:
                    Random rnd = new();
                    for (int i = 0; i < NodesNumber; ++i) 
                        Values[i] = 13 * rnd.NextDouble();
                    break;
                case SPf.Linear:
                    for (int i = 0; i < NodesNumber; ++i) 
                        Values[i] = Grid[i];
                    break;
                case SPf.Cubic:
                    for (int i = 0; i < NodesNumber; ++i) 
                        Values[i] = Math.Pow(Grid[i], 3);
                    break;
            }
            Changed = false; 
            Err = (NodesNumber <= 2) || (Start >= End);
        }

        public bool SetErr()
        {
            bool not_enough_nodes = NodesNumber <= 2;
            bool incorrect_segment = Start >= End;
            bool incorrect_integral_segment = !(Start <= IntegralStart && IntegralStart < IntegralEnd && IntegralEnd <= End);
            return Err = not_enough_nodes || incorrect_segment || incorrect_integral_segment;
        }

        public ObservableCollection<string>? _str = new();

        public override string ToString()
        {
            string res = $"[{Start}, {End}] count: {NodesNumber}\n";
            for (int i = 0; i < NodesNumber - 1; i++)
                res += $"{Grid[i]}, ";
            res += $"{Grid[NodesNumber - 1]}\n\n\n";
            for (int i = 0; i < NodesNumber - 1; i++)
                res += $"{Values[i]}, ";
            res += $"{Values[NodesNumber - 1]}\n";
            return res;
        }

        public string this[string columnName]
        {
            get
            {
                string err = "";
                switch (columnName)
                {
                    case "N":
                        if (NodesNumber <= 2)
                            err = "Число точек должно быть больше 2";
                        break;
                    case "Start": case "End":
                        if (Start >= End || !(Start <= IntegralStart && IntegralStart < IntegralEnd && IntegralEnd <= End))
                            err = "Правый конец меньше левого";
                        break;
                    case "Int_Start": case "Int_End":
                        if (IntegralStart >= IntegralEnd || !(Start <= IntegralStart && IntegralStart < IntegralEnd && IntegralEnd <= End))
                            err = "Неверный отрезок для интеграла";
                        break;
                    default:
                        break;
                }
                return err;
            }
        }

        public string Error => throw new NotImplementedException();
    }
}



