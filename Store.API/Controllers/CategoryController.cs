using Microsoft.AspNetCore.Mvc;
using Store.Service.Contracts;

namespace Store.API.Controllers;
[ApiController]
[Route("api/[controller]")]
public class CategoryController(IServiceManager services) : ControllerBase
{
    private readonly IServiceManager _services = services;

    [HttpGet]
    public async Task<IActionResult> GetAllCategoriesAsync()
    {
        return Ok(await _services
            .CategoryService
            .GetAllCategoriesAsync(false));
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetAllCategoriesAsync([FromRoute] int id)
    {
        return Ok(await _services
            .CategoryService
            .GetOneCategoryByIdAsync(id));
    }
}