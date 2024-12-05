using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PokemonApi.DTOs;
using PokemonApi.InterFace;
using PokemonApi.Moldels;

namespace PokemonApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewController : ControllerBase
    {
        private readonly IReviewRepository _repository;
        private readonly IPokemonRepository _pokemonRepository;
        private readonly IReviewerRepository _reviewerRepository;
        private readonly IMapper _mapper;

        public ReviewController(IReviewRepository repository,IPokemonRepository pokemonRepository,IReviewerRepository reviewerRepository,IMapper mapper)
        {
            this._repository = repository;
            this._pokemonRepository = pokemonRepository;
            this._reviewerRepository = reviewerRepository;
            this._mapper = mapper;
        }
        [HttpGet("Reviews")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Review>))]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public IActionResult GetReviews()
        {
          var Reviews=  _mapper.Map<List<ReviewDtos>>(_repository.GetReviews());
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            return Ok(Reviews);
        }
        [HttpGet("{Id}/review")]
        [ProducesResponseType(200, Type = typeof(Review))]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public IActionResult GetReview(int Id)
        {
            if(!_repository.ReviewExist(Id))
                return NotFound();
            var Review=_mapper.Map<ReviewDtos>(_repository.GetReview(Id));
            if (!ModelState.IsValid) 
                return BadRequest( ModelState);

            return Ok(Review);
        }
        [HttpGet("{pokeId}/reviews")]
        [ProducesResponseType(200, Type = typeof(Review))]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public IActionResult GetReviewsByPokemon(int pokeId)
        {
            if (!_repository.ReviewExist(pokeId))
                return NotFound();
            var Review=_mapper.Map<List<ReviewDtos>>(_repository.GetReviewsOfAPokemon(pokeId));
            if(!ModelState.IsValid)
                return BadRequest(ModelState);
            return Ok(Review);
        }
        [HttpPost("CreaterReview")]
        public IActionResult CreaterReview([FromQuery] int pokeId,[FromQuery] int reviewerId,[FromBody] ReviewDtos CreaterReview)
        {
            if (CreaterReview == null)
                return BadRequest();
            var rviewer= _repository.GetReviews().Where(x=>x.Title.Trim().ToUpper()
            == CreaterReview.Title.TrimEnd().ToUpper()).FirstOrDefault();
            if (rviewer != null)
            {
                ModelState.AddModelError("", "Review already exist");
                return StatusCode(402, ModelState);
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var mapReview = _mapper.Map<Review>(CreaterReview);
            mapReview.Pokemon=_pokemonRepository.GetPokemon(pokeId);    
            mapReview.Reviewer=_reviewerRepository.GetReviewer(reviewerId);
            if (!_repository.CreateReview(mapReview))
            {
                ModelState.AddModelError("", "Something went wrong");
                return StatusCode(500, ModelState);
            }
            return Ok("Successfully created");
                 
        }
        [HttpPut("UpdateReview/{id}")]
        public IActionResult UpdateOwner([FromRoute] int id, [FromBody] ReviewDtos review)
        {
            if (review == null)
                return BadRequest();
            if (review.Id != id)
                return BadRequest();
            if (!_repository.ReviewExist(id))
                return NotFound();
            if (!ModelState.IsValid)
                return BadRequest();
            var mapReview = _mapper.Map<Review>(review);
            if (!_repository.UpdateReview(mapReview))
            {
                ModelState.AddModelError("", "Something went wrong");
                return StatusCode(500, ModelState);
            }
            return NoContent();
        }
        [HttpDelete("DeleteReview")]
        public IActionResult DeleteCategry(int id)
        {
            if (!_repository.ReviewExist(id))
                return NotFound();
            var review = _repository.GetReview(id);
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            if (!_repository.DeleteReview(review))
            {
                ModelState.AddModelError("", "Something went wrong");
                return StatusCode(500, ModelState);
            }
            return NoContent();
        }
    }
}
