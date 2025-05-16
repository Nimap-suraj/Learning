namespace Solid_Priniciple.@abstract
{
    public class TemporaryEmployee : Employee
    {
        public TemporaryEmployee() { }

        public TemporaryEmployee(int id, string name)
        {
            Id = id;
            Name = name;
        }

        public override decimal CalculateBonus(int salary)
        {
            return salary * 0.05M;
        }
    }
}
