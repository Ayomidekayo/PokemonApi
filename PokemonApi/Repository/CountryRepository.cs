using PokemonApi.Data;
using PokemonApi.InterFace;
using PokemonApi.Moldels;

namespace PokemonApi.Repository
{
    public class CountryRepository : ICountryRepository
    {
        private readonly Appdbcontext _context;

        public CountryRepository(Appdbcontext context)
        {
            this._context = context;
        }
        public bool CountryExist(int Id)
        {
            return _context.Countries.Any(c => c.Id == Id);
        }

        public bool CreateCountry(Country country)
        {
            _context.Countries.Add(country);
            return Save();
        }

        public bool DeleteCountry(Country country)
        {
           _context.Remove(country);
            return Save();
        }

        public ICollection<Country> GetCountries()
        {
            return _context.Countries.OrderBy(u => u.Id).ToList();
        }

        public Country GetCountry(int Id)
        {
            return _context.Countries.Where(u => u.Id == Id).FirstOrDefault();
        }

        public Country GetCountryByOwner(int OwnerId)
        {
            return _context.Owners.Where(u => u.Id == OwnerId).Select(u => u.Country).FirstOrDefault();
        }

        public ICollection<Owner> GetOwnerFromCountry(int countryId)
        {
            return _context.Owners.Where(u => u.Country.Id == countryId).ToList();
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool UpdateCountry(Country country)
        {
            _context.Update(country);
            return Save();
        }
    }
}
