using AutoMapper;
using LibraryApp.Application.Books.Queries.GetBooksByAuthor;
using LibraryApp.Application.Books.Queries.GetCardBooks;
using LibraryApp.Application.User.Commands;
using LibraryApp.Domain.Entities;
using LibraryApp.Infrastructure.Identity.Models;
using System.Linq;

namespace LibraryApp.Application.Common.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Book, CardBookDto>();
            CreateMap<UserRegistrationCommand, AppUser>();
            CreateMap<Book, LibraryBookDto>()
                .ForMember(dto => dto.Authors,
                opt => opt.MapFrom(b => b.Authors))
                .ForMember(dto => dto.BecomeAvailableDate,
                opt => opt.MapFrom(b => b.ReturnDate));
        }
    }
}
