using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using Newtonsoft.Json;

namespace OperatoerLibrary
{
    public class UDPSender : IUDPSender
    {
        private IPAddress broadCastIP;
        private IPEndPoint endPointIP;
        private IPEndPoint SingledataEP;
        private readonly int port = 11000;
        private Socket socket;

        /// <summary>
        /// Creates relevant pots for the UDP connection between this program and patient program 
        /// </summary>
        public void OpenSendPorts()
        {
            broadCastIP = IPAddress.Parse("127.0.0.1"); //Computer vi sender til IVP4 adresse på det givne netværk

            socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

            endPointIP = new IPEndPoint(broadCastIP, port);
        }

        /// <summary>
        /// Sends Measurement data to Patient program
        /// </summary>
        /// <param name="measurementData">
        /// The measurement data for a single point
        /// </param>
        public void SendMeasurementData(DTO_Measurement measurementData)
        {
            var json = JsonConvert.SerializeObject(measurementData);
            var sendData = Encoding.ASCII.GetBytes(json);
            
            
                socket.SendTo(sendData, endPointIP);
            
            
            
        }
        /// <summary>
        /// Sends information about what GUI to use to the patient program
        /// </summary>
        /// <param name="guiInfo">
        /// A number that indicates the correct userinterface theme for Patient program to use
        /// </param>
        public void SendGuiInfo(int guiInfo)
        {
            SingledataEP = new IPEndPoint(broadCastIP, 11000);


            var json = JsonConvert.SerializeObject(guiInfo);
            var sendData = Encoding.ASCII.GetBytes(json);
            
            socket.SendTo(sendData, SingledataEP);
            
            
            
        }



    }
}