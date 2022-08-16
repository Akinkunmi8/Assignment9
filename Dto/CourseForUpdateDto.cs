using Assignment9.ValidationAttributes;
using System.ComponentModel.DataAnnotations;

namespace Assignment9.Dto
{
    public class CourseForUpdateDto : CourseForManipulationDto
    {
        [Required(ErrorMessage =" fill the Discription box. ")]
        public override string? Description { get => base.Description; set => base.Description = value; }

    }
}
