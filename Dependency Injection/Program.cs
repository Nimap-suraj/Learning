using Dependency_Injection.Models;

internal class Program
{
    private static void Main(string[] args)
    {
        Home home = new Home();
        // Constructor Injection
        Person person = new Person(home);
        person.TakeRefuge(); // Home method
       // person.School = new School();// pehele clases swap nahi hota tha ab hoga niche dekho
        person.School = new College();// new update code

        person.Study();     // school method
        person.GetTreatment(new Hospital());  // Hospital 

        // dependency injection  classes se bhi ho sakta hain.  
    }
}