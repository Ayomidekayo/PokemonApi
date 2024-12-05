using PokemonApi.Moldels;

namespace PokemonApi.InterFace
{
    public interface IOwnerReository
    {
        ICollection<Owner> GetOwners();
        Owner GetOwner(int id);
        ICollection<Owner> GetOwnersOfAPokemon(int PokeId);
        ICollection<Pokemon> GetPokemonByOwner(int ownerId);
        bool OwnerExist(int ownerId);
        bool CreateOwner(Owner owner);
        bool UpdateOwner(Owner owner);
        bool DeleteteOwner(Owner owner);
        bool Save();
    }
}
