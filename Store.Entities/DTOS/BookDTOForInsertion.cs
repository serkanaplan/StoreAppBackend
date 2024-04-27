using System.ComponentModel.DataAnnotations;

namespace Store.Entities.DTOS;

public record BookDTOForInsertion:BaseBookDTO
{
    [Required(ErrorMessage = "categoryId is required")]
    public int CategoryId { get; init; }
}
