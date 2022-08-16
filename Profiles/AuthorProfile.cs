using Assignment9.Date;
using AutoMapper;

namespace Assignment9.Profiles
{
    public class AuthorProfile : Profile
    {
        public AuthorProfile()
        {
            CreateMap<Entities.Author, Dto.AuthorDto>()
                .ForMember(
                    dest => dest.Name,
                    opt => opt.MapFrom(src => $"{src.FirstName} {src.LastName}"))

                .ForMember(
                    dest => dest.Age,
                    opt => opt.MapFrom(src => src.DateofBirth.GetCurrentAge()));
            CreateMap<Dto.AuthorForCreatingDto, Entities.Author>();
        }
    }
}
