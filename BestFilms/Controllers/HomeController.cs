using BestFilms.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.IO;

namespace BestFilms.Controllers
{
    public class HomeController : Controller
    {
        IWebHostEnvironment _appEnvironment;
        FilmsContext db;
        public HomeController(FilmsContext context, IWebHostEnvironment appEnvironment)
        {          
                db = context;
            _appEnvironment = appEnvironment;
        }

        public async Task<IActionResult> Index()
        {
            IEnumerable<Film> f = await Task.Run(() => db.Films);
            ViewBag.Films = f;
            return View();
        }
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,Genre,Director,Year,Story")] Film film, IFormFile photo)
        {
            if (photo != null)
            {
                // Путь к папке Files
                string path = "/Posters/" + photo.FileName; // имя файла

                using (var fileStream = new FileStream(_appEnvironment.WebRootPath + path, FileMode.Create))
                {
                    await photo.CopyToAsync(fileStream); // копируем файл в поток
                }
                Film f = new();
                f.Name = film.Name;
                f.Genre = film.Genre;
                f.Director = film.Director;
                f.Year = film.Year;
                f.Story = film.Story;
                f.Photo = path;
                try
                {
                    db.Add(f);
                    await db.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch
                {
                    return View(film);
                }
            }
            return View(film);
        }

    }
}