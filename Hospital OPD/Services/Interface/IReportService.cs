using Hospital_OPD.DTO;

namespace Hospital_OPD.Services.Interface
{
    public interface IReportService
    {

        Task<List<PatientVisitMonthlyDto>> GetPatientVisitMonthlyAsync(DateTime? startDate, DateTime? endDate);

        Task<List<DailyAppointmentReportDto>> GetDailyAppointmentReport(DateTime date);
    }
}
