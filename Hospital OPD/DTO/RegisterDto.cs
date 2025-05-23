namespace Hospital_OPD.DTO
{
    public class RegisterDto
    {
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty ;

        public int? DoctorId { get; set; } // Optional: only required if Role is doctor
        public string Role { get; set; } = string.Empty ;
    }
}
