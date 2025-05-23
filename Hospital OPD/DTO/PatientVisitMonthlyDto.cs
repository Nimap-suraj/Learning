namespace Hospital_OPD.DTO
{
    public class PatientVisitMonthlyDto
    {
        public int PatientId { get; set; }
        public string? PatientName { get; set; }

        public string? VisitMonth { get; set; }
        public int? VisitCount { get; set; }
        public string? VisitDates {  get; set; }
    }
}
