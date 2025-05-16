using Solid_Priniciple.Interface;

namespace Solid_Priniciple.@abstract
{
    public abstract class Employee : IEmployee
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public Employee() { }

        public Employee(int id, string name)
        {
            Id = id;
            Name = name;
        }

        public abstract decimal CalculateBonus(int salary);

        public override string ToString()
        {
            return $"Id: {Id}, Name: {Name}";
        }

        public decimal GetMinimumSalary()
        {
            return 1000;
        }
        public decimal CalclateBonus(decimal salary)
        {
            return CalculateBonus((int)salary); // or change all to decimal
        }
    }
}
