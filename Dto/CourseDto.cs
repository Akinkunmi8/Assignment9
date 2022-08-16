namespace Assignment9.Dto
{
    public class CourseDto
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public Guid AuthorId { get; set; }
    }
}
