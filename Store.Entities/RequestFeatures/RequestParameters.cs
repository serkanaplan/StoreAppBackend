namespace Store.Entities.RequestFeatures;

public abstract class RequestParameters
{
    const int maxPageSize = 50;//kayıtlrımız isterse 100 tane olsun biz 50 ilw sınırlandırdık yani kullanıcı sadece 50 kayda ulaşabilir 
    public int PageNumber { get; set; }//auto implemented prop
    private int _pageSize;//full prop
    public int PageSize
    {
        get => _pageSize;
        set => _pageSize = (value > maxPageSize) ? maxPageSize : value;
    }
    public string?  MyOrderBy { get; set; }
    public string? Fields { get; set; }
}
