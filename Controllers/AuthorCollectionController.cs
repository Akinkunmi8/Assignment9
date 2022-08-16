using Assignment9.Date;
using Assignment9.Dto;
using Assignment9.Services;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace Assignment9.Controllers
{
    [ApiController]
    [Route("api/authorCollection")]
    public class AuthorCollectionController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IAssignment9Repo _assignment9Repo1;
        public AuthorCollectionController(IAssignment9Repo assignment9Repo, IMapper mapper)
        {
           _assignment9Repo1 = assignment9Repo ?? throw new ArgumentNullException(nameof(assignment9Repo));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }
        // Key 
        [HttpGet("({ids})", Name = "GetAuthoresCollection")]
        public ActionResult GetAuthorCollection([FromRoute][ModelBinder(BinderType = typeof(ArrayModelBinding))] IEnumerable<Guid> ids)
        {
         if(ids == null)
            {
                return BadRequest();
            }
            var authorEntites = _assignment9Repo1.GetAuthors(ids);
            if (ids.Count() != authorEntites.Count())
            {
                return NotFound();
            }
            var authorsroReturn = _mapper.Map<IEnumerable<AuthorDto>>(authorEntites);
            return Ok(authorsroReturn);

        }
        // Controller action for AuthorCollectio.
        [HttpPost]
        public ActionResult<IEnumerable<AuthorDto>> CreatauthorCollection(IEnumerable<AuthorForCreatingDto> authorCollection)
        {
            var authorEntities = _mapper.Map<IEnumerable<Entities.Author>>(authorCollection);
            foreach (var author in authorEntities)
            {
                _assignment9Repo1.AddAuthor(author);
            }
            _assignment9Repo1.Save();
            
            var authorCollectionToReturn = _mapper.Map<IEnumerable<AuthorDto>>(authorEntities);
            var idsAsString = string.Join(",", authorCollectionToReturn.Select(a => a.Id));
            return CreatedAtRoute("GetAuthorCollection", new { ids = idsAsString }, authorCollectionToReturn);
        }
        [HttpOptions]
        public IActionResult GetAthorsOptions()
        {
            Response.Headers.Add("ALLOW", "OPTIONS");
            return Ok();
        }

    }
}
