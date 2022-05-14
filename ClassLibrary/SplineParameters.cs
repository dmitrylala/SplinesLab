using System.ComponentModel;


namespace ClassLibrary
{
    public class SplineParameters : IDataErrorInfo
    {
        public int NumberNodes { get; set; } = 10;
        public double DerivativeLeft { get; set; }
        public double DerivativeRight { get; set; }
        public bool Err { get; set; }

        public SplineParameters()
        {}

        public SplineParameters(double a, double b)
        {
            DerivativeLeft = a;
            DerivativeRight = b;
        }

        public bool SetErr()
        {
            return Err = (NumberNodes <= 2);
        }

        public string this[string columnName]
        {
            get
            {
                string err = "";
                switch (columnName)
                {
                    case "N":
                        if (NumberNodes <= 2)
                            err = "Число точек должно быть больше 2";
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
