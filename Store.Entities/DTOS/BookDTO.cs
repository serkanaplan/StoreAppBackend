
namespace Store.Entities.DTOS;

public record BookDTO :BaseBookDTO
{
    public int BookId { get; init; }
}