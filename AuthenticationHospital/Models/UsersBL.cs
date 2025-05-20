namespace AuthenticationHospital.Models
{
    public class UsersBL
    {
        public List<User> GetUsers()
        {
            List<User> usersList = new List<User>()
            {
                new User{ ID = 101, UserName = "MaleUser",Password = "123456"},
                new User{  ID = 101,UserName = "FemaleUser",Password = "abcdef"} 
            };
            return usersList;

        }
    }
}
