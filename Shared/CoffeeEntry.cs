using System;

namespace BlazorApp.Shared
{
    public class CoffeeEntry
    {
        public string ImagePath { get; set; }
        public string CoffeeBeans { get; set; }
        public string TasteNotes { get; set; }
        public string Description { get; set; }
        public DateTimeOffset Time { get; set; }
        public int Rating { get; set; }
        public int BrewTime { get; set; }
    }
}
