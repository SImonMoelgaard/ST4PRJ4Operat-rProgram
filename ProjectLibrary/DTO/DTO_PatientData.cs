namespace OperatoerLibrary.DTO
{
    public class DTO_PatientData
    {
        public DTO_PatientData(int id, double time, string name, string typeOfTreatment)
        {
            ID = id;
            Time = time;
            Name = name;
            TypeOfTreatment = typeOfTreatment;
        }

        public int ID { get; set; }
        public double Time { get; set; }
        public string Name { get; set; }
        public string TypeOfTreatment { get; set; }
    }
}