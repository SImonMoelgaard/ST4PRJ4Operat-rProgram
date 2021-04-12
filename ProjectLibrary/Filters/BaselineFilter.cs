using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OperatoerLibrary.Filters
{
    public class BaselineFilter
    {
        private List<double> pointstaken = new List<double>();
        private List<double> baselineValues = new List<double>();
        private int goingdown;
        private double lastnumber = 0;
        private double baseLineValue;

        public void GetBaseLineValue(List<DTO_Measurement> measurementlist)
        {
            foreach (var data in measurementlist)
            {
                pointstaken.Add(data.MeasurementData);
                if (data.MeasurementData < lastnumber)
                {
                    goingdown++;

                    if (goingdown >=3)
                    {
                        baselineValues.Add(pointstaken.Min());
                        pointstaken.Clear();
                    }
                }
                else
                {
                    goingdown = 0;
                }

                
                lastnumber = data.MeasurementData;
            }

            CalculateBaseLineValue(baselineValues);


        }

        public void CalculateBaseLineValue(List<double> data)
        {
            
            baseLineValue = data.Sum() / data.Count;
            
        }

        public double BaseLineAdjustBreathingValue(double dataPoint)
        {
            return dataPoint - baseLineValue;
        }




    }
}
