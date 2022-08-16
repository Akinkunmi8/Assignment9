using AutoMapper;

namespace Assignment9.Profiles
{
    public class CoursesProfile : Profile
    {
        public CoursesProfile()
        {
            CreateMap<Entities.Course, Dto.CourseDto>();
            CreateMap<Dto.CourseForCreatingDto, Entities.Course>();
            CreateMap<Dto.CourseForUpdateDto, Entities.Course>();
            CreateMap<Entities.Course, Dto.CourseForUpdateDto>();
        }
    }
}
