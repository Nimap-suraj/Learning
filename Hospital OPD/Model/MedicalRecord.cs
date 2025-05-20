using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Hospital_OPD.Model
{
    public class MedicalRecord
    {
        public int ID { get; set; }

        [ForeignKey("Patient")]
        public int PatientID { get; set; }

        [JsonIgnore]
        public Patient? Patient { get; set; }    

        [ForeignKey("Doctor")]
        public int DoctorId { get; set; }
        [JsonIgnore]
        public Doctor? Doctor { get; set; }

        public DateTime VisitDate { get; set; }

        public string VisitNotes { get; set; } = string.Empty;
        public string Prescription { get; set; } = string.Empty;
        public string FollowUpInstructions { get; set; } = string.Empty;
    }
}
