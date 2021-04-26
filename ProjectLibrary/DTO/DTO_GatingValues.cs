using System;
using System.Collections.Generic;
using System.Text;

namespace OperatoerLibrary.DTO
{
    public class DTO_GatingValues
    {
        public double UpperGatingValue { get; set; }
        public double LowerGatingValue { get; set; }

        public DTO_GatingValues(double upperGatingValue, double lowerGatingValue)
        {
            UpperGatingValue = upperGatingValue;
            LowerGatingValue = lowerGatingValue;
        }



    }
}
