using Hospital_OPD.Model;

namespace Hospital_OPD.Services.Interface
{
    public interface IPatientServices
    {
        public Task<Patient> AddPatient(Patient patient);
        public Task<List<Patient>> SearchPatient(string name);
        public Task<List<Patient>> GetALLPatient();

    }
}
