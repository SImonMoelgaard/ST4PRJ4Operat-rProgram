using System;

namespace OperatoerLibrary
{
    public class DTO_Measurement
    {
        public DTO_Measurement( double measurementData, double gatingLowerValue,
            double gatingUpperValue)
        {
            MeasurementData = measurementData;
            GatingLowerValue = gatingLowerValue;
            GatingUpperValue = gatingUpperValue;
        }

        public double MeasurementData { get; set; }

        public double GatingLowerValue { get; set; }
        public double GatingUpperValue { get; set; }
    }
}