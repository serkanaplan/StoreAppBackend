using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Net.Http.Headers;
using Store.Entities.DTOS;
using System.Text;

//apimizin json dışında csv tipinde dönüş yapabilmesi için sadece book nesnesi ve id,title,price alanları için bu sınıfı oluşturduk. kaydetmek için önce extension oluştrup ardından program.cs dosyasına ekleyeceğiz
//test etmek için postmanin header kısmında key değerine accep value değerine text/csv yazıp o şekilde istek at
namespace Store.API.Formatters;

public class CSVOutputFormatter : TextOutputFormatter
{
    public CSVOutputFormatter()
    {
        SupportedMediaTypes.Add(MediaTypeHeaderValue.Parse("text/csv"));
        SupportedEncodings.Add(Encoding.UTF8);
        SupportedEncodings.Add(Encoding.Unicode);
    }

    protected override bool CanWriteType(Type? type)
    {
        if (typeof(BookDTO).IsAssignableFrom(type) ||
            typeof(IEnumerable<BookDTO>).IsAssignableFrom(type))
        {
            return base.CanWriteType(type);
        }
        return false;
    }
    private static void FormatCsv(StringBuilder buffer, BookDTO book)
    {
        buffer.AppendLine($"{book.BookId}, {book.Title}, {book.Price}");
    }

    public override async Task WriteResponseBodyAsync(OutputFormatterWriteContext context,
        Encoding selectedEncoding)
    {
        var response = context.HttpContext.Response;
        var buffer = new StringBuilder();

        if (context.Object is IEnumerable<BookDTO>)
        {
            foreach (var book in (IEnumerable<BookDTO>)context.Object)
            {
                FormatCsv(buffer, book);
            }
        }
        else
        {
            FormatCsv(buffer, (BookDTO)context.Object);
        }
        await response.WriteAsync(buffer.ToString());
    }
}
