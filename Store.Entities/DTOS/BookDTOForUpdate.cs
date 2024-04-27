using Store.Entities.DTOS;

namespace Store.Entities.DTOS;

public record BookDTOForUpdate :BaseBookDTO
{
    //inint ile readonly ve immutable olur ve ctorda veya başlatıcıda doldurulmak zorundadır 
    public int BookId { get; init; }
}

//recordları bu şekilde de tanımlayabilirsin. init kullanmana gerek kalmaz, otomatikman readonly ve immutable olur
// public record BookDTOForUpdate(int id ,string Title, string Author, string Category, int Price);