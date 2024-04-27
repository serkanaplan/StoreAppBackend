namespace Store.Entities.RequestFeatures;

public class PagedList<T> : List<T>
{
    public MetaData MetaData { get; set; }
    public PagedList(List<T> items, int count, int pageNumber, int pageSize)
    {
        MetaData = new MetaData
        {
            TotalCount = count,
            PageSize = pageSize,
            CurrentPage = pageNumber,
            TotalPage = (int)Math.Ceiling(count / (double)pageSize)//Math.Ceiling ile sayıyı tam sayıya yuvarlıyoruz
        };

        AddRange(items);
    }

    public static PagedList<T> ToPagedList(IEnumerable<T> source,int pageNumber,int pageSize)
    {
        var count =source.Count();
        var items= source.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();

        return new PagedList<T>(items, count, pageNumber, pageSize);
    }
}
