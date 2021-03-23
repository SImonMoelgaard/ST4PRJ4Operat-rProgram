namespace OperatoerLibrary
{
    public interface ITCPSender
    {
        string OpenConnection();
        void CloseConnection();
        void SendTreatmentData(string layoutNumber);
        


    }
}
