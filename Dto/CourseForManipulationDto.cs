using Assignment9.ValidationAttributes;
using System.ComponentModel.DataAnnotations;

namespace Assignment9.Dto
{
    [courseTitleMustBeDiffFromDescription(
    ErrorMessage = "Title must be different fro description")]

    public abstract class CourseForManipulationDto
    {

        [Required(ErrorMessage = "Fill the Title")]
        [MaxLength(100, ErrorMessage = "The Title shouldn't have more than 100 charcters")]
        public string? Title { get; set; }
        [Required(ErrorMessage = "Fill the Title")]
        [MaxLength(1500, ErrorMessage = "The descriptin should not have more then 1500 character")]
        public  virtual string? Description { get; set; }
    }
}