using System;
using System.Collections.Generic;
using System.Text;

namespace OperatoerLibrary.Filters
{
    public  interface IBaseLineFilter
    {
        public void CalculateBaseLineValue(List<double> data);

        public double BaseLineAdjustBreathingValue(double dataPoint);
        public double AdjustBaseLineValue();
    }
}
