﻿namespace PokemonApi.Moldels
{
    public class PokemonCategory
    {
        public Pokemon Pokemon { get; set; }
        public int PokemonId { get; set; }
        public Category Category { get; set; }
        public int CategoryId { get; set; }
    }
}
