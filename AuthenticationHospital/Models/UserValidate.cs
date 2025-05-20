namespace AuthenticationHospital.Models
{
    public class UserValidate
    {
       public static bool Login(string username, string password)
        {
            UsersBL userBL = new UsersBL();
            var data = userBL.GetUsers();
            return data.Any(user =>
            user.UserName.Equals(username,StringComparison.OrdinalIgnoreCase)&&
            user.Password == password
            );
        }

    }
}
