using TaskEcommerce.DTO;
using TaskEcommerce.Models;

namespace TaskEcommerce.Services.Interface
{
    public interface ICategoryServices
    {
        Task<IEnumerable<CategoryDto>> GetCategoriesAsync(int CurrentPage, int PageSize);
        Task<CategoryDto?> GetCategoriesByIdAsync(int Id);
        Task<CategoryDto> CreateCategoryAsync(Category category);
        Task<CategoryDto> UpdateCategoriesAsync(int id, Category category);
        Task<bool> SafeDeleteCategory(int Id);
    }
}
