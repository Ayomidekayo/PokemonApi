namespace PokemonApi.Moldels
{
    public class Reviewer
    {
        public int Id { get; set; }
        public String firstName { get; set; }
        public String LastName { get; set; }
        public ICollection<Review> Reviews { get; set; }
    }
}
