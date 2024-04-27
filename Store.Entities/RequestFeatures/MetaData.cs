namespace Store.Entities.RequestFeatures;

public class MetaData
{
    public int CurrentPage { get; set; }//geçerli sayfa
    public int TotalPage { get; set; }//toplam sayfa sayısı
    public int PageSize { get; set; }//sayfada listelenen kayıt sayısı
    public int TotalCount { get; set; }//toplam kayıt sayısı

    public bool HasPrevious => CurrentPage > 1;
    public bool HasNext => CurrentPage < TotalPage; 
}
