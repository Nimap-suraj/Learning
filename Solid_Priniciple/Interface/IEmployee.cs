namespace Solid_Priniciple.Interface
{
    internal interface IEmployee
    {
        int Id { get; set; }
        string Name { get; set; }
        decimal GetMinimumSalary();
    }
}
