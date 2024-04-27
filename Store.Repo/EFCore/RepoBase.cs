using Microsoft.EntityFrameworkCore;
using Store.Repo.Contracts;
using System.Linq.Expressions;

namespace Store.Repo.EFCore;

public abstract class RepoBase<T> : IRepoBase<T> where T : class
{
    protected readonly RepoContext _context;

    public RepoBase(RepoContext context) => _context = context;

    public void Create(T entity) => _context.Set<T>().Add(entity);

    public void Update(T entity) => _context.Set<T>().Update(entity);

    public void Delete(T entity) => _context.Set<T>().Remove(entity);

    public IQueryable<T> FindAll(bool trackchanges) => trackchanges ? _context.Set<T>() : _context.Set<T>().AsNoTracking();

    public IQueryable<T> FindByConditionAsync(Expression<Func<T, bool>> expression, bool trackchanges)
    =>trackchanges ? _context.Set<T>().Where(expression) : _context.Set<T>().Where(expression).AsNoTracking();

    public async Task<T?> GetByIdAsync(Expression<Func<T, bool>> expression) =>await _context.Set<T>().AsNoTracking().FirstOrDefaultAsync(expression);//find kullanmadık çünkü find otomatikman takip ediyor ve asnotracking yapamıyoruz. ama firstordefault takip etmiyor

    public async Task<bool> AnyAsync(Expression<Func<T, bool>> expression)
    =>await _context.Set<T>().AnyAsync(expression);
}