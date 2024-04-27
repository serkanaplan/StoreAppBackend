using Store.Entities.Models;

namespace Store.Service.Contracts;
public interface ICategoryService
{
    Task<IEnumerable<Category>> GetAllCategoriesAsync(bool trackChanges);
    Task<Category> GetOneCategoryByIdAsync(int id);
}