using System;

namespace OperatoerLibrary
{
    public class DTO_Measurement
    {
        public DTO_Measurement( double measurementData, double gatingLowerValue,
            double gatingUpperValue, DateTime time)
        {
            MeasurementData = measurementData;
            GatingLowerValue = gatingLowerValue;
            GatingUpperValue = gatingUpperValue;
            Time = time;
        }

        public double MeasurementData { get; set; }

        public double GatingLowerValue { get; set; }
        public double GatingUpperValue { get; set; }
        public DateTime Time { get; set; }
    }
}