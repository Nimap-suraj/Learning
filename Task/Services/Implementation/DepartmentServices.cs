using System;
using Microsoft.EntityFrameworkCore;
using Task.Data;
using Task.Model;
using Task.Services.Interfaces;

namespace Task.Services.Implementation
{
    public class DepartmentServices : IDepartmentServices
    {
        private readonly AppDbContext _context;

        public DepartmentServices(AppDbContext context)
        {
            _context = context;
        }
        public async Task<Department> CreateDepartment(Department department)
        {
          
            await _context.Departments.AddAsync(department);
            await _context.SaveChangesAsync();
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
            await _context.SaveChangesAsync();
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
            if (department == null)
            {
                return null;
            }
            department.Name = UpdateDepartment.Name;
            await _context.SaveChangesAsync();
            return department;

        }
    }
}
