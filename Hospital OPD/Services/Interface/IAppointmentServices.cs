using Hospital_OPD.Model;

namespace Hospital_OPD.Services.Interface
{
    public interface IAppointmentServices
    {
        public Task<string> BookAppointment(Appointment appointment);
        public Task<string> CancelAppointment(int patientId,int doctorID);
    }
}
