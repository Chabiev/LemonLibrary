using AutoMapper;
using Business.DTOs;
using Database.Entities;

namespace Business.Helpers;

public class AutoMapperProfiles : Profile
{
    public AutoMapperProfiles()
    {
        CreateMap<Book, BookDTO>()
                .ForMember(dest => dest.Authors, opt => opt.MapFrom(src => src.BookAuthors.Select(ba => ba.Author).ToList()));
        CreateMap<BookAuthor, BookAuthorDTO>();
        CreateMap<Author, AuthorDTO>();
    }
}