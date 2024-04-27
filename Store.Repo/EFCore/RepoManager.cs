//manager sınıfı =unitofwork
using Store.Repo.Contracts;

namespace Store.Repo.EFCore;

public class RepoManager(RepoContext context, IBookRepo bookrepo, ICategoryRepo categoryrepo) : IRepoManager
{
    private readonly RepoContext _context = context;
    //istersek sadece save metodu nu kullanabiliriz çünkü varlıklara _context üzerinden zaten erişebiliyoruz manager üzerinden erişmek zorunda değiliz
    private readonly IBookRepo _bookrepo = bookrepo;
    private readonly ICategoryRepo _categoryrepo = categoryrepo;


    // public IBookRepo Book => new BookRepo(_context)
    public IBookRepo Book => _bookrepo;//nesne sadece kullanıldığı anda newlenecek lazy bu işe yarıyor. önceki hali yukarıda
    public ICategoryRepo Category => _categoryrepo;
    public async Task SaveAsync() =>await _context.SaveChangesAsync();
}
