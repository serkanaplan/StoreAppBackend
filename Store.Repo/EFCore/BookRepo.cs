using Microsoft.EntityFrameworkCore;
using Store.Entities.Models;
using Store.Entities.RequestFeatures;
using Store.Repo.Contracts;
using Store.Repo.EFCore.Extensions;

namespace Store.Repo.EFCore;

//bu metodların aynısı baserepo  da var ama burda base repodan çekilen sorgulara her entity için farklı özelleiştirmeler yapabiliyoruz
public sealed class BookRepo(RepoContext context) : RepoBase<Book>(context), IBookRepo
{
        public void CreateOneBook(Book book) => Create(book);
        public void DeleteOneBook(Book book) => Delete(book);
        public async Task<PagedList<Book>> GetAllBookAsync(BookParameters bookParameters, bool trackChanges)
        {
                var books =await FindAll(trackChanges)
                .FilterBook(bookParameters.MinPrice, bookParameters.MaxPrice)
                .Search(bookParameters.SearchTerm)
                .Sort(bookParameters.MyOrderBy)
                .ToListAsync();

                return PagedList<Book>.ToPagedList(books, bookParameters.PageNumber, bookParameters.PageSize);
        }
    public void UpdateOneBook(Book book) => Update(book);

    public async Task<IEnumerable<Book>> GetAllBooksWithDetailsAsync(bool trackChanges)
    {
         return await _context
                .Books
                .OrderBy(b => b.BookId)
                .Include(b => b.Category)
                .ToListAsync();
    }

}
