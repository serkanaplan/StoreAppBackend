namespace Store.Entities.DTOS;
public record TokenDTO
{
    public string AccessToken { get; init; }
    public string RefreshToken { get; init; }
}