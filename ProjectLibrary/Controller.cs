using System.Collections.Concurrent;
using OperatoerLibrary.ProducerConsumer;

namespace OperatoerLibrary
{
    public class Controller
    {
        private readonly BlockingCollection<BreathingValuesDataContainer> _breathingData;
        public double BreathingValue { get; set; }

        public Controller(BlockingCollection<BreathingValuesDataContainer> breathingData)
        {
            _breathingData = breathingData;
        }

        public void Start()
        {
            BreathingValuesDataContainer breathingBreathingValuesDataContainer = _breathingData.Take();
            BreathingValue = breathingBreathingValuesDataContainer.BreathingSample;

        }
    }

}