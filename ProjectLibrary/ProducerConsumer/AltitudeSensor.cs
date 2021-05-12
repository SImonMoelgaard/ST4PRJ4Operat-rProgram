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
    public class AltitudeSensor : IAltitudeSensor
    {
        private readonly BlockingCollection<BreathingValuesDataContainer> _breathingData;
        private string data = "";
        private string datavalue = "";
        private string[] dataList = new string[1];
        public double BreathingSamples = 0;
        private BreathingValuesDataContainer dataContainer;

        /// <summary>
        /// Recieves the DataContainer from controller
        /// </summary>
        /// <param name="breathingData"></param>
        public AltitudeSensor(BlockingCollection<BreathingValuesDataContainer> breathingData)
        {
            _breathingData = breathingData;
        }

        /// <summary>
        /// Runs the GetOneBreathingValue Method
        /// </summary>
        public void Run()
        {
            GetOneBreathingValue();
        }

        /// <summary>
        /// Reads the file used for program
        /// </summary>
        public void GetOneBreathingValue()
        {
            string path = "DIBH.txt";
            
            if (File.Exists(path))
            {
                data =  File.ReadAllText(path);
                dataList = data.Split(",");
            }

            foreach (var VARIABLE in dataList)
            {
                datavalue = VARIABLE.Replace("0\r\n", string.Empty);
               if (datavalue.Length >= 7)
               {
                   Thread.Sleep(40);
                   var sample = decimal.Parse(datavalue, CultureInfo.InvariantCulture);

                   BreathingSamples = Convert.ToDouble(sample);
                   dataContainer = new BreathingValuesDataContainer
                       {BreathingSample = BreathingSamples};
                   _breathingData.Add(dataContainer);
               }
            }
            

        }

        
    }
}
