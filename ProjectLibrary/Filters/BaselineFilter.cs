using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OperatoerLibrary.Filters
{
    public class BaselineFilter : IBaseLineFilter
    {
        private List<double> baseLineAdjustList = new List<double>();
        private List<double> pointstaken = new List<double>();
        private List<double> baselineValues = new List<double>();
        private int goingdown;
        private double lastnumber = 0;
        private double baseLineValue = 0;

        public double AdjustBaseLineValue()
        {
            foreach (var data in baseLineAdjustList)
            {
                pointstaken.Add(data);
                if (data < lastnumber)
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

                
                lastnumber = data;
            }

            CalculateBaseLineValue(baselineValues);


            return baseLineValue;
        }

        public void CalculateBaseLineValue(List<double> data)
        {
            
            baseLineValue = data.Sum() / data.Count;
            
        }

        public double BaseLineAdjustBreathingValue(double dataPoint)
        {
            AddToBaseLineList(dataPoint);
            return dataPoint - baseLineValue;
        }

        public void AddToBaseLineList(double dataPoint)
        {
            baseLineAdjustList.Add(dataPoint);
            if (baseLineAdjustList.Count >250)
            {
                baseLineAdjustList.RemoveAt(0);
            }
        }




    }
}
