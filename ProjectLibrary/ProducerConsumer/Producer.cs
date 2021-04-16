using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;

namespace OperatoerLibrary.ProducerConsumer
{
    public class Producer : IProducer
    {
        private readonly BlockingCollection<BreathingValuesDataContainer> _breathingData;
        private const int sampletime = 33;
        private string dataRead;
        private string data;
        private string[] dataList;
        public List<double> BreathingSamples = new List<double>();
        private BreathingValuesDataContainer datacontainer;

        public Producer(BlockingCollection<BreathingValuesDataContainer> breathingData)
        {
            _breathingData = breathingData;
        }

        public void Run()
        {
            
                GetOneBreathingValue();
                //Tilføj tråd   
            
            
        }


        public void GetOneBreathingValue()
        {

            string log = "DIBH.txt";
            

            if (File.Exists(log))
            {
                data =  File.ReadAllText(log);
                dataList = data.Split(",");


            }


            foreach (var VARIABLE in dataList)
            {
                string data = VARIABLE.Replace("0\r\n", string.Empty);
               if (data.Length >= 7)
               {
                   var sample = decimal.Parse(data, CultureInfo.InvariantCulture);
                   
                   BreathingSamples.Add(Convert.ToDouble(sample));
                   datacontainer = new BreathingValuesDataContainer
                       {BreathingSample = BreathingSamples};
               }
               



            }
            _breathingData.Add(datacontainer);

            
            //for (int i = 0; i < data.Length; i++)
            //{

            //    string samplestring = data[i];
            //    double sample = Convert.ToDouble(samplestring);
            //    BreathingValuesDataContainer breathingValuesDataContainer = new BreathingValuesDataContainer{BreathingSample = Convert.ToDouble(sample)};

            //    _breathingData.Add(breathingValuesDataContainer);
            //    Thread.Sleep(sampletime);

            //}

        }

        
    }
}
