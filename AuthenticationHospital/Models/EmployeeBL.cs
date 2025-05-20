namespace AuthenticationHospital.Models
{
    public class EmployeeBL
    {
        public List<Employee> GetEmployee()
        {
            List<Employee> employeeList = new List<Employee>();
            for(int i = 0; i < 10; i++)
            {
                if( i > 5)
                {
                    employeeList.Add(new Employee()
                    {
                        Id = i,
                        Name = "DEV" + i,
                        Departments = "IT",
                        Salary = 10000 + i,
                        Gender = "MALE"
                    });
                }
                else
                {
                    employeeList.Add(new Employee()
                    {
                        Id = i,
                        Name = "Name" + i,
                        Departments = "HR",
                        Salary = 1000 + i,
                        Gender = "FEMALE"
                    });
                }
            }
            return employeeList;
        }
    }
}
