using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PokemonApi.DTOs;
using PokemonApi.InterFace;
using PokemonApi.Moldels;

namespace PokemonApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewerController : ControllerBase
    {
        private readonly IReviewerRepository _repository;
        private readonly IMapper _mapper;

        public ReviewerController(IReviewerRepository repository,IMapper mapper)
        {
            this._repository = repository;
            this._mapper = mapper;
        }
        [HttpGet("reviewers")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Reviewer>))]
        [ProducesResponseType(500)]
        public IActionResult GetReviewers()
        {
            var reviewers=_mapper.Map<List<ReviewerDto>>(_repository.GetReviewers());
            if(!ModelState.IsValid) 
                return BadRequest(ModelState);
            return Ok(reviewers);
        }
        [HttpGet("reviewerId")]
        [ProducesResponseType(200, Type = typeof(Reviewer))]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public IActionResult GetPokemon(int reviewerId)
        {
            if (!_repository.ReviewerExist(reviewerId))
                return NotFound();
            var reviewer=_mapper.Map<PokemonDTo>(_repository.GetReviewer(reviewerId));
            if(!ModelState.IsValid)
                return BadRequest(ModelState);
            return Ok(reviewer);
        }
        [HttpGet("reviewerId/reviews")]
        [ProducesResponseType(200, Type = typeof(Reviewer))]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public IActionResult GetReviewsByAReviewer(int reviewerId)
        {
            if (!_repository.ReviewerExist(reviewerId))
                return NotFound();
            var reviews=_mapper.Map<List<ReviewDtos>>(_repository.GetReviewsByReviewer(reviewerId));
            if(!ModelState.IsValid)
                return BadRequest(ModelState);
            return Ok(reviews);
        }
        [HttpPost("CreateReviewer")]
        public IActionResult CreateReviewer([FromBody]ReviewerDto CreateReviewer)
        {
            if (CreateReviewer == null)
            {
                return BadRequest();
            }
            var reviewer=_repository.GetReviewers().Where(x=>x.LastName.Trim().ToUpper()
            == CreateReviewer.LastName.ToUpper().TrimEnd()).FirstOrDefault();
            if(reviewer != null)
            {
                ModelState.AddModelError("", "Reviewer already exist");
                return StatusCode(402, ModelState);
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var MapReviewer = _mapper.Map<Reviewer>(CreateReviewer);
            if (!_repository.CreateReviewer(MapReviewer))
            {
                ModelState.AddModelError("", "Something went wrong");
                return StatusCode(500, ModelState);
            }
                
            return Ok("Successfully created");
        }
        [HttpPut("UpdateReviewer/{id}")]
        public IActionResult UpdateOwner([FromRoute] int id, [FromBody] ReviewerDto reviewer)
        {
            if (reviewer == null)
                return BadRequest();
            if (reviewer.Id != id)
                return BadRequest();
            if (!_repository.ReviewerExist(id))
                return NotFound();
            if (!ModelState.IsValid)
                return BadRequest();
            var mapReviewer = _mapper.Map<Reviewer>(reviewer);
            if (!_repository.UpateReviewer(mapReviewer))
            {
                ModelState.AddModelError("", "Something went wrong");
                return StatusCode(500, ModelState);
            }
            return NoContent();
         
        }
        [HttpDelete("DeleteReviewer")]
        public IActionResult DeleteCategry(int id)
        {
            if (!_repository.ReviewerExist(id))
                return NotFound();
            var pokemon = _repository.GetReviewer(id);
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            if (!_repository.DeleteReviewer(pokemon))
            {
                ModelState.AddModelError("", "Something went wrong");
                return StatusCode(500, ModelState);
            }
            return NoContent();
        }
    }
    
}
