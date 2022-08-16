namespace Assignment9.Dto
{
    public class AuthorForCreatingDto
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public DateTimeOffset DateOfBirth { get; set; }
        public string? MainCategory { get; set; }
        public ICollection<CourseForCreatingDto> Courses { get; set; }
         = new List<CourseForCreatingDto>();
    }
}
