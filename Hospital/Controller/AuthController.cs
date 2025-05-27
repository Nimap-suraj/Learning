using Hospital_OPD.Data;
using Hospital_OPD.DTO;
using Hospital_OPD.Model;
using Hospital_OPD.Services.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly ITokenService _tokenService;

    public AuthController(AppDbContext context, ITokenService tokenService)
    {
        _context = context;
        _tokenService = tokenService;       
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterDto dto)
    {
        var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == dto.Email);
        if (existingUser != null)
            return BadRequest("User with this email already exists.");

        var user = new User
        {
            UserName = dto.Username,
            Email = dto.Email,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
            Role = dto.Role
        };


        if (dto.Role.ToLower() == "doctor")
        {
            if (!dto.DoctorId.HasValue)
                return BadRequest("DoctorId is required for role doctor.");

            var doctor = await _context.Doctor.FindAsync(dto.DoctorId.Value);
            if (doctor == null)
                return BadRequest("Invalid DoctorId. No such doctor exists.");

            var linkedUser = await _context.Users.FirstOrDefaultAsync(u => u.DoctorId == dto.DoctorId.Value);
            if (linkedUser != null)
                return BadRequest("A user is already linked to this doctor.");

            user.DoctorId = dto.DoctorId.Value;
        }
        
        //if (dto.Role.ToLower() == "admin")
        //{

        //    return Ok("Admin can't registerd Contact Hospial");


        //}

        _context.Users.Add(user);
        await _context.SaveChangesAsync();
        return Ok("User registered successfully.");
        
    }


    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginDto dto)
    {
        var user = await _context.Users.FirstOrDefaultAsync(x =>
            x.UserName == dto.Username || x.Email == dto.Email);

        if (user == null || !BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash))
            return Unauthorized("Invalid credentials.");

        var token = _tokenService.GenerateToken(user);
        return Ok(token);
    }

}
