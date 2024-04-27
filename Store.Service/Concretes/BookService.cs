
using System.Dynamic;
using System.Linq.Expressions;
using AutoMapper;
using Store.Entities.DTOS;
using Store.Entities.Exceptions;
using Store.Entities.Models;
using Store.Entities.RequestFeatures;
using Store.Repo.Contracts;
using Store.Service.Contracts;

//bu sınıf iş katmanıdır repo sınıfından cektiğimiz verilerin kontrolünü konfigürasyonunu yaparız
namespace Store.Service.Concretes;
public class BookService(IRepoManager manager, ILoggerService logger, IMapper mapper, IDataShaper<BookDTO> dataShaper, ICategoryService categoryService) : IBookService
{
    private readonly IRepoManager _manager = manager;
    private readonly ICategoryService _categoryService = categoryService;
    private readonly ILoggerService _logger = logger;
    private readonly IMapper _mapper = mapper;
    private readonly IDataShaper<BookDTO> _dataShaper = dataShaper;

    public async Task<BookDTO> CreateOneBookAsync(BookDTOForInsertion bookdto)
    {

        var category = await _categoryService
               .GetOneCategoryByIdAsync(bookdto.CategoryId);
               
        var book = _mapper.Map<Book>(bookdto);
        _manager.Book.Create(book);
        await _manager.SaveAsync();
        return _mapper.Map<BookDTO>(book);
    }

    public async Task DeleteOneBookAsync(int id, bool trackChanges)
    {
        // var entity = await _manager.Book.GetByIdAsync(x => x.BookId == id);
        // if (entity is null)
        //     throw new BookNotFoundException(id);

        var entity = await GetOneBookByIdAndCheckExists(id);
        _manager.Book.Delete(entity);
        await _manager.SaveAsync();
    }

    // public async Task<IEnumerable<BookDTO>> GetAllBookAsync(BookParameters bookParameters,bool trackChanges)
    // public async Task<(IEnumerable<BookDTO> books, MetaData metaData)> GetAllBookAsync(BookParameters bookParameters, bool trackChanges)
    public async Task<(IEnumerable<ExpandoObject> books, MetaData metaData)> GetAllBookAsync(BookParameters bookParameters, bool trackChanges)
    {
        if (!bookParameters.ValidPriceRange)
            throw new PriceAutofRangeBadrequestException();

        var booksWithMetaData = await _manager.Book.GetAllBookAsync(bookParameters, trackChanges);
        var booksDTO = _mapper.Map<IEnumerable<BookDTO>>(booksWithMetaData);
        // return (booksDTO, booksWithMetaData.MetaData);
        var shapedData = _dataShaper.ShapeData(booksDTO, bookParameters.Fields);
        return (books: shapedData, metaData: booksWithMetaData.MetaData);
    }

    public async Task<BookDTO?> GetOneBookByIdAsync(int id)
    {
        // var book = await _manager.Book.GetByIdAsync(x => x.BookId == id);
        // if (book is null) throw new BookNotFoundException(id);
        var book = await GetOneBookByIdAndCheckExists(id);
        return _mapper.Map<BookDTO>(book);
    }

    public async Task UpdateOneBookAsync(int id, BookDTOForUpdate bookDTO)
    {
        // var entity = await _manager.Book.GetByIdAsync(x => x.BookId == id);
        // if (entity is null)
        //     throw new BookNotFoundException(id);

        var entity = await GetOneBookByIdAndCheckExists(id);
        if (bookDTO is null)
            throw new BookNotFoundException(id);
        //automapper kullandığımız için bu şekilde eşleştirmemize gerek kalmadı
        // entity.Title = book.Title;
        // entity.Author = book.Author;
        // entity.Category = book.Category;
        // entity.Price = book.Price;

        entity = _mapper.Map<Book>(bookDTO);
        //getbyid metodu getirdiği veriyi takip etmediği için update metodunu kullanıp takibi sağlayıp değiştirdik. eğer takip edilmiş bu satır gerek kalmazdı. zaten değişiklikleri izlediği için direk save metodunu çağırman yeterli olurdu
        _manager.Book.Update(entity);
        await _manager.SaveAsync();
    }

    public async Task<bool> AnyAsync(Expression<Func<Book, bool>> expression, bool trackChanges) => await _manager.Book.AnyAsync(expression);

    public IEnumerable<Book> WhereAsync(Expression<Func<Book, bool>> expression, bool trackChanges) => _manager.Book.FindByConditionAsync(expression, trackChanges);

    private async Task<Book> GetOneBookByIdAndCheckExists(int id)
    {
        // check entity 
        var entity = await _manager.Book.GetByIdAsync(x => x.BookId == id);
        if (entity is null)
            throw new BookNotFoundException(id);
        return entity;
    }

    public async Task<IEnumerable<Book>> GetAllBooksWithDetailsAsync(bool trackChanges)
    {
        return await _manager
                 .Book
                 .GetAllBooksWithDetailsAsync(trackChanges);
    }
}