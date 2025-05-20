namespace Hospital_OPD.Model
{
    public class User
    {
        public int ID{ get; set; }
        public string UserName { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;

        public string Role { get; set; } = "Receptionist";
    }
}
