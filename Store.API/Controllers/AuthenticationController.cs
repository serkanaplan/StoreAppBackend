using System.Linq.Dynamic.Core.Tokenizer;
using Microsoft.AspNetCore.Mvc;
using Store.Entities.DTOS;
using Store.Service.Contracts;

namespace Store.API.Controllers;
[ApiController]
// [Route("api/authentication")]
[Route("api/[controller]")]
public class AuthenticationController(IServiceManager manager) : ControllerBase
{
    private readonly IServiceManager _manager = manager;

    [HttpPost]
    [ServiceFilter(typeof(ValidationFilterAttribute))]
    public async Task<IActionResult> RegisterUser([FromBody] UserForRegistrationDTO userForRegisterDto)
    {
        var result = await _manager.AuthenticationService.RegisterUser(userForRegisterDto);
        if (!result.Succeeded)
        {
            foreach (var user in result.Errors)
                ModelState.TryAddModelError(user.Code, user.Description);

            return BadRequest(ModelState);
        }

        return StatusCode(StatusCodes.Status201Created);
    }

    [HttpPost("login")]
    // [HttpPost]
    [ServiceFilter(typeof(ValidationFilterAttribute))]
    public async Task<IActionResult> Authenticate([FromBody] UserForAuthenticationDTO user)
    {
        if (!await _manager.AuthenticationService.ValidateUser(user))
            return Unauthorized(); // 401

        var tokenDto = await _manager
            .AuthenticationService
            .CreateToken(true);

        return Ok(tokenDto);
    }

      [HttpPost("refresh")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> Refresh([FromBody] TokenDTO tokenDto)
        {
            var tokenDtoToReturn = await _manager
                .AuthenticationService
                .RefreshToken(tokenDto);
            return Ok(tokenDtoToReturn);
        }

}