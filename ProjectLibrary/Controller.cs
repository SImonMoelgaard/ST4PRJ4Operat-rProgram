using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using OperatoerLibrary.ProducerConsumer;

namespace OperatoerLibrary
{
    public class Controller
    {
        private readonly BlockingCollection<BreathingValuesDataContainer> _breathingData;
        public double BreathingValue { get; set; }
        public List<double> baseLineList = new List<double>();
        private List<DTO_Measurement> measurementdata;
        

        public Controller(BlockingCollection<BreathingValuesDataContainer> breathingData)
        {
            _breathingData = breathingData;
        }

        public void Start()
        {
            BreathingValuesDataContainer breathingBreathingValuesDataContainer = _breathingData.Take();
            BreathingValue = breathingBreathingValuesDataContainer.BreathingSample;



            //Sætter målingerne i list, så vi er klar til baselinevalue
            baseLineList.Add(BreathingValue);
            if (baseLineList.Count >90)
            {
                baseLineList.RemoveAt(0);
            }

        }

        private DateTime time;
        private double i;
        public List<DTO_Measurement> getdata()
        {
            measurementdata = new List<DTO_Measurement>();
            i++;
            time = new DateTime();
            measurementdata.Add(new DTO_Measurement(i, 200, 250, DateTime.Now));
           

            
            return measurementdata;
        }


        


    }

}