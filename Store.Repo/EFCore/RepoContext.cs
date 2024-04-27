
using System.Reflection;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Store.Entities.Models;
namespace Store.Repo.EFCore;

public class RepoContext(DbContextOptions<RepoContext> options) : IdentityDbContext<User>(options)
{
    public DbSet<Book> Books { get; set; }
    public DbSet<Category> Categories { get; set; } 

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);//ıdentityserverin bize getirdiği default tabloların oluşması için
        // modelBuilder.ApplyConfiguration(new BookConfig());
        // modelBuilder.ApplyConfiguration(new RoleConfig());
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }

}
