using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using OperatoerLibrary.ProducerConsumer;

namespace OperatoerLibrary
{
    public class Controller
    {
        
        public double BreathingValue { get; set; }
        public List<double> baseLineList = new List<double>();
        private List<DTO_Measurement> measurementdata;
        private IProducer producer;
        private readonly BlockingCollection<BreathingValuesDataContainer> _breathingData;

        public Controller(BlockingCollection<BreathingValuesDataContainer> datacontainer)
        {
            _breathingData = datacontainer; 
          producer = new Producer(_breathingData);
        }

        public void Start()
        {
            //BreathingValuesDataContainer breathingBreathingValuesDataContainer = _breathingData.Take();
            //BreathingValue = breathingBreathingValuesDataContainer.BreathingSample;



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
            measurementdata.Add(new DTO_Measurement(220, 200, 250, DateTime.Now.ToLocalTime()));
           

            
            return measurementdata;
        }

        public void loaddata()
        {
             producer.GetOneBreathingValue();

        }
        public double ReadFile(string test)
        {
            double dataraw = 0;
            //dataraw = producer.GetOneBreathingValue();

           
            return dataraw;
        }


        


    }

}