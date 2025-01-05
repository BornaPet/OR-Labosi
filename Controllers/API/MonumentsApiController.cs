using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OtvorenoRacunalstvoLabosi.Controllers.Services;
using OtvorenoRacunalstvoLabosi.DTO;

namespace OtvorenoRacunalstvoLabosi.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class MonumentsApiController : ControllerBase
    {
        private readonly IMonumentService _monumentService;

        public MonumentsApiController(IMonumentService monumentService)
        {
            _monumentService = monumentService;
        }
        /// <summary>
        /// Dohvaća sve spomenike
        /// </summary>
        /// <returns>Lista spomenika</returns>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var monuments = await _monumentService.GetAll();
            return Ok(new { success = true, data = monuments });
        }
        /// <summary>
        /// Dohvaća spomenik prema ID-u
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Spomenik objekt</returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var monument = await _monumentService.GetById(id);
            if (monument == null)
                return NotFound(new { success = false, message = "Monument not found" });

            return Ok(new { success = true, data = monument });
        }
        /// <summary>
        /// Filtrira spomenike prema imenu
        /// </summary>
        /// <param name="name"></param>
        /// <returns>Lista spomenika</returns>
        [HttpGet("filter")]
        public async Task<IActionResult> Search([FromQuery] string name)
        {
            var results = await _monumentService.SearchByName(name);
            return Ok(new { success = true, data = results });
        }
        /// <summary>
        /// Filtrira umjetnike prema imenu
        /// </summary>
        /// <param name="name"></param>
        /// <returns>Lista umjetnika</returns>
        [HttpGet("artist")]
        public async Task<IActionResult> SearchArtists([FromQuery] string name)
        {
            var results = await _monumentService.SearchByName(name);
            return Ok(new { success = true, data = results });
        }
        /// <summary>
        /// Stvara novi spomenik
        /// </summary>
        /// <param name="monumentDto"></param>
        /// <returns>Stranica</returns>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] MonumentDto monumentDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { success = false, message = "Invalid data" });

            await _monumentService.Create(monumentDto);
            return CreatedAtAction(nameof(GetById), new { id = monumentDto.Id }, new { success = true, data = monumentDto });
        }
        /// <summary>
        /// Uređuje spomenik
        /// </summary>
        /// <param name="id"></param>
        /// <param name="monumentDto"></param>
        /// <returns>Poruku o uspješnosti promjene</returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] MonumentDto monumentDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { success = false, message = "Invalid data" });

            var updated = await _monumentService.Update(id, monumentDto);
            if (!updated)
                return NotFound(new { success = false, message = "Monument not found" });

            return Ok(new { success = true, message = "Monument updated successfully" });
        }
        /// <summary>
        /// Briše spomenik
        /// </summary>
        /// <param name="id"></param>
        /// <returns> Poruku o uspješnosti brisanja</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _monumentService.Delete(id);
            if (!deleted)
                return NotFound(new { success = false, message = "Monument not found" });

            return Ok(new { success = true, message = "Monument deleted successfully" });
        }
    }

}
