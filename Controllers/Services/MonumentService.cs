﻿using AutoMapper;
using Microsoft.EntityFrameworkCore;
using OtvorenoRacunalstvoLabosi.DTO;
using OtvorenoRacunalstvoLabosi.Models;

namespace OtvorenoRacunalstvoLabosi.Controllers.Services
{
    public class MonumentService : IMonumentService
    {
        private readonly OtvorenoRacunarstvoLabosiContext _context;
        private readonly IMapper _mapper;
        public MonumentService(OtvorenoRacunarstvoLabosiContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<MonumentDto>> GetAll()
        {
            var model = await _context.Monuments.Include(m => m.Artist).ToListAsync();
            return _mapper.Map(model, new List<MonumentDto>());
        }

        public async Task<MonumentDto> GetById(int id)
        {
            var model = _context.Monuments.Include(m => m.Artist).FirstOrDefaultAsync(m => m.Id == id);
            return _mapper.Map(model, new MonumentDto());
        }

        public async Task<IEnumerable<MonumentDto>> SearchByName(string name)
        {
            var model = await _context.Monuments.Include(m => m.Artist).Where(m => m.Name.Contains(name)).ToListAsync();
            return _mapper.Map(model, new List<MonumentDto>());
        }
        public async Task<IEnumerable<ArtistDto>> SearchByNameArtists(string name)
        {
            var model = await _context.Artists.Where(m => m.Name.Contains(name)).ToListAsync();
            return _mapper.Map(model, new List<ArtistDto>());
        }

        public async Task<bool> Create(MonumentDto monumentDto)
        {
            var model = _mapper.Map(monumentDto, new Monument());
            _context.Monuments.Add(model);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> Update(int id, MonumentDto monumentDto)
        {
            var existing = await _context.Monuments.FindAsync(id);
            if (existing == null) return false;

            _context.Entry(existing).CurrentValues.SetValues(monumentDto);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> Delete(int id)
        {
            var monument = await _context.Monuments.FindAsync(id);
            if (monument == null) return false;

            _context.Monuments.Remove(monument);
            return await _context.SaveChangesAsync() > 0;
        }
    }

}
