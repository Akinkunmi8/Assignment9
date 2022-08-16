using Assignment9.Dto;
using Assignment9.Services;
using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Options;

namespace Assignment9.Controllers
{
    [ApiController]
    [Route("api/authors/{authorId}/courses")]
    public class CoursesController : ControllerBase
    {
        private readonly IAssignment9Repo _assignment9Repo;
        private readonly IMapper _mapper;
        public CoursesController(IAssignment9Repo assignment9Repo, IMapper mapper)
        {
            _assignment9Repo = assignment9Repo ?? throw new ArgumentNullException(nameof(assignment9Repo));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        [HttpGet]
        public ActionResult<IEnumerable<CourseDto>> GetCoursesForAuthor(Guid authorId)
        {
            if (!_assignment9Repo.AuthorExists(authorId))
            {
                return NotFound();
            }

            var coursesForAuthorFromrepo = _assignment9Repo.GetCourses(authorId);
            return Ok (_mapper.Map<IEnumerable<CourseDto>>(coursesForAuthorFromrepo));

        }
        [HttpGet("courseId", Name ="GetCourseForAuthor")]
        public ActionResult<CourseDto> GetCourseForAuthor(Guid authorId, Guid courseId)
        {
            if (!_assignment9Repo.AuthorExists(authorId))
            {
                return NotFound();
            }
            var courseForAuthorFromrepo = _assignment9Repo.GetCourse(authorId, courseId);
            if (courseForAuthorFromrepo == null)
            {
                return NotFound();
            }
            return Ok (_mapper.Map<CourseDto>(courseForAuthorFromrepo));
        }
        [HttpPost]
        public ActionResult<CourseDto> CreateCoursForAuhor(Guid authorId,CourseForCreatingDto course)
        {
            if (!_assignment9Repo.AuthorExists(authorId))
            {
                return NotFound();
            }
            var courseEntity = _mapper.Map<Entities.Course>(course);
            _assignment9Repo.AddCourse(authorId, courseEntity);
            _assignment9Repo.Save();

            var courseToReturn = _mapper.Map<CourseDto>(courseEntity);
            return CreatedAtRoute("GetCourseForAuthor", new { authorId = authorId, courseId = courseToReturn.Id }, courseToReturn);

            
        }
        // to update coures
        [HttpPut("{courseId}")]
        public IActionResult UpdateCourseForAuthor(Guid authorId, Guid courseId, CourseForUpdateDto course)
        {
            if (!_assignment9Repo.AuthorExists(authorId))
            {
                return NotFound();
            }
            var courseForAuthorFromRepo = _assignment9Repo.GetCourse(authorId,courseId);
            if (courseForAuthorFromRepo == null)
            {
                // let create new course for the author.
                var courseToAdd = _mapper.Map<Entities.Course>(course);
                courseToAdd.Id = courseId;
                _assignment9Repo.AddCourse(authorId, courseToAdd);
                _assignment9Repo.Save();

                var courseToReturn = _mapper.Map<CourseDto>(courseToAdd);

                return CreatedAtRoute("GetCourseForAuthor", 
                    new {authorId, courseId = courseToReturn.Id }, courseToReturn);
            }
            // map the entity to CourseUpdateDto
            _mapper.Map(course, courseForAuthorFromRepo);
            _assignment9Repo.UpdateCourse(courseForAuthorFromRepo);
            _assignment9Repo.Save();
            return NoContent();
        }
        // Contoller action for Patch
        [HttpPatch("{courseId}")]
        public ActionResult PartiallyUpdateCourseForAuthor(Guid authorId,
            Guid courseId, JsonPatchDocument<CourseForUpdateDto> patchDocument)
        {
            if (!_assignment9Repo.AuthorExists(authorId))
            {
                return NotFound();
            }

            var courseForAuthorFromRepo = _assignment9Repo.GetCourse(authorId, courseId);
            if (courseForAuthorFromRepo == null)
            {
                // to create a patch if its null.
                var courseDto = new CourseForUpdateDto();
                patchDocument.ApplyTo(courseDto, ModelState);

                if (!TryValidateModel(courseDto))
                {
                    return ValidationProblem(ModelState);
                }
                var courseToAdd = _mapper.Map<Entities.Course>(courseDto);
                courseToAdd.Id = courseId;
                _assignment9Repo.AddCourse(authorId, courseToAdd);
                _assignment9Repo.Save();
                // to get patch response
                var courseToReturn = _mapper.Map<CourseDto>(courseToAdd);
                return CreatedAtRoute("GetCourseForAuthor",
                    new { authorId, courseId = courseToReturn.Id }, courseToReturn);
            }
            var courseToPatch = _mapper.Map<CourseForUpdateDto>(courseForAuthorFromRepo);
            // validation
            patchDocument.ApplyTo(courseToPatch, ModelState);
            if (!TryValidateModel(courseToPatch))
            {
                return ValidationProblem(ModelState);
            }
            _mapper.Map(courseToPatch, courseForAuthorFromRepo);
            _assignment9Repo.UpdateCourse(courseForAuthorFromRepo);
            _assignment9Repo.Save();

            return NoContent();
        }
        // delete Action Controller
        [HttpDelete("{courseId}")]
        public ActionResult DelelCourseForAuthor(Guid authorId, Guid courseId)
        {
            if (!_assignment9Repo.AuthorExists(authorId))
            {
                return NotFound();
            }
            var courseforAthorFromRepo = _assignment9Repo.GetCourse(authorId,courseId);
            if (courseforAthorFromRepo == null)
            {
                return NotFound();

            }
            _assignment9Repo.DeleteCourse(courseforAthorFromRepo);
            _assignment9Repo.Save();
            return NoContent();
        }

        public override ActionResult ValidationProblem(
            [ActionResultObjectValue] ModelStateDictionary modelStateCictionary)
        {
            var option = HttpContext.RequestServices
                .GetRequiredService<IOptions<ApiBehaviorOptions>>();
            return (ActionResult)option.Value.InvalidModelStateResponseFactory(ControllerContext);
        }



    }
}
