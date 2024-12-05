using PokemonApi.Data;
using PokemonApi.InterFace;
using PokemonApi.Moldels;

namespace PokemonApi.Repository
{
    public class PokemonRepository : IPokemonRepository
    {
        private readonly Appdbcontext _context;

        public PokemonRepository(Appdbcontext context)
        {
            this._context = context;
        }

        public bool CreatePokemon(int Ownid, int Cateid,Pokemon pokemon)
        {
            var PokeOwnerEnity = _context.Owners.Where(x => x.Id == Ownid).FirstOrDefault();
            var PokeCateEnity = _context.Categories.Where(x => x.Id == Cateid).FirstOrDefault();

            var PokeOwner = new PokemonOwner()
            {
                Pokemon = pokemon,
                Owner = PokeOwnerEnity
            };
            _context.Add(PokeOwner);
            var PokeCate = new PokemonCategory()
            {
                Pokemon = pokemon,
                Category = PokeCateEnity
            };
            _context.Add(PokeCate);
            _context.Add(pokemon);
            return Save();
        }

        public bool UpdatePokemon(int Ownid, int Cateid, Pokemon pokemon)
        {
            _context.Update(pokemon);
            return Save();
        }

        public ICollection<Pokemon> GetPokemns()
        {
            var pokemons=_context.Pokemons.OrderBy(p => p.Id).ToList();
            return pokemons;
        }

        public Pokemon GetPokemon(int id)
        {
            return _context.Pokemons.Where(u => u.Id == id).FirstOrDefault();
        }

        public Pokemon GetPokemon(string name)
        {
            return _context.Pokemons.Where(u => u.Name == name).FirstOrDefault();
        }

        public decimal GetPokemonRating(int pokeId)
        {
            var review=_context.Reviews.Where(u=>u.Pokemon.Id==pokeId);
            if (review.Count () <= 0)
                return 0;
            return ((decimal)review.Sum(r => r.Rating) / review.Count());
        }

        public bool PokemonExist(int PokeId)
        {
            return _context.Pokemons.Any(u => u.Id == PokeId);
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool DeletePokemon(Pokemon pokemon)
        {
             _context.Remove(pokemon);
            return Save();
        }
    }
} 
