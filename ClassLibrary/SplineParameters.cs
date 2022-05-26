using System.ComponentModel;


namespace ClassLibrary
{
    public class SplineParameters : IDataErrorInfo
    {
        private readonly string IncorrectIntegralSegmentMessage = "Некорректный сегмент для " +
            "вычисления интеграла";

        public bool ErrorOccured => NotEnoughNodes || IncorrectIntegralSegment;
        public bool NotEnoughNodes => NumberNodes <= 2;
        public bool IncorrectIntegralSegment => IntegralStart >= IntegralEnd;


        public string Error
        {
            get
            {
                if (NotEnoughNodes)
                    return MeasuredData.NotEnoughNodesMessage;

                if (IncorrectIntegralSegment)
                    return IncorrectIntegralSegmentMessage;

                return "";
            }
        }

        public string this[string columnName]
        {
            get
            {
                if (columnName == "Nodes" && NotEnoughNodes)
                    return MeasuredData.NotEnoughNodesMessage;

                if (columnName == "IntegralSegment" && IncorrectIntegralSegment)
                    return IncorrectIntegralSegmentMessage;

                return "";
            }
        }

        public int NumberNodes { get; set; } = 10;
        public double DerivativeLeft { get; set; } = 0;
        public double DerivativeRight { get; set; } = 0;

        public double IntegralStart { get; set; } = 0;
        public double IntegralEnd { get; set; } = 1;
    }
}
