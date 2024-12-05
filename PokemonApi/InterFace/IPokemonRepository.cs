using PokemonApi.Moldels;

namespace PokemonApi.InterFace
{
    public interface IPokemonRepository
    {
        ICollection<Pokemon> GetPokemns();
        Pokemon GetPokemon(int id);
        Pokemon GetPokemon(string name);
        decimal GetPokemonRating(int pokeId);
        bool PokemonExist(int PokeId);
        bool  CreatePokemon(int Ownid,int Cateid, Pokemon pokemon);
        bool UpdatePokemon(int Ownid,int Cateid, Pokemon pokemon);
        bool DeletePokemon( Pokemon pokemon);
        bool  Save();
    }
}
