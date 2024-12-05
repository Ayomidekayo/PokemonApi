using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PokemonApi.DTOs;
using PokemonApi.InterFace;
using PokemonApi.Moldels;

namespace PokemonApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PokemonController : ControllerBase
    {
        private readonly IPokemonRepository _repository;
        private readonly IReviewRepository _reviewRepository;
        private readonly IMapper _mapper;

        public PokemonController(IPokemonRepository repository,IReviewRepository reviewRepository, IMapper mapper)
        {
            this._repository = repository;
            this._reviewRepository = reviewRepository;
            this._mapper = mapper;
        }
        [HttpGet]
        [ProducesResponseType(200,Type =typeof(IEnumerable<Pokemon>))]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetPokemons()
        {
            var pokemons=_mapper.Map<List<PokemonDTo>>(_repository.GetPokemns());
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            return Ok(pokemons);
        }
        [HttpGet("pokeId")]
        [ProducesResponseType(200, Type =typeof(Pokemon))]
       // [ProducesResponseType()]
        public IActionResult GetPokemon(int PokeId)
        {
            if (!_repository.PokemonExist(PokeId))
                return NotFound();
            var pokemon =_mapper.Map<PokemonDTo>( _repository.GetPokemon(PokeId));
            if (!ModelState.IsValid) return BadRequest(ModelState);
            return Ok(pokemon);
        }
        [HttpGet("{PokeId}/rating")]
        [ProducesResponseType(200, Type = typeof(decimal))]

        public IActionResult GetPokemonRating(int PokeId)
        {
            if(!_repository.PokemonExist(PokeId))
                return NotFound();
            var rating= _repository.GetPokemonRating(PokeId);
            if(!ModelState.IsValid) return BadRequest(ModelState);
            return Ok(rating);
        }
        [HttpPost("CreatePokemon")]
        public IActionResult CreatePokemon([FromQuery] int CounId,int OwnId,PokemonDTo Createpokemon)
        {
            if (Createpokemon == null)
                return BadRequest();
            var Poke=_repository.GetPokemns().Where(x=>x.Name.Trim().ToUpper()
            == Createpokemon.Name.TrimEnd().ToUpper()).FirstOrDefault();
            if (Poke != null)
            {
                ModelState.AddModelError("", "Owner already exist");
                return StatusCode(402, ModelState);
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var PokeMap = _mapper.Map<Pokemon>(Createpokemon);
            if (!_repository.CreatePokemon(OwnId, CounId, PokeMap))
            {
                ModelState.AddModelError("", "Something went wrong");
                return StatusCode(500, ModelState);
            }
            return Ok("Successfully Created");
        }
        [HttpPut("UpdatePoken/{Pokeid}")]
        public IActionResult UpdateOwner([FromRoute] int OwnId,[FromRoute] int CateId, [FromRoute] int Pokeid, [FromBody] PokemonDTo pokemon)
        {
            if (pokemon == null)
                return BadRequest();
            if (pokemon.Id != Pokeid)
                return BadRequest();
            if (!_repository.PokemonExist(Pokeid))
                return NotFound();
            if (!ModelState.IsValid)
                return BadRequest();
            var mapPoke = _mapper.Map<Pokemon>(pokemon);
            if (!_repository.UpdatePokemon(CateId,OwnId,mapPoke))
            {
                ModelState.AddModelError("", "Something went wrong");
                return StatusCode(500, ModelState);
            }
            return NoContent();
        }
         [HttpDelete("DeletePokemon")]
        public IActionResult DeleteCategry(int id)
        {
            if (!_repository.PokemonExist(id))
                return NotFound();
            var pokemon = _repository.GetPokemon(id);
            var revie=_reviewRepository.GetReviews();
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            if (!_reviewRepository.DeleteReviews(revie.ToList()))
             {
                ModelState.AddModelError("", "Something went wrong");
                return StatusCode(500, ModelState);
            }
            if (!_repository.DeletePokemon(pokemon))
            {
                ModelState.AddModelError("", "Something went wrong");
                return StatusCode(500, ModelState);
            }
            return NoContent();
        }
    }

}
