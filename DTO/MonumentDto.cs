using Microsoft.AspNetCore.Mvc;

namespace OtvorenoRacunalstvoLabosi.DTO
{

    public class MonumentDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }
        public int YearInstalled { get; set; }
        public string Material { get; set; }
        public double? Height { get; set; } 
        public string HistoricalSignificance { get; set; }
        public int? ArtistId { get; set; }
        public ArtistDto? Artist { get; set; }
    }
}
