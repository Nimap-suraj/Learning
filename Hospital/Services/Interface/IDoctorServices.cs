using Hospital_OPD.Model;

namespace Hospital_OPD.Services.Interface
{
    public interface IDoctorServices
    {


        Task<string> MarkDoctorLeave(int doctorId, bool isOnLeave);

        Task<Doctor> AddDoctor(Doctor doctor);
         Task<List<Doctor>> GetAllDoctor();
         Task<Doctor> GetDoctor(int id);
         Task<Doctor> UpdateDoctor(int id,Doctor doctor);
         Task<bool> DeleteDoctor(int id);
        Task<Dictionary<string, string>> GetDoctorScheduleWithNames(int doctorId, DateTime date);
        
    }
}
