namespace AuthenticationHospital.Models
{
    public class User
    {
        public int ID { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}
