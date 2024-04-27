using Microsoft.AspNetCore.Identity;
using Store.Entities.DTOS;

namespace Store.Service.Contracts;
public interface IAuthenticationService
{
    Task<IdentityResult> RegisterUser(UserForRegistrationDTO userForRegistration);
    Task<bool> ValidateUser(UserForAuthenticationDTO userForAuthDto);
    // Task<string> CreateToken();
    Task<TokenDTO> CreateToken(bool populateExp);
    Task<TokenDTO> RefreshToken(TokenDTO tokenDto);
}