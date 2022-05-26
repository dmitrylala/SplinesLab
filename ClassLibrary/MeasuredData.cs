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
        public static readonly string NotEnoughNodesMessage = "Число точек должно быть больше 2";
        public static readonly string IncorrectSegmentMessage = "Некорректный сегмент для " +
            "неравномерной сетки";

        public bool ErrorOccured => NotEnoughNodes || IncorrectSegment;
        public bool NotEnoughNodes => NumberNodes <= 2;
        public bool IncorrectSegment => Start >= End;


        public string Error
        {
            get
            {
                if (NotEnoughNodes)
                    return NotEnoughNodesMessage;

                if (IncorrectSegment)
                    return IncorrectSegmentMessage;

                return "";
            }
        }

        public string this[string columnName]
        {
            get
            {
                if ((columnName == "NumberNodes") && NotEnoughNodes)
                    return NotEnoughNodesMessage;

                if ((columnName == "Segment") && IncorrectSegment)
                    return IncorrectSegmentMessage;

                return "";
            }
        }

        public bool HasInside(SplineParameters other) => (
            (Start <= other.IntegralStart) && (other.IntegralEnd <= End)
        );


        public int NumberNodes { get; set; } = 10;
        public double Start { get; set; } = 0;
        public double End { get; set; } = 1;
        public SPf Function { get; set; } = SPf.Linear;
        public double[] Grid { get; set; }
        public double[] Values { get; set; }

        public bool Changed { get; set; } = true;

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
            Grid = new double[NumberNodes];
            Values = new double[NumberNodes];
            Clear();
        }

        public MeasuredData(int n_nodes, double start, double end, SPf function)
        {
            NumberNodes = n_nodes;
            Grid = new double[NumberNodes];
            Values = new double[NumberNodes];
            Clear();

            Start = start; 
            End = end; 
            Function = function;
        }

        public void Generate()
        {
            if (!Changed)
                return;

            Random rnd = new();

            Grid = new double[NumberNodes];
            Grid[0] = Start;
            for (int i = 1; i < NumberNodes - 1; ++i)
                Grid[i] = rnd.Next((int)Start, (int)End) + rnd.NextDouble();
            Grid[NumberNodes - 1] = End;

            Array.Sort(Grid);

            Values = new double[NumberNodes];
            switch (Function)
            {
                case SPf.Random:
                    for (int i = 0; i < NumberNodes; ++i) 
                        Values[i] = 13 * rnd.NextDouble();
                    break;
                case SPf.Linear:
                    for (int i = 0; i < NumberNodes; ++i) 
                        Values[i] = Grid[i];
                    break;
                case SPf.Cubic:
                    for (int i = 0; i < NumberNodes; ++i) 
                        Values[i] = Math.Pow(Grid[i], 3);
                    break;
            }
            Changed = false;
        }

        public void Clear()
        {
            Array.Clear(Grid, 0, NumberNodes);
            Array.Clear(Values, 0, NumberNodes);
        }

        public ObservableCollection<string>? _str = new();

        public override string ToString()
        {
            string res = $"[{Start}, {End}] count: {NumberNodes}\n";
            for (int i = 0; i < NumberNodes - 1; i++)
                res += $"{Grid[i]}, ";
            res += $"{Grid[NumberNodes - 1]}\n\n\n";
            for (int i = 0; i < NumberNodes - 1; i++)
                res += $"{Values[i]}, ";
            res += $"{Values[NumberNodes - 1]}\n";
            return res;
        }
    }
}



