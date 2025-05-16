namespace AuthWebApi.Models
{
    public class Admin
    {
        public string UserName { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;

        public string Role { get; set; }  = string.Empty ;
    }
}
}
