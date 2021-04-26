using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using OperatoerLibrary.DTO;
using OperatoerLibrary.Filters;
using OperatoerLibrary.ProducerConsumer;

namespace OperatoerLibrary
{
    public class Controller
    {
        
        private IProducer producer;
        private IBaseLineFilter baseLineFilter = new BaselineFilter();
        private IUDPSender udpSender = new UDPSender();
        private IGatingArea gatingArea = new GatingArea();
        private readonly BlockingCollection<BreathingValuesDataContainer> _breathingData;
        private DTO_GatingValues gatingValues;

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
        public void LoadData()
        {
             producer.Run();
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

        public string SaveGatingArea(double lowerGating, double higherGating)
        {
            var result = gatingArea.SaveGatingArea(lowerGating, higherGating);
            return result;
        }

        public DTO_GatingValues GetGatingValue()
        {
            gatingValues = gatingArea.GetGatingValue();
            return gatingValues;
        }
    }
}