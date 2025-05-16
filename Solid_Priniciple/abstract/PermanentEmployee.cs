namespace Solid_Priniciple.@abstract
{
    public class PermanentEmployee : Employee
    {
        public PermanentEmployee() { }

        public PermanentEmployee(int id, string name)
        {
            Id = id;
            Name = name;
        }

        public override decimal CalculateBonus(int salary)
        {
            return salary * 0.1M;
        }
    }
}
