namespace Store.Repo.Contracts;

public interface IRepoManager
{
    IBookRepo Book { get;}
    ICategoryRepo Category { get;}
    Task SaveAsync();
}
