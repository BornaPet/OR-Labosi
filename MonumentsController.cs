﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OtvorenoRacunalstvoLabosi.DTO;
using OtvorenoRacunalstvoLabosi.Models;
using Microsoft.CodeAnalysis.Elfie.Serialization;
using System.Globalization;
using CsvHelper.Configuration;
using CsvHelper;

namespace OtvorenoRacunalstvoLabosi
{
    public class MonumentsController : Controller
    {
        private readonly OtvorenoRacunarstvoLabosiContext _context;

        public MonumentsController(OtvorenoRacunarstvoLabosiContext context)
        {
            _context = context;
        }

        // GET: Monuments
        public async Task<IActionResult> Index()
        {
            var model = _context.Monuments
             .Include(m => m.Artist)
             .Select(m => new MonumentDto
             {
                 Id = m.Id,
                 Name = m.Name,
                 Location = m.Location,
                 YearInstalled = (int)m.YearInstalled,
                 Material = m.Material,
                 Height = m.Height,
                 HistoricalSignificance = m.HistoricalSignificance,

                 ArtistId = m.Id,
                 Artist = m.Artist != null ? new ArtistDto
                 {
                     Id = m.Artist.Id,
                     Name = m.Artist.Name,
                     BirthYear = m.Artist.BirthYear,
                     DeathYear = m.Artist.DeathYear,
                     Nationality = m.Artist.Nationality
                 } : null
             }).ToList();
            return View(model);
        }

        [HttpGet("filtered")]
        public  IActionResult GetFilteredMonuments(string searchField, string searchTerm, string format = "json")
        {
            
            var query = _context.Monuments.Include(m => m.Artist).ToList();

            var monuments = query;
            if(searchTerm != null)
            {
                searchTerm = searchTerm.ToLower();
                if (searchField == "name")
                {
                    monuments = query.Where(x => x.Name.ToLower().Contains(searchTerm)).ToList();
                }
                else if (searchField == "location")
                {
                    monuments = query.Where(x => x.Location.ToLower().Contains(searchTerm)).ToList();
                }
                else if (searchField == "material")
                {
                    monuments = query.Where(x => x.Material.ToLower().Contains(searchTerm)).ToList();
                }
                else if (searchField == "artist")
                {
                    monuments = query.Where(x => x.Artist.Name.ToLower().Contains(searchTerm)).ToList();
                }
                else if (searchField == "height")
                {
                    monuments = query.Where(x => x.Height.ToString().ToLower().Contains(searchTerm)).ToList();
                }
                else if (searchField == "an")
                {
                    monuments = query.Where(x => x.Artist.Nationality.ToLower().Contains(searchTerm)).ToList();
                }
                else if (searchField == "all")
                {
                    monuments = query.Where(x => x.Name.ToLower().Contains(searchTerm) 
                        || x.Location.ToLower().Contains(searchTerm) 
                        || x.Material.ToLower().Contains(searchTerm)
                        || x.Artist.Name.ToLower().Contains(searchTerm)
                        || x.Height.ToString().ToLower().ToString().Contains(searchTerm)
                        || x.Artist.Nationality.ToLower().Contains(searchTerm)).ToList();
                }


            }


            if (format.ToLower() == "csv") return ExportToCsv(monuments);
            else return ExportToJson(monuments);
        }
        private IActionResult ExportToCsv(IEnumerable<Monument> monuments)
        {
            var csvConfig = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                Delimiter = ",",
                Encoding = Encoding.UTF8
            };

            using var memoryStream = new MemoryStream();
            using var writer = new StreamWriter(memoryStream);
            using var csv = new CsvHelper.CsvWriter(writer, csvConfig);

            csv.WriteRecords(monuments); // Piše redove u CSV format
            writer.Flush();
            memoryStream.Position = 0;

            return File(memoryStream, "text/csv", "monuments.csv");
        }

        private IActionResult ExportToJson(IEnumerable<Monument> monuments)
        {
            var options = new JsonSerializerOptions { WriteIndented = true };
            var jsonData = JsonSerializer.Serialize(monuments, options);

            return File(Encoding.UTF8.GetBytes(jsonData), "application/json", "monuments.json");
        }


        // GET: Monuments/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var monument = await _context.Monuments
                .FirstOrDefaultAsync(m => m.Id == id);
            if (monument == null)
            {
                return NotFound();
            }

            return View(monument);
        }

        // GET: Monuments/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Monuments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Location,Type,YearInstalled,Material,Height,HistoricalSignificance,Popularity,District")] Monument monument)
        {
            if (ModelState.IsValid)
            {
                _context.Add(monument);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(monument);
        }

        // GET: Monuments/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var monument = await _context.Monuments.FindAsync(id);
            if (monument == null)
            {
                return NotFound();
            }
            return View(monument);
        }

        // POST: Monuments/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Location,Type,YearInstalled,Material,Height,HistoricalSignificance,Popularity,District")] Monument monument)
        {
            if (id != monument.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(monument);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MonumentExists(monument.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(monument);
        }

        // GET: Monuments/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var monument = await _context.Monuments
                .FirstOrDefaultAsync(m => m.Id == id);
            if (monument == null)
            {
                return NotFound();
            }

            return View(monument);
        }

        // POST: Monuments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var monument = await _context.Monuments.FindAsync(id);
            if (monument != null)
            {
                _context.Monuments.Remove(monument);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MonumentExists(int id)
        {
            return _context.Monuments.Any(e => e.Id == id);
        }
    }
}
