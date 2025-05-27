using Azure.Core;
using Hospital_OPD.Data;
using Hospital_OPD.Model;
using Hospital_OPD.Services.Interface;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Hospital_OPD.Services.Implementation
{
    public class DepartmentServices : IDepartmentServices
    {
        private readonly AppDbContext _context;
        private readonly ILogger<AppointmentService> _logger;

        public DepartmentServices(AppDbContext context, ILogger<AppointmentService> logger)
        {
            _context = context;
            _logger = logger;
        }
        public async Task<Department> CreateDepartment(Department department)
        {
            try
            {
                await _context.Departments.AddAsync(department);
                _logger.LogInformation("Department '{DepartmentName}' added successfully at {Time}", department.Name, DateTime.UtcNow);

                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to create department '{DepartmentName}' at {Time}", department.Name, DateTime.UtcNow);
                throw; // This will be handled by global exception middleware
            }

            return department;
        }

        public async Task<bool> DeleteDepartment(int id)
        {
            var department = await _context.Departments.FindAsync(id);
            if (department == null)
            {
                return false;
            }
           
            _context.Departments.Remove(department);
           await  _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<Department>> GetALLDepartment()
        {
            var departments = await _context.Departments.ToListAsync();
            if (departments == null)
            {
                return null;
            }
            return departments;

        }

        public async Task<Department> UpdateDepartment(int id, Department UpdateDepartment)
        {
           var department = await _context.Departments.FindAsync(id);
            if(department == null)
            {
                return null;
            }
            department.Name =UpdateDepartment.Name;
             await _context.SaveChangesAsync();
            return department;
           
        }
    }
}
