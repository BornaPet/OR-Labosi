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


namespace OtvorenoRacunalstvoLabosi.Controllers
{
    public class MonumentsController : Controller
    {
        private readonly OtvorenoRacunarstvoLabosiContext _context;

        public MonumentsController(OtvorenoRacunarstvoLabosiContext context)
        {
            _context = context;
        }


        [HttpGet]
        public async Task<IActionResult> Index()
        {

            var model = await _context.Monuments
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
                         }).ToListAsync();
            return View(model);
        }
       

        [HttpGet("filtered")]
        public IActionResult GetFilteredMonuments(string searchField, string searchTerm, string format = "json")
        {
            var query = _context.Monuments.Include(m => m.Artist).ToList();

            var monuments = query;
            if (searchTerm != null)
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


            if (format.ToLower() == "csv")
                return ExportToCsv(monuments);
            else return ExportToJson(monuments);
        }

        private IActionResult ExportToCsv(IEnumerable<Monument> monuments)
        {
            if (monuments == null || !monuments.Any())
            {
                return NotFound("No data available to export.");
            }

            var csvConfig = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                Delimiter = ",",
                Encoding = Encoding.UTF8,
                HasHeaderRecord = true
            };

            using var memoryStream = new MemoryStream();
            using (var writer = new StreamWriter(memoryStream, leaveOpen: true))
            using (var csv = new CsvHelper.CsvWriter(writer, csvConfig))
            {
                csv.WriteRecords(monuments);
            }

            memoryStream.Position = 0;
            return File(memoryStream.ToArray(), "text/csv", "monuments.csv");
        }

        private IActionResult ExportToJson(IEnumerable<Monument> monuments)
        {
            var options = new JsonSerializerOptions { WriteIndented = true };
            var jsonData = JsonSerializer.Serialize(monuments, options);

            return File(Encoding.UTF8.GetBytes(jsonData), "application/json", "monuments.json");
        }

        public IActionResult RefreshData()
        {
            RefreshCsv();
            RefreshJson();
            
            return RedirectToAction("Index");
        }
        public void RefreshCsv()
        {
            var monuments = _context.Monuments.Include(x => x.Artist).ToList();

            
            var csvConfig = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                Delimiter = ",",
                Encoding = Encoding.UTF8,
                HasHeaderRecord = true
            };

            string dataFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "data");
            string filePath = Path.Combine(dataFolder, "monuments.csv");

            if (!Directory.Exists(dataFolder))
            {
                Directory.CreateDirectory(dataFolder);
            }

            using (var writer = new StreamWriter(filePath, false, Encoding.UTF8)) 
            using (var csv = new CsvHelper.CsvWriter(writer, csvConfig))
            {
                csv.WriteRecords(monuments);
            }

            byte[] fileBytes = System.IO.File.ReadAllBytes(filePath);
        }
        public void RefreshJson()
        {
            var monuments = _context.Monuments.Include(x => x.Artist).ToList();
            var jsonLdMonuments = monuments.Select(m => new
            {
                @context = "https://schema.org",
                @type = "Sculpture",
                identifier = m.Id,
                name = m.Name,
                locationCreated = new
                {
                    @type = "Place",
                    name = m.Location,
                    address = new
                    {
                        @type = "PostalAddress",
                        addressLocality = m.District
                    }
                },
                artform = m.Type,
                dateCreated = m.YearInstalled,
                material = m.Material,
                height = new
                {
                    @type = "QuantitativeValue",
                    value = m.Height,
                    unitCode = "MTR"
                },
                keywords = m.HistoricalSignificance,
                aggregateRating = new
                {
                    @type = "AggregateRating",
                    ratingValue = m.Popularity
                },
                containedInPlace = new
                {
                    @type = "AdministrativeArea",
                    name = m.District
                },
                author = new
                {
                    @type = "Person",
                    name = m.Artist.Name,
                    birthDate = m.Artist.BirthYear,
                    deathDate = m.Artist.DeathYear,
                    nationality = m.Artist.Nationality
                }
            });
            var options = new JsonSerializerOptions
            {
                WriteIndented = true,
                Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping 
            };
            string dataFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "data");
            string filePath = Path.Combine(dataFolder, "monuments.json");

            if (!Directory.Exists(dataFolder))
            {
                Directory.CreateDirectory(dataFolder);
            }
            var jsonData = JsonSerializer.Serialize(jsonLdMonuments, options);
            System.IO.File.WriteAllText(filePath, jsonData, Encoding.UTF8);
            byte[] fileBytes = System.IO.File.ReadAllBytes(filePath);

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


        [HttpGet]
        public IActionResult Create()
        {
            var artists = _context.Artists.ToList();

            ViewBag.Artists = new SelectList(artists, "Id", "Name");

            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Location,Type,YearInstalled,Material,Height,HistoricalSignificance,Popularity,District,Artist")] Monument monument)
        {

            var artist = _context.Artists.Where(x => x.Id == monument.Artist.Id).FirstOrDefault();
            var artist1 = _context.Artists.OrderBy(x => x.Id).LastOrDefault();
            var monument1 = _context.Monuments.OrderBy(x => x.Id).LastOrDefault();
            artist.Id = artist1.Id+1;
            _context.Add(artist);
            monument.Artist = artist;
            monument.Id = monument1.Id + 1;
            if (ModelState.IsValid)
            {
                _context.Add(monument);
                await _context.SaveChangesAsync();
                artist.MonumentId = monument.Id;
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            return RedirectToAction("Index");
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            var monument = _context.Monuments.Find(id);
            if(monument == null)
            {

                return RedirectToAction("Index");
            }
            if (monument != null)
            {
                _context.Monuments.Remove(monument);
            }

             _context.SaveChanges();
            return RedirectToAction("Index");
        }

        private bool MonumentExists(int id)
        {
            return _context.Monuments.Any(e => e.Id == id);
        }
    }
}
