using System.Text.Json.Serialization;

namespace Hospital_OPD.Model
{
    public class Appointment
    {
        [JsonIgnore]
        public int Id { get; set; }
        public int DoctorId { get; set; }
        public int PatientId { get; set; }

       
        public DateTime AppointmentDate { get; set; }
        

        public TimeOnly AppointmentTime { get; set; }

        [JsonIgnore]
        public Doctor? Doctor { get; set; }
        [JsonIgnore]
        public Patient? Patient { get; set; }
    }

}
