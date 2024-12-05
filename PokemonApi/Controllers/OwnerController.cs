using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PokemonApi.DTOs;
using PokemonApi.InterFace;
using PokemonApi.Moldels;

namespace PokemonApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OwnerController : ControllerBase
    {
        private readonly IOwnerReository _reository;
        private readonly ICountryRepository _countryRepository;
        private readonly IMapper _mapper;

        public OwnerController(IOwnerReository reository, ICountryRepository countryRepository, IMapper mapper)
        {
            this._reository = reository;
            this._countryRepository = countryRepository;
            this._mapper = mapper;
        }
        [ProducesResponseType(200, Type = typeof(IEnumerable<Owner>))]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        [HttpGet("Owners")]
        public IActionResult GetOwners()
        {
            var owner = _mapper.Map<List<OwnerDto>>(_reository.GetOwners());
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            return Ok(owner);
        }
        [ProducesResponseType(200, Type = typeof(Owner))]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        [HttpGet("Owner")]
        public IActionResult GetOwner(int id)
        {
            if (!_reository.OwnerExist(id))
                return NotFound();
            var owner = _mapper.Map<OwnerDto>(_reository.GetOwner(id));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            return Ok(owner);
        }
        [ProducesResponseType(200, Type = typeof(Owner))]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        [HttpGet("Owners/{pokeId}")]
        public IActionResult GetOwnerByPokemon(int pokeId)
        {
            if (!_reository.OwnerExist(pokeId))
                return NotFound();
            var owner = _mapper.Map<List<OwnerDto>>(_reository.GetOwnersOfAPokemon(pokeId));
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            return Ok(owner);
        }
        [ProducesResponseType(200, Type = typeof(Owner))]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        [HttpGet("{ownerId}/pokemon")]
        public IActionResult GetPokemonByOwner(int ownerId)
        {
            if (!_reository.OwnerExist(ownerId))
                return NotFound();
            var pokemon = _mapper.Map<List<PokemonDTo>>(_reository.GetPokemonByOwner(ownerId));
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            return Ok(pokemon);
        }
        [HttpPost("CreateOwner")]
        public IActionResult CreateOwner([FromQuery] int couId, [FromBody] OwnerDto CreateOwner)
        {
            if (CreateOwner == null)
                return BadRequest();
            var owner = _reository.GetOwners().Where(x => x.LastName.Trim().ToUpper() ==
            CreateOwner.LastName.TrimEnd().ToUpper()).FirstOrDefault();
            if (owner != null)
            {
                ModelState.AddModelError("", "Owner already exist");
                return StatusCode(402, ModelState);
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var OwnerMap = _mapper.Map<Owner>(CreateOwner);
            OwnerMap.Country = _countryRepository.GetCountry(couId);
            if (!_reository.CreateOwner(OwnerMap))
            {
                ModelState.AddModelError("", "Something went wrong");
                return StatusCode(500, ModelState);
            }
            return Ok("Successfully  created");
        }
        [HttpPut("UpdateOwner/{id}")]
        public IActionResult UpdateOwner([FromRoute] int id, [FromBody] OwnerDto owner)
        {
            if (owner == null)
                return BadRequest();
            if (owner.Id != id)
                return BadRequest();
            if (!_reository.OwnerExist(id))
                return NotFound();
            if (!ModelState.IsValid)
                return BadRequest();
            var mapOwner = _mapper.Map<Owner>(owner);
            if (!_reository.UpdateOwner(mapOwner))
            {
                ModelState.AddModelError("", "Something went wrong");
                return StatusCode(500, ModelState);
            }
            return NoContent();
        }
        [HttpDelete("DeleteOwner")]
        public IActionResult DeleteCategry(int id)
        {
            if (!_reository.OwnerExist(id))
                return NotFound();
            var owner = _reository.GetOwner(id);
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            if (!_reository.DeleteteOwner(owner))
            {
                ModelState.AddModelError("", "Something went wrong");
                return StatusCode(500, ModelState);
            }
            return NoContent();
        }
    }
}
