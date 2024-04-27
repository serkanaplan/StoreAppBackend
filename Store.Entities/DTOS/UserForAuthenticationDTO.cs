using System.ComponentModel.DataAnnotations;

namespace Store.Entities.DTOS;
public record UserForAuthenticationDTO
{
    [Required(ErrorMessage = "Username is required.")]
    public string? UserName { get; init; }

    [Required(ErrorMessage = "Password is required.")]
    public string? Password { get; init; }
}