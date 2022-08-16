using Assignment9.Date;
using Assignment9.Dto;
using Assignment9.ResourceParameter;
using Assignment9.Services;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace Assignment9.Controllers
{
    [ApiController]
    [Route("api/authors")]
    public class AuthorsController : ControllerBase
    {
        private readonly IAssignment9Repo _assignment9Repo;
        private readonly IMapper _mapper;
        public AuthorsController(IAssignment9Repo assignment9Repo, IMapper mapper)
        {
            _assignment9Repo = assignment9Repo ?? throw new ArgumentNullException(nameof(assignment9Repo));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }
        // get authors by query
        
        [HttpGet()]
        public ActionResult<IEnumerable<AuthorDto>> GetAuthors([FromQuery] AuthorResourcesParameter authorResourcesParameter)
        {
            var authorFromRepo = _assignment9Repo.GetAuthors(authorResourcesParameter);

            return Ok(_mapper.Map<IEnumerable<AuthorDto>>(authorFromRepo));
        }
        // get singele author by route binding method.
        [HttpGet("authorId", Name ="GetAuthor")]
        public IActionResult GetAuthor(Guid authorId)
        {
            var authorFromRepo = _assignment9Repo.GetAuthor(authorId);
            if (!_assignment9Repo.AuthorExists(authorId))
            {
                return NotFound();
            }
            
            return Ok(_mapper.Map<AuthorDto>(authorFromRepo));
        }
        //to create authors and course
        [HttpPost]
        public ActionResult<AuthorDto> CreateAuthor (AuthorForCreatingDto author)
        {
            var authorEntities = _mapper.Map<Entities.Author>(author);
            _assignment9Repo.AddAuthor(authorEntities);
            _assignment9Repo.Save();

            var authorToReturn = _mapper.Map<AuthorDto>(authorEntities);
            return CreatedAtRoute("GetAuthor", new { authorId = authorToReturn.Id }, authorToReturn);
        }
        // delete controler action
        [HttpDelete("{authorId}")]
        public ActionResult DeleteAuthor(Guid authorId)
        {
            var authorFromRepo = _assignment9Repo.GetAuthor(authorId);
            if (authorFromRepo == null)
            {
                return NotFound();

            }
            _assignment9Repo.DeleteAuthor(authorFromRepo);
            _assignment9Repo.Save();
            return NoContent();
        }
    }
} 
