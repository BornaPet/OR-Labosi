using OtvorenoRacunalstvoLabosi.DTO;
using OtvorenoRacunalstvoLabosi.Models;

namespace OtvorenoRacunalstvoLabosi.Services
{
    public interface IMonumentService
    {
        Task<IEnumerable<Monument>> GetAll();
        Task<Monument> GetById(int id);
        Task<IEnumerable<Monument>> SearchByName(string name);
        Task<IEnumerable<Artist>> SearchByNameArtists(string name);
        Task<bool> Create(Monument monument);
        Task<bool> Update(int id, Monument monument);
        Task<bool> Delete(int id);
    }

}
