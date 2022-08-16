using System.ComponentModel.DataAnnotations;

namespace Assignment9.Entities
{
    public class Author
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        [MaxLength(50)]
        public string? FirstName { get; set; }
        [Required]
        [MaxLength(50)]
        public string? LastName { get; set; }
        [Required]
        public DateTimeOffset DateofBirth { get; set; }
        [Required]
        [MaxLength(50)]
        public string? MainCategory { get; set; }
        public ICollection<Course> Courses { get; set; } = new List<Course>();
    }
}
