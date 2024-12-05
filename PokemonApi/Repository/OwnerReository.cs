using PokemonApi.Data;
using PokemonApi.InterFace;
using PokemonApi.Moldels;

namespace PokemonApi.Repository
{
    public class OwnerReository : IOwnerReository
    {
        private readonly Appdbcontext _context;

        public OwnerReository(Appdbcontext context)
        {
            this._context = context;
        }

        public bool CreateOwner(Owner owner)
        {
            _context.Owners.Add(owner);
            return Save();
        }

        public bool DeleteteOwner(Owner owner)
        {
           _context.Remove(owner);
            return Save();
        }

        public Owner GetOwner(int id)
        {
            return _context.Owners.Where(u => u.Id == id).FirstOrDefault();
        }

        public ICollection<Owner> GetOwners()
        {
            return _context.Owners.ToList();
        }

        public ICollection<Owner> GetOwnersOfAPokemon(int PokeId)
        {
            return _context.PokemonOwners.Where(u => u.PokemonId == PokeId).Select(u=>u.Owner).ToList();
        }

        public ICollection<Pokemon> GetPokemonByOwner(int ownerId)
        {
           return _context.PokemonOwners.Where(u=>u.OwnerId == ownerId).Select(u=>u.Pokemon).ToList();
        }

        public bool OwnerExist(int ownerId)
        {
          return _context.Owners.Any(u=>u.Id == ownerId);
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool UpdateOwner(Owner owner)
        {
            _context.Update(owner);
            return Save();
        }
    }
}
