using System;
using System.Net.Sockets;
using System.Text;

namespace OperatoerLibrary
{
    public class TCPSender : ITCPSender
    {
        private TcpClient client;
        private readonly string IP = "xx,xxx,xxxx,xx";
        private int layout;

        private readonly int port = 13001;
        private NetworkStream stream;


        public string OpenConnection()
        {
            try
            {
                client = new TcpClient(IP, port);
                stream = client.GetStream();
                return "forbindelse oprettet";
            }
            catch (SocketException e)
            {
                return "Ingen forbindelse";
            }
            catch (ArgumentNullException e)
            {
                return "Ingen forbindelse";
            }
        }

        public void CloseConnection()
        {
            stream.Close();
            client.Close();
        }

        public void SendTreatmentData(string layoutNumber)
        {
            var data = Encoding.ASCII.GetBytes(layoutNumber);
            stream.Write(data, 0, data.Length);
        }
    }
}