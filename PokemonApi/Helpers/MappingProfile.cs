using AutoMapper;
using PokemonApi.DTOs;
using PokemonApi.Moldels;

namespace PokemonApi.Helpers
{
    public class MappingProfile:Profile
    {
        public MappingProfile()
        {
            CreateMap<Pokemon, PokemonDTo>().ReverseMap();
            CreateMap<Category, CategoryDto>().ReverseMap();
            CreateMap<Country, CountryDTO>().ReverseMap();
            CreateMap<Owner, OwnerDto>().ReverseMap();
            CreateMap<Review, ReviewDtos>().ReverseMap();
            CreateMap<Reviewer, ReviewerDto>().ReverseMap();
        }
    }
}
