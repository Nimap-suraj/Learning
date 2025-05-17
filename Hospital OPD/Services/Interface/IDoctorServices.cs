using Hospital_OPD.Model;

namespace Hospital_OPD.Services.Interface
{
    public interface IDoctorServices
    {
         Task<Doctor> AddDoctor(Doctor doctor);
         Task<List<Doctor>> GetAllDoctor();
         Task<Doctor> GetDoctor(int id);
         Task<bool> UpdateDoctor(int id,Doctor doctor);
         Task<bool> DeleteDoctor(int id);
    }
}
