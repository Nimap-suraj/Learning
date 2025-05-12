using WebProject.Data;
using WebProject.Dto;

namespace WebProject.Services
{
    public interface ICategoryInterface
    {
        Task<CategoryDto> CreateCategory(CategoryDto category);
        Task<List<Category>> GetCategories();
        Task<Category> GetCategoryById(int id);
    }
}
