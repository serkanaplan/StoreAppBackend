using AutoMapper;
using Store.Entities;
using Store.Entities.DTOS;
using Store.Entities.Models;

namespace Store.Service.Mapping;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<BookDTOForUpdate, Book>();
        CreateMap<Book, BookDTO>();
        CreateMap<BookDTOForInsertion, Book>();
        CreateMap<UserForRegistrationDTO, User>();
    }

}
