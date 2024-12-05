using PokemonApi.Moldels;


namespace PokemonApi.InterFace
{
    public interface ICountryRepository
    {
        ICollection<Country> GetCountries();
        Country GetCountry(int Id);
        Country GetCountryByOwner(int OwnerId);
        ICollection<Owner> GetOwnerFromCountry(int countryId);
        bool CountryExist(int Id);
        bool CreateCountry(Country country);
        bool UpdateCountry(Country country);
        bool DeleteCountry(Country country);
        bool Save();
    }
}
