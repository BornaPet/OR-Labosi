using AutoMapper;
using OtvorenoRacunalstvoLabosi.DTO;
using OtvorenoRacunalstvoLabosi.Models;

namespace OtvorenoRacunalstvoLabosi.Models
{
    public class Mappings : Profile
    {
        public Mappings()
        {
            CreateMap<Monument, MonumentDto>().ReverseMap();
            CreateMap<Artist, ArtistDto>().ReverseMap();
        }
    }
}
