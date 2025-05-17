using Hospital_OPD.Model;

namespace Hospital_OPD.Services.Interface
{
    public interface IDepartmentServices
    {
        public Task<Department> CreateDepartment(Department department);
        public Task<List<Department>> GetALLDepartment();
        public Task<Department> UpdateDepartment(int id ,Department department);
        public Task<bool> DeleteDepartment(int id);
    }
}
