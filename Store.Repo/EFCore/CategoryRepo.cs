using Microsoft.EntityFrameworkCore;
using Store.Entities.Models;
using Store.Repo.Contracts;

namespace Store.Repo.EFCore;

public sealed class CategoryRepo(RepoContext context) : RepoBase<Category>(context), ICategoryRepo
{
     public void CreateOneCategory(Category category) => Create(category);

        public void DeleteOneCategory(Category category)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Category>> GetAllCategoriesAsync(bool trackChanges) =>  
             await FindAll(trackChanges)
                .OrderBy(c => c.CategoryName)
                .ToListAsync();

        public void UpdateOneCategory(Category category)
        {
            throw new NotImplementedException();
        }
}