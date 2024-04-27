using Entities.Exceptions;
using Store.Entities.Models;
using Store.Repo.Contracts;
using Store.Service.Contracts;

namespace Store.Service.Concretes;
public class CategoryService(IRepoManager manager) : ICategoryService
{
   private readonly IRepoManager _manager = manager;

    public async Task<IEnumerable<Category>> GetAllCategoriesAsync(bool trackChanges)
        {
            return await _manager
                .Category
                .GetAllCategoriesAsync(trackChanges);
        }

        public async Task<Category> GetOneCategoryByIdAsync(int id)
        {

            var category = await _manager
                .Category
                .GetByIdAsync(x => x.CategoryId == id);

            if (category is null)
                throw new CategoryNotFoundException(id);
            return category;
        }
}