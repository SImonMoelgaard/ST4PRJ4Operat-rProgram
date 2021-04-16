using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using OperatoerLibrary.Filters;
using OperatoerLibrary.ProducerConsumer;

namespace OperatoerLibrary
{
    public class Controller
    {
        
        private IProducer producer;
        private IBaseLineFilter baseLineFilter = new BaselineFilter();
        private IUDPSender udpSender = new UDPSender();
        private readonly BlockingCollection<BreathingValuesDataContainer> _breathingData;
       

        /// <summary>
        /// CTOR, Recieves datacontainer. Will maybe be deleted
        /// </summary>
        /// <param name="datacontainer"></param>
        public Controller(BlockingCollection<BreathingValuesDataContainer> datacontainer)
        {
            _breathingData = datacontainer; 
          producer = new Producer(_breathingData);
        }

        public void OpenPorts()
        {
            udpSender.OpenSendPorts();
        }
        /// <summary>
        /// Adjust the baseline value
        /// </summary>
        public double AdjustBaseLine()
        {
             return baseLineFilter.AdjustBaseLineValue();
        }


        /// <summary>
        /// Loads datafile for the testapplication
        /// </summary>
        public void loaddata()
        {
             producer.GetOneBreathingValue();
        }

        /// <summary>
        /// Adjust each datapoint 
        /// </summary>
        /// <param name="dataPoint"></param>
        /// <returns></returns>
        public double AdjustBaseLine(double dataPoint)
        {
            return baseLineFilter.BaseLineAdjustBreathingValue(dataPoint);
            
        }

        /// <summary>
        /// Sends each measurement to Patientapplication
        /// </summary>
        /// <param name="dataPoint"></param>
        public void SendMeasurement(DTO_Measurement dataPoint)
        {
            udpSender.SendMeasurementData(dataPoint);
        }

        public void SendGUIInfo(int guiID)
        {
            udpSender.SendGuiInfo(guiID);
        }

       


        


    }

}