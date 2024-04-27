using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Store.Entities.Models;

namespace Store.Repo.Config;

public class BookConfig : IEntityTypeConfiguration<Book>
{
    public void Configure(EntityTypeBuilder<Book> builder)
    {
        builder.HasData(
            new Book { BookId = 1, CategoryId = 1, Title = "Karagöz ve Hacivat", Price = 75 },
            new Book { BookId = 2, CategoryId = 2, Title = "Mesnevi", Price = 175 },
            new Book { BookId = 3, CategoryId = 1, Title = "Devlet", Price = 375 }
        );
    }
}
