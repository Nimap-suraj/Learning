using Hospital_OPD.Model;

namespace Hospital_OPD.Services.Interface
{
    public interface ITokenService
    {
        string GenerateToken(User user);
    }
}
