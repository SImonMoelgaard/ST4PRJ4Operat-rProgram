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

        public Controller(BlockingCollection<BreathingValuesDataContainer> breathingData)
        {
            _breathingData = breathingData;
        }

        public void Start()
        {
            BreathingValuesDataContainer breathingBreathingValuesDataContainer = _breathingData.Take();
            BreathingValue = breathingBreathingValuesDataContainer.BreathingSample;




            baseLineList.Add(BreathingValue);
            if (baseLineList.Count >90)
            {
                baseLineList.RemoveAt(0);
            }

        }
    }

}