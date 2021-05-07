using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OperatoerLibrary.Filters
{
    public class BaselineFilter : IBaseLineFilter
    {
        public List<double> BaseLineAdjustList { get; set; } = new List<double>();
        private List<double> pointstaken = new List<double>();
        private List<double> baselineValues = new List<double>();
        private int goingdown;
        private double lastnumber = 0;
        private double baseLineValue = 0;

        /// <summary>
        /// Takes the 250 points from the list and sorts them. Each number will be put in a new list. If 3 points in a row is lesser than the point before it will take this list, find the lowest value and put it into baseline values list and clear itself. 
        /// </summary>
        /// <returns>
        /// Last it will return the list to be calculated
        /// </returns>
        public double AdjustBaseLineValue()
        {
            foreach (var data in BaseLineAdjustList)
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

        /// <summary>
        /// Calculates the mean baseline value from all baseline values from each sinus wave in list.
        /// </summary>
        /// <param name="data"></param>
        public void CalculateBaseLineValue(List<double> data)
        {
            
            baseLineValue = data.Sum() / data.Count;
            
        }

        /// <summary>
        /// Takes a datapoint and adjust it with the baseline value
        /// </summary>
        /// <param name="dataPoint">
        /// Recives a single unedited datapoint
        /// </param>
        /// <returns>
        /// Returns the adjusted datapoint to controller
        /// </returns>
        public double BaseLineAdjustBreathingValue(double dataPoint)
        {
            AddToBaseLineList(dataPoint);
            return dataPoint - baseLineValue;
        }

        /// <summary>
        /// Recieves a datapoint, puts it in a list. Removes the last datapoint if the list exceeds 250 points
        /// </summary>
        /// <param name="dataPoint">
        /// Datapoint recieved
        /// </param>
        public void AddToBaseLineList(double dataPoint)
        {
            BaseLineAdjustList.Add(dataPoint);
            if (BaseLineAdjustList.Count >250)
            {
                BaseLineAdjustList.RemoveAt(0);
            }
        }

        public void SaveBaseLine(double baseLine)
        {
            baseLineValue = baseLine;
        }



    }
}
