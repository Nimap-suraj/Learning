namespace AuthenticationHospital.Models
{
    public class Employee
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Gender { get; set; } = string.Empty;
        public string Departments { get; set; } = string.Empty;
        public int Salary { get; set; }
    }
}
