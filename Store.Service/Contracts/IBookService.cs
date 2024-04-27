using System.Dynamic;
using System.Linq.Expressions;
using Store.Entities.DTOS;
using Store.Entities.Models;
using Store.Entities.RequestFeatures;

namespace Store.Service.Contracts;

public interface IBookService 
{
        // Task<IEnumerable<BookDTO>> GetAllBookAsync(BookParameters bookParameters,bool trackChanges);
        // Task<(IEnumerable<BookDTO> books, MetaData metaData)> GetAllBookAsync(BookParameters bookParameters,bool trackChanges);
        Task<(IEnumerable<ExpandoObject> books, MetaData metaData)> GetAllBookAsync(BookParameters bookParameters,bool trackChanges);
        Task<BookDTO?> GetOneBookByIdAsync(int id);
        Task<BookDTO> CreateOneBookAsync(BookDTOForInsertion book);
        Task UpdateOneBookAsync(int id, BookDTOForUpdate bookDTO);
        Task DeleteOneBookAsync(int id, bool trackChanges);
        Task<bool> AnyAsync(Expression<Func<Book, bool>> expression, bool trackChanges);
        IEnumerable<Book> WhereAsync(Expression<Func<Book, bool>> expression, bool trackChanges);
        Task<IEnumerable<Book>> GetAllBooksWithDetailsAsync(bool trackChanges);
}