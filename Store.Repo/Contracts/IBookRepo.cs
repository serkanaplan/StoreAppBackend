using Store.Entities.Models;
using Store.Entities.RequestFeatures;

namespace Store.Repo.Contracts;

public interface IBookRepo :IRepoBase<Book>
{
       Task<PagedList<Book>> GetAllBookAsync(BookParameters bookParameters,bool trackChanges);
        void CreateOneBook(Book book);
        void UpdateOneBook(Book book);
        void DeleteOneBook(Book book);
        Task<IEnumerable<Book>> GetAllBooksWithDetailsAsync(bool trackChanges); 
}
