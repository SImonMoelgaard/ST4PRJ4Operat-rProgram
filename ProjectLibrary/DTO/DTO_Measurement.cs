using System;
using System.Collections.Generic;
using System.Text;

namespace OperatoerLibrary
{
    public class DTO_Measurement
    {
        public DateTime timeStamp { get; set; }
        public double measurementData { get; set; }

        public DTO_Measurement(DateTime _timestamp, double _measurementData)
        {
            timeStamp = _timestamp;
            measurementData = _measurementData;
        }

    }
}
