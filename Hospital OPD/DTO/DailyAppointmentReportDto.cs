namespace Hospital_OPD.DTO
{
    public class DailyAppointmentReportDto
    {
        //public int Id { get; set; }
        public string DoctorName { get; set; } = string.Empty;
        public string Department { get; set; } = string.Empty;
        public int AppointmentCount { get; set; }
    }
}
