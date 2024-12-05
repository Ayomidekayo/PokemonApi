using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PokemonApi.DTOs;
using PokemonApi.InterFace;
using PokemonApi.Moldels;

namespace PokemonApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CountryController : ControllerBase
    {
        private readonly ICountryRepository _repository;
        private readonly IMapper _mapper;

        public CountryController(ICountryRepository repository, IMapper mapper)
        {
            this._repository = repository;
            this._mapper = mapper;
        }
        [HttpGet("Countries")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Country>))]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public IActionResult GetCountries()
        {
            var countries = _mapper.Map<List<CountryDTO>>(_repository.GetCountries());
            if (!ModelState.IsValid)
                return BadRequest();
            return Ok(countries);
        }
        [HttpGet("Country")]
        [ProducesResponseType(200, Type = typeof(Country))]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public IActionResult GetCountry(int Id)
        {
            if (!_repository.CountryExist(Id))
                return NotFound();
            var country = _mapper.Map<CountryDTO>(_repository.GetCountry(Id));
            if (!ModelState.IsValid)
                return BadRequest();
            return Ok(country);
        }
        [HttpGet("/Owners/{OwnerId}")]
        [ProducesResponseType(200, Type = typeof(Country))]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public IActionResult GetCountryOfAnOwner(int OwnerId)
        {
            if (!_repository.CountryExist(OwnerId))
                return NotFound();
            var country = _mapper.Map<CountryDTO>(_repository.GetCountryByOwner(OwnerId));
            if (!ModelState.IsValid)
                return BadRequest();
            return Ok(country);
        }
        [HttpPost("createCountry")]
        public IActionResult CreateCountry([FromBody] CountryDTO createCountry)
        {
            if (createCountry == null)
                return BadRequest();
            var country = _repository.GetCountries().Where(x => x.Name.Trim().ToUpper() == createCountry.Name.TrimEnd().ToUpper())
            .FirstOrDefault();
            if (country != null)
            {
                ModelState.AddModelError("", "Country already exist");
                return StatusCode(402, ModelState);
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);

            }
            var countryMap = _mapper.Map<Country>(createCountry);
            if (!_repository.CreateCountry(countryMap))
            {
                ModelState.AddModelError("", "Something went wrong");
                return StatusCode(500, ModelState);
            }
            return Ok("Successfully  created");

        }
        [HttpPut("UpdateCountry/{id}")]
        public IActionResult UpdateCountry([FromRoute] int id, [FromBody] CountryDTO country)
        {
            if (country == null)
                return BadRequest();
            if (country.Id != id)
                return BadRequest();
            if (!_repository.CountryExist(id))
                return NotFound();
            if (!ModelState.IsValid)
                return BadRequest();
            var mapCounty = _mapper.Map<Country>(country);
            if (!_repository.UpdateCountry(mapCounty))
            {
                ModelState.AddModelError("", "Something went wrong");
                return StatusCode(500, ModelState);
            }
            return NoContent();
        }
        [HttpDelete("DeleteCountry")]
        public IActionResult DeleteCategry(int id)
        {
            if (!_repository.CountryExist(id))
                return NotFound();
            var country = _repository.GetCountry(id);
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            if (!_repository.DeleteCountry(country))
            {
                ModelState.AddModelError("", "Something went wrong");
                return StatusCode(500, ModelState);
            }
            return NoContent();
        }
    }
}