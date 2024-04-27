namespace Store.Entities.Models;

public class Book
{
    public int BookId { get; set; }
    public string? Title { get; set; }
    public string? Author { get; set; }
    public int CategoryId { get; set; }
    public Category? Category { get; set; }
    public int Price { get; set; }
}
