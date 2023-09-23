using AutoMapper;

namespace Liberia.Helpers
{
	public class AutoMapperProfile : Profile
	{
		public AutoMapperProfile()
		{
			CreateMap<Category, CategoryVM>().ReverseMap();

			CreateMap<Author, AuthorVM>().ReverseMap();

			CreateMap<Book, BookVM>()
				.ForMember(dest => dest.Author, from => from.MapFrom(src => src.Author!.Name))
				.ForMember(dest => dest.Categories, from => from.MapFrom(src => src.Categories.Select(e => e.Category!.Name)));

			CreateMap<BookCopy, BookCopyVM>()
			   .ForMember(dest => dest.BookTitle, from => from.MapFrom(src => src.Book!.Title));

			CreateMap<ApplicationUser, UserVM>()
				.ForMember(dest => dest.UserId, from => from.MapFrom(src => src.Id));

			CreateMap<UserFormVM, ApplicationUser>()
			.ForMember(dest => dest.Id, from => from.MapFrom(src => src.UserId));
		}
	}
}
