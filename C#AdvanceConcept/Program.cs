using System;
using System.Collections.Generic;
internal class Program
{
    private static void Main(string[] args)
    {
        List<Employee> emp = new List<Employee>();
        emp.Add(new Employee() { Id = 1,Name = "suraj shah",Salary = 3000,Experience = 3});
        emp.Add(new Employee() { Id = 3,Name = "om sambhar",Salary = 5000,Experience = 6});
        emp.Add(new Employee() { Id = 2,Name = "Madure makka",Salary = 8000,Experience = 8});

        Employee.PromotedEmployee(emp);
        
    }
}
class Employee
{
    public int Id { get; set; }
    public string Name { get; set; }

    public int Salary { get; set; }

    public int Experience{ get; set; }

    public static void PromotedEmployee(List<Employee> employee)
    {
        foreach (Employee emp in employee)
        {
            if(emp.Experience >= 5)
            {
                Console.WriteLine("Promoted Employee: "+ emp.Name);
            }
        }
    }

}