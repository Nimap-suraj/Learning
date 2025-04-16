using Mapster;
using Microsoft.EntityFrameworkCore;
using TaskEcommerce.Context;
using TaskEcommerce.DTO;
using TaskEcommerce.Models;
using TaskEcommerce.Services.Interface;

namespace TaskEcommerce.Services.Implementaion
{
    public class CategoryServices : ICategoryServices
    {
        private readonly DataContext _context;

        public CategoryServices(DataContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<CategoryDto>> GetCategoriesAsync(int CurrentPage, int PageSize)
        {
            int TotalCategory = await _context.categories.CountAsync();
            int TotalPages = (int)Math.Ceiling((decimal)TotalCategory / PageSize);

            var categories = await _context.categories
                .Skip((CurrentPage - 1) * PageSize)
                .Take(PageSize)
                .ToListAsync();

            return categories.Adapt<IEnumerable<CategoryDto>>();
        }

        public async Task<CategoryDto?> GetCategoriesByIdAsync(int Id)
        {
            try
            {
                var category = await _context.categories.FindAsync(Id);

                if (category == null || category.Name == "string")
                    return null;

                return category.Adapt<CategoryDto>();
            }
            catch (Exception)
            {
                throw new Exception("Something went wrong! Please try again later.");
            }
        }

        public async Task<CategoryDto> CreateCategoryAsync(Category category)
        {
            try
            {
                await _context.categories.AddAsync(category);
                await _context.SaveChangesAsync();
                return category.Adapt<CategoryDto>(); // Return adapted CategoryDto
            }
            catch (Exception)
            {
                throw new Exception("Something went wrong while creating the category.");
            }
        }

        public async Task<CategoryDto> UpdateCategoriesAsync(int id, Category updatedCategory)
        {
            try
            {
                var category = await _context.categories.FindAsync(id);

                if (updatedCategory.Name?.ToLower() == "string" || updatedCategory.CategoryNumber?.ToLower() == "string")
                    throw new Exception("Please provide valid values for Name and Category Number.");

                if (category == null)
                    return null;

                category.Name = updatedCategory.Name;
                category.CategoryNumber = updatedCategory.CategoryNumber;

                await _context.SaveChangesAsync();
                return category.Adapt<CategoryDto>();
            }
            catch (Exception)
            {
                throw new Exception("Something went wrong while updating the category.");
            }
        }

        public async Task<bool> SafeDeleteCategory(int Id)
        {
            try
            {
                var category = await _context.categories.FindAsync(Id);
                if (category == null)
                    return false;

                _context.categories.Remove(category);
                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception)
            {
                throw new Exception("Something went wrong while deleting the category.");
            }
        }
    }
}
