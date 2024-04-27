
namespace Store.Entities.Exceptions;

public sealed class BookNotFoundException : NotFoundException
{
    public BookNotFoundException(int id) : base($"Book with id: {id} was not found")
    {
    }
}