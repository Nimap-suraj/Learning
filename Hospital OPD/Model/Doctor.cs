using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Hospital_OPD.Model
{
    public class Doctor
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        //user uid
        // Link to Department
        [ForeignKey("Department")]
        public int DepartmentId { get; set; }
        [JsonIgnore]
        public Department? Department { get; set; }  // Add [JsonIgnore] using System.Text.Json.Serialization

        [Required]
        public string Specialization { get; set; } = string.Empty;
        public bool IsOnLeave { get; set; }
        public string MorningSlotStart { get; set; } = string.Empty;
        public string MorningSlotEnd { get; set; } = string.Empty;

        public string EveningSlotStart { get; set; } = string.Empty;
        public string EveningSlotEnd { get; set; } = string.Empty;
    }
}
