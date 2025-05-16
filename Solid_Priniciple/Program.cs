using System;
using Solid_Priniciple.@abstract;
using Solid_Priniciple.Interface;

internal class Program
{
    private static void Main(string[] args)
    {
        List<Employee> employees = new List<Employee>();
        employees.Add(new PermanentEmployee(1, "John"));
        employees.Add(new TemporaryEmployee(2, "sah"));
        //employees.Add(new ContractEmployee(3, "jseon")); gives error

        foreach (var employee in employees)
        {
            Console.WriteLine($"employee: {employee.ToString()} , Minimum Salry: {employee.GetMinimumSalary().ToString()},Bonus:{employee.CalclateBonus(10000).ToString()}");
        }

        Console.WriteLine();
        List<IEmployee> employees1 = new List<IEmployee>();
        employees1.Add(new PermanentEmployee(8, "kaka"));
        employees1.Add(new TemporaryEmployee(3, "naka"));
        employees1.Add(new ContractEmployee(6, "skak"));

        foreach (var employee in employees1)
        {
            Console.WriteLine($"employee: {employee.ToString()} , Minimum Salry: {employee.GetMinimumSalary().ToString()}");
        }


    }
}
