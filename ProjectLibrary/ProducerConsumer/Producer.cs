using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace OperatoerLibrary.ProducerConsumer
{
    public class Producer : IProducer
    {
        private readonly BlockingCollection<BreathingValuesDataContainer> _breathingData;
        private const int sampletime = 33;
       


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
            BreathingValuesDataContainer breathingValuesDataContainer = new BreathingValuesDataContainer();
            _breathingData.Add(breathingValuesDataContainer);





            

        }
    }
}
