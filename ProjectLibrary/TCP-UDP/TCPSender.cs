using System;
using System.Linq.Expressions;
using System.Net.Sockets;

namespace OperatoerLibrary
{
    public class TCPSender : ITCPSender
    {

        private int port = 13001;
        private string IP = "xx,xxx,xxxx,xx";
        private TcpClient client;
        private NetworkStream stream;
        private int layout;


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
            byte[] data = System.Text.Encoding.ASCII.GetBytes(layoutNumber);
            stream.Write(data, 0, data.Length);
        }
    }
}
