using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using WebApiProjectWithDto.Data;
using WebProject.Data;
using WebProject.Dto;

namespace WebProject.Services
{
    public class CategoryService : ICategoryInterface
    {
        private readonly AppDbContext _context;
        public CategoryService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<CategoryDto> CreateCategory(CategoryDto category)
        {
            var item = new Category
            {
                Name = category.Name,
            };
            _context.Add(item);
            await _context.SaveChangesAsync();
            return new CategoryDto { Name = category.Name, };
            throw new NotImplementedException();
        }


        public async Task<List<Category>> GetCategories()
        {
            var category = await _context.categories.ToListAsync();
            return category.Select(u => new Category { Id = u.Id, Name = u.Name }).ToList();
        }

        public async Task<Category> GetCategoryById(int id)
        {
            var category = await _context.categories.FindAsync(id);
            if (category == null)
            {
                return null;
            }
            return new Category
            {
                Id = category.Id,
                Name = category.Name
            };
        }
    }
}
