using System.Reflection;
using System.Text;
using Store.Entities.Models;
using System.Linq.Dynamic.Core;

namespace Store.Repo.EFCore.Extensions;
public static class BookRepoExtensions
{
    public static IQueryable<Book> FilterBook(this IQueryable<Book> books, uint minPrice, uint maxPrice)
    => books.Where(b => b.Price >= minPrice && b.Price <= maxPrice);

    public static IQueryable<Book> Search(this IQueryable<Book> books, string? searchTerm)
    {
        if (string.IsNullOrWhiteSpace(searchTerm)) return books;
        var loverCaseTerm = searchTerm.Trim().ToLower();
        return books.Where(b => b.Title.ToLower().Contains(loverCaseTerm));
    }

//bu metod ile querystring parametresi olarak MyOrederBy=Title veya MyOrederBy=Title desc veya MyOrederBy=Price veya MyOrederBy=BookId desc veya MyOrederBy=BookId,Tite desc,Price gibi sıralama işlemleri ekleyebilirsin
    public static IQueryable<Book> Sort(this IQueryable<Book> books, string? orderByQueryString)
    {
        if (string.IsNullOrWhiteSpace(orderByQueryString)) return books.OrderBy(b => b.BookId);

        var orderQuery = OrderQueryBuilder.CreateOrderQuery<Book>(orderByQueryString);
        if (orderQuery is null)
            return books.OrderBy(b => b.BookId);
        //sıralama işlemi için System.Linq.Dynamic.Core paketini kurduk Bu kütüphane ile bir IQueryable üzerinde Dinamik LINQ sorguları (string tabanlı) yazmak mümkündür:
        return books.OrderBy(orderQuery);
    }
}