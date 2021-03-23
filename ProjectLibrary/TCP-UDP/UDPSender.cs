using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using Newtonsoft.Json;

namespace OperatoerLibrary
{
    public class UDPSender : IUDPSender
    {
        private int port = 11000;
        private IPAddress broadCastIP;
        private Socket socket;
        private IPEndPoint endPointIP;

        public void OpenSendPorts()
        {
            broadCastIP = IPAddress.Parse("xxx.xxx.xxx"); //Computer vi sender til IVP4 adresse på det givne netværk

            socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

            endPointIP = new IPEndPoint(broadCastIP, port);
        }

        public void SendMeasurementData(DTO_Measurement measurementData)
        {
            
            var json = JsonConvert.SerializeObject(measurementData);
            byte[] sendData = Encoding.ASCII.GetBytes(json);

            socket.SendTo(sendData, endPointIP);

        }





    }
}
