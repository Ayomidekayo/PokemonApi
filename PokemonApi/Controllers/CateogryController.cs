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
    public class CateogryController : ControllerBase
    {
        private readonly ICategoryRepository _repository;
        private readonly IMapper _mapper;

        public CateogryController(ICategoryRepository repository, IMapper mapper)
        {
            this._repository = repository;
            this._mapper = mapper;
        }

        [HttpGet("Categorys")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Category>))]
        [ProducesResponseType(500)]
        public IActionResult GetCategories()
        {
            var categorys = _mapper.Map<List<CategoryDto>>(_repository.GetCategories());
            if (!ModelState.IsValid)
                BadRequest(ModelState);
            return Ok(categorys);
        }

        [HttpGet("Category")]
        [ProducesResponseType(200, Type = typeof(Category))]
        public IActionResult GetCategory(int id)
        {
            if (!_repository.CategoryExist(id))
                return NotFound();
            var category = _mapper.Map<CategoryDto>(_repository.GetCategory(id));
            if (!ModelState.IsValid)
                BadRequest(ModelState);
            return Ok(category);

        }
        [HttpGet("pokeemon/{categoryId}")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Pokemon>))]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public IActionResult GetPokemonByCategory(int categoryId)
        {
            if (!_repository.CategoryExist(categoryId))
                return NotFound();
            var pokemon = _mapper.Map<List<PokemonDTo>>(_repository.GetPokemonByCategogy(categoryId));
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            return Ok(pokemon);
        }
        [HttpPost("CreateCategory")]
        public IActionResult CreateCategory(CategoryDto Createcategory)
        {
            if (Createcategory == null)
                return BadRequest();
            var category = _repository.GetCategories()
                .Where(x => x.Name.Trim().ToUpper() == Createcategory.Name.TrimEnd().ToUpper()
               ).FirstOrDefault();

            if (category != null)
            {
                ModelState.AddModelError("", "cCategory already exist");
                return StatusCode(422, ModelState);
            }
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var categoryMap = _mapper.Map<Category>(Createcategory);
            if (!_repository.CreateCategory(categoryMap))
            {
                ModelState.AddModelError("", "Something went wrong");
                return StatusCode(500, ModelState);
            }
            return Ok("Successfully  created");

        }
        [HttpPut("UpdateCategory/{id}")]
        public IActionResult UpdateCategory([FromRoute] int id, [FromBody] CategoryDto category)
        {
            if (category == null)
                return BadRequest();
            if (category.Id != id)
                return BadRequest();
            if (!_repository.CategoryExist(id))
                return NotFound();
            if (!ModelState.IsValid)
                return BadRequest();
            var mapCategory = _mapper.Map<Category>(category);
            if (!_repository.UpdateCategory(mapCategory))
            {
                ModelState.AddModelError("", "Something went wrong");
                return StatusCode(500, ModelState);
            }
            return NoContent();
        }
        [HttpDelete("DeleteCategry")]
        public IActionResult DeleteCategry(int id)
        {
            if (!_repository.CategoryExist(id))
                return NotFound();
            var category=_repository.GetCategory(id);
            if(!ModelState.IsValid) 
                return BadRequest(ModelState);
            if (!_repository.DeleteCategory(category))
            {
                ModelState.AddModelError("", "Something went wrong");
                return StatusCode(500, ModelState);
            }
            return NoContent();
        }
    }
}
