namespace OperatoerLibrary
{
    public interface IUDPSender
    {
        void OpenSendPorts();

        void SendMeasurementData(DTO_Measurement measurementData);
        public void SendGuiInfo(int guiinfo);
    }
}