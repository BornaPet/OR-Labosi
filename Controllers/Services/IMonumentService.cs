using OtvorenoRacunalstvoLabosi.DTO;

namespace OtvorenoRacunalstvoLabosi.Controllers.Services
{
    public interface IMonumentService
    {
        Task<IEnumerable<MonumentDto>> GetAll();
        Task<MonumentDto> GetById(int id);
        Task<IEnumerable<MonumentDto>> SearchByName(string name);
        Task<IEnumerable<ArtistDto>> SearchByNameArtists(string name);
        Task<bool> Create(MonumentDto monumentDto);
        Task<bool> Update(int id, MonumentDto monumentDto);
        Task<bool> Delete(int id);
    }

}
