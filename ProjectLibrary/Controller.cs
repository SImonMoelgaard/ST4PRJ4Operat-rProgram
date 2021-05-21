﻿using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using OperatoerLibrary.DTO;
using OperatoerLibrary.Filters;
using OperatoerLibrary.ProducerConsumer;

namespace OperatoerLibrary
{
    public class Controller
    {
        
        private IAltitudeSensor producer;
        private IBaseLineFilter baseLineFilter = new BaselineFilter();
        private IUDPSender udpSender = new UDPSender();
        private IGatingArea gatingArea = new GatingArea();
        private readonly BlockingCollection<BreathingValuesDataContainer> _breathingData;
        private DTO_GatingValues gatingValues;
        private Thread loadDataThread;

        /// <summary>
        /// CTOR, Recieves datacontainer from mainWindow.
        /// </summary>
        /// <param name="datacontainer"></param>
        public Controller(BlockingCollection<BreathingValuesDataContainer> datacontainer)
        {
            _breathingData = datacontainer; 
          producer = new AltitudeSensor(_breathingData);


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
        /// Runs the producer
        /// </summary>
        public void RunProducer()
        {
            loadDataThread = new Thread(producer.Run);
            loadDataThread.Start();
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

        /// <summary>
        /// Saves the gating area and returns the result
        /// </summary>
        /// <param name="lowerGating"></param>
        /// <param name="higherGating"></param>
        /// <returns></returns>
        public string SaveGatingArea(double lowerGating, double higherGating)
        {
            var result = gatingArea.SaveGatingArea(lowerGating, higherGating);
            return result;
        }

        /// <summary>
        /// Gets the gating area and returns them
        /// </summary>
        /// <returns></returns>
        public DTO_GatingValues GetGatingValue()
        {
            gatingValues = gatingArea.GetGatingValue();
            return gatingValues;
        }

        /// <summary>
        /// Saves the manual baseline value
        /// </summary>
        /// <param name="baseline"></param>
        public void SaveBaseLineValue(double baseline)
        {
            baseLineFilter.SaveBaseLine(baseline);
        }
    }
}