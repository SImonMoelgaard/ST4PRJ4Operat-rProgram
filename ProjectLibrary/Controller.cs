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
        /// CTOR, Recieves datacontainer from mainWindow.
        /// </summary>
        /// <param name="datacontainer"></param>
        public Controller(BlockingCollection<BreathingValuesDataContainer> datacontainer)
        {
            _breathingData = datacontainer; 
          producer = new Producer(_breathingData);
        }

        /// <summary>
        /// Calls a method in UDPSender to open ports
        /// </summary>
        public void OpenPorts()
        {
            udpSender.OpenSendPorts();
        }

        /// <summary>
        /// Adjusts the baseline value and returns them
        /// </summary>
        public double AdjustBaseLine()
        {
             return baseLineFilter.AdjustBaseLineValue();
        }


        /// <summary>
        /// Loads datafile for the application
        /// </summary>
        public void loaddata()
        {
             producer.GetOneBreathingValue();
        }

        /// <summary>
        /// Adjusts each datapoint and returns it.
        /// </summary>
        /// <param name="dataPoint"></param>
        /// <returns></returns>
        public double AdjustBaseLine(double dataPoint)
        {
            return baseLineFilter.BaseLineAdjustBreathingValue(dataPoint);
            
        }

        /// <summary>
        /// Sends each measurement to patient application
        /// </summary>
        /// <param name="dataPoint"></param>
        public void SendMeasurement(DTO_Measurement dataPoint)
        {
            udpSender.SendMeasurementData(dataPoint);
        }

        /// <summary>
        /// Sends User Interface ID to patient application.
        /// </summary>
        /// <param name="guiID"></param>
        public void SendGUIInfo(int guiID)
        {
            udpSender.SendGuiInfo(guiID);
        }
    }
}