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
        private double sample;


        public Producer(BlockingCollection<BreathingValuesDataContainer> breathingData)
        {
            _breathingData = breathingData;
        }

        public void Run()
        {
            while (true)
            {
                GetOneBreathingValue();
                //Tilføj tråd   
            }
            
        }


        public void GetOneBreathingValue()
        {

            string log = "logfile.txt";
            

            if (File.Exists(log))
            {
                string[] lines =  File.ReadAllLines(log);

                foreach (string Line in lines)
                {
                    sample = Convert.ToDouble(Line);
                    BreathingValuesDataContainer breathingValuesDataContainer = new BreathingValuesDataContainer{BreathingSample = sample};
                    
                    _breathingData.Add(breathingValuesDataContainer);

                    Thread.Sleep(sampletime);
                }
            }

            

            






            

        }
    }
}
