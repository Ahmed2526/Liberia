using AutoMapper;

namespace Liberia.Helpers
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Category, CategoryVM>().ReverseMap();

            CreateMap<Author, AuthorVM>().ReverseMap();

        }
    }
}
