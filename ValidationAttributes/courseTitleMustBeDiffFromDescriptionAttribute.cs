using Assignment9.Dto;
using System.ComponentModel.DataAnnotations;

namespace Assignment9.ValidationAttributes
{
    public class courseTitleMustBeDiffFromDescriptionAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value,
            ValidationContext validationContext)
        {
            var course = (CourseForManipulationDto)validationContext.ObjectInstance;
            if (course.Title == course.Description)
            {
                return new ValidationResult(
                    "The provided description should be different from title.", new[] { nameof(CourseForManipulationDto) });
            }
            return ValidationResult.Success;
        }
    }
}
