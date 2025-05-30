using Task.Model;

namespace Task.Services.Interfaces
{
    public interface IDepartmentServices
    {
        public Task<Department> CreateDepartment(Department department);
        public Task<List<Department>> GetALLDepartment();
        public Task<Department> UpdateDepartment(int id, Department department);
        public Task<bool> DeleteDepartment(int id);
    }
}
