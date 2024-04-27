namespace Store.Entities.DTOS;

public record BaseBookDTO
{
    public string? Title { get; init; }
    public string? Author { get; init; }
    public int Price { get; init; }
}
