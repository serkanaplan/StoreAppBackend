using System.Text.Json;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Store.Entities.DTOS;
using Store.Entities.RequestFeatures;
using Store.Service.Contracts;

namespace Store.API.Controllers;

[ServiceFilter(typeof(LogFilterAttribute))]//tüm kontrollerların loglanmasını istediğimiz için bunu sınıfın üzerine yazdık
[Route("api/[controller]")]
[ApiController]
[ResponseCache(CacheProfileName = "5MinutesDuration")]
public class BookController(IServiceManager manager) : ControllerBase
{
    private readonly IServiceManager _manager = manager;

    [Authorize]//eğer oturum açılmamışsa kitaplara erişemez. bütün metodlara uygulayacaksan sınıfın başına yaz
    [HttpHead]//get metodu ile aynıdır sadece body dönmez sadece head bilgilerini döner oyuzden ayrı bir metod olarak yazmadık get isteğide head isteğide bu metoda gelecek
    [HttpGet]
    [ResponseCache(Duration = 60)]
    public async Task<ActionResult> GetAllBooksAsync([FromQuery] BookParameters bookParameters)
    {
        var (books, metaData) = await _manager.BookService.GetAllBookAsync(bookParameters, false);
        Response.Headers.Append("X-Pagination", JsonSerializer.Serialize(metaData));

        return Ok(books);
    }

    [Authorize]//oturum açmış herkes erişebilir
    [HttpGet("{id}")]
    public async Task<ActionResult> GetBookByIdAsync([FromRoute] int id)
    {
        //global hata yönetimi ile try metodlardaki try catch lerden kurtulduk  ve artık herhangi bir hata durumunda extension olarak tasarlayıp kaydettiğimiz middleware çalışacak.
        var book = await _manager.BookService.GetOneBookByIdAsync(id);
        return Ok(book);
    }

    [Authorize]
    [HttpGet("details")]
    public async Task<IActionResult> GetAllBooksWithDetailsAsync()
    {
        return Ok(await _manager
            .BookService
            .GetAllBooksWithDetailsAsync(false));
    }
    [Authorize(Roles = "Admin,Moderator")]//rolü admin ve moderator olanlar erişebilir
    [ServiceFilter(typeof(ValidationFilterAttribute))]
    [HttpPost]
    public async Task<ActionResult> CreateBookAsync([FromBody] BookDTOForInsertion bookDTO)
    {
        //actionfilter kullandığımız için ve kontrolleri orada yaptığımız için ,her metod da tekrara düşmemek için bu kontrollerri yorum satırına aldık
        // if (!ModelState.IsValid)
        //     return UnprocessableEntity(ModelState);

        // if (bookDTO is null) return BadRequest();
        var book = await _manager.BookService.CreateOneBookAsync(bookDTO);
        return StatusCode(200, book);
    }

    [Authorize(Roles = "Admin,Moderator")]
    [ServiceFilter(typeof(ValidationFilterAttribute))]
    [HttpPut("{id:int}")]
    public async Task<ActionResult> UpdateBookAsync([FromRoute] int id, [FromBody] BookDTOForUpdate bookDTO)
    {
        //kendi oluşturduğumuz validaasyonlar dışında ekstra default gelen validasyonları bastırıp sadece kendi fluent validasyonlarımızın çalışması için bu satırı ekledik
        //ama bundan önce program.cs dosyasında kendi oluşturduğumuz validasyonla beraber hepsini devre dışı bıraktık ondan sonra burda sadece kendi validasyonumuzu aktif ettik
        //modelstat e servis katmanından erişemediğimiz için kotrolü burda yaptık.

        //actionfilter kullandık oyuzden yorum satırına aldık
        // if (!ModelState.IsValid)
        //     return UnprocessableEntity(ModelState);

        await _manager.BookService.UpdateOneBookAsync(id, bookDTO);
        return Ok(bookDTO);
    }
    [Authorize(Roles = "Admin")]//silme işlemini sadece admin yapabilir
    [HttpDelete("{id:int}")]
    public async Task<ActionResult> DeleteBookAsync([FromRoute] int id)
    {
        await _manager.BookService.DeleteOneBookAsync(id, false);
        return NoContent();
    }

    [Authorize]
    [HttpOptions]
    public IActionResult GetBookOptions()
    {
        Response.Headers.Append("Allow", "GET,POST,PUT,DELETE,HEAD,OPTIONS");
        return Ok();
    }
}



