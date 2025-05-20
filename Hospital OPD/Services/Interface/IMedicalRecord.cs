using Hospital_OPD.Model;

namespace Hospital_OPD.Services.Interface
{
    public interface IMedicalRecord
    {
        Task<string> AddMedicalHistory(MedicalRecord record);
        Task<List<MedicalRecord>> GetMedicalHistory(int patientId);

    }
}
