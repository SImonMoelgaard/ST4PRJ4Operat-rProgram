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

        public void OpenSendPorts()
        {
            broadCastIP = IPAddress.Parse("127.0.0.1"); //Computer vi sender til IVP4 adresse på det givne netværk

            socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

            endPointIP = new IPEndPoint(broadCastIP, port);
        }

        public void SendMeasurementData(DTO_Measurement measurementData)
        {
            var json = JsonConvert.SerializeObject(measurementData);
            var sendData = Encoding.ASCII.GetBytes(json);
            
            
                socket.SendTo(sendData, endPointIP);
            
            
            
        }
        public void SendGuiInfo(int guiInfo)
        {
            SingledataEP = new IPEndPoint(broadCastIP, 11000);


            var json = JsonConvert.SerializeObject(guiInfo);
            var sendData = Encoding.ASCII.GetBytes(json);
            
            socket.SendTo(sendData, SingledataEP);
            
            
            
        }



    }
}