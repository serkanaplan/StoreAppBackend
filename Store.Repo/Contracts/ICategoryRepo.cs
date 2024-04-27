using Store.Entities.Models;

namespace Store.Repo.Contracts;

public interface ICategoryRepo :IRepoBase<Category>
{
        Task<IEnumerable<Category>> GetAllCategoriesAsync(bool trackChanges);
        void CreateOneCategory(Category category);
        void UpdateOneCategory(Category category);
        void DeleteOneCategory(Category category);

}