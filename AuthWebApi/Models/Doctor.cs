namespace AuthWebApi.Models
{
    public class Doctor
    {
        public string Role { get; set; } = string.Empty;

        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Department { get; set; } = string.Empty;
        public string Specialization { get; set; } = string.Empty;
        public  bool IsOnLeave { get; set; }
        public  bool IsAvailable { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}
