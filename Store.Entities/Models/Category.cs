namespace Store.Entities.Models;
public class Category
{
    public Category() => Books = new HashSet<Book>();
    public int CategoryId { get; set; }
    public string? CategoryName { get; set; }
    public ICollection<Book> Books { get; set; }
}