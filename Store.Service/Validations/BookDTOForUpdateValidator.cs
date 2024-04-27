using FluentValidation;
using Store.Entities.DTOS;

namespace Store.Service.Validations;

public class BookDTOForUpdateValidator :AbstractValidator<BookDTOForUpdate>
{
    public BookDTOForUpdateValidator()
    {
        RuleFor(x => x.BookId)
            .NotEmpty()
            .NotNull();
        RuleFor(x => x.Title)
            .NotEmpty()
            .NotNull()
            .MinimumLength(2)
            .MaximumLength(50)
            .WithMessage("Title alanı boş geçilemez");
        RuleFor(x => x.Author)
            .NotEmpty()
            .NotNull()
            .WithMessage("Author alanı boş geçilemez");
        RuleFor(x => x.Price)
            .Must(x => x >= 10 && x <= 100)
            .WithMessage("Price alanı 10 ile 100 arasında olmalıdır.");
    }
}
