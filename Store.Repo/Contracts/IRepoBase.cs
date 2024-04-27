
using System.Linq.Expressions;

namespace Store.Repo.Contracts;

public interface IRepoBase<T>
{
    IQueryable<T> FindAll(bool trackchanges);
    Task<T?> GetByIdAsync(Expression<Func<T, bool>> expression);
    IQueryable<T> FindByConditionAsync(Expression<Func<T,bool>> expression,bool trackchanges);
    Task<bool> AnyAsync(Expression<Func<T, bool>> expression);
    void Create(T entity);
    void Update(T entity);
    void Delete(T entity);
}