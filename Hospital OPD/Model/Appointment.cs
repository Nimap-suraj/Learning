namespace Hospital_OPD.Model
{
    public class Appointment
    {
        public int Id { get; set; }
        public int DoctorId { get; set; }
        public int PatientId { get; set; }

        public DateTime AppointmentDate { get; set; }
        public TimeOnly AppointmentTime { get; set; }

        public Doctor? Doctor { get; set; }
        public Patient? Patient { get; set; }
    }

}
