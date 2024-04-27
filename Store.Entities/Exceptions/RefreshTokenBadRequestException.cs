namespace Store.Entities.Exceptions;
public class RefreshTokenBadRequestException : BadRequestException
{
    public RefreshTokenBadRequestException() : base("invalid client request. The tokenDTO has some invalid values.")
    {
    }
}