using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
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

        public Producer(BlockingCollection<BreathingValuesDataContainer> breathingData)
        {
            _breathingData = breathingData;
        }

        public void Run()
        {
            //while (true)
            //{
                GetOneBreathingValue();
                //Tilføj tråd   
            //}
            
        }


        public void GetOneBreathingValue()
        {

            string log = "Respiration.txt";
            

            if (File.Exists(log))
            {
                data =  File.ReadAllText(log);
                dataList = data.Split(",");


            }


            foreach (var VARIABLE in dataList)
            {
                
                double sample = Convert.ToDouble(VARIABLE);
                BreathingValuesDataContainer breathingValuesDataContainer = new BreathingValuesDataContainer{BreathingSample =sample};

                _breathingData.Add(breathingValuesDataContainer);
                Thread.Sleep(sampletime);
            }

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
