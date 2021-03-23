using System;
using System.Collections.Generic;
using System.Text;

namespace OperatoerLibrary
{
    public class DTO_Measurement
    {
        public DateTime TimeStamp { get; set; }
        public double MeasurementData { get; set; }

        public double GatingLowerValue { get; set; }
        public double GatingUpperValue { get; set; }
        

        public DTO_Measurement(DateTime timestamp, double measurementData, double gatingLowerValue, double gatingUpperValue)
        {
            TimeStamp = timestamp;
            MeasurementData = measurementData;
            GatingLowerValue = gatingLowerValue;
            GatingUpperValue = gatingUpperValue;
        }

    }
}
