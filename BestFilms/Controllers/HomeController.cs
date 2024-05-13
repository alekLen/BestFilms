using BestFilms.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.IO;
using static System.Net.WebRequestMethods;

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
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || db.Films == null)
            {
                return NotFound();
            }

            var f = await db.Films
                .FirstOrDefaultAsync(m => m.Id == id);
            if (f == null)
            {
                return NotFound();
            }

            return View(f);
        }
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,Genre,Director,Year,Story,")] Film film, IFormFile photo)
        {
            if (photo == null)
                ModelState.AddModelError("", "вы не добавили постер");
            DateTime today = DateTime.Today;
            int currentYear = today.Year;
            if (Convert.ToInt32(film.Year) < 1895|| Convert.ToInt32(film.Year)> currentYear)
                ModelState.AddModelError("", "не корректный год");
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
                if (ModelState.IsValid)
                {
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
                else
                    return View(film);
            }
            return View(film);
        }
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || db.Films == null)
            {
                return NotFound();
            }

            var f = await db.Films
                .FirstOrDefaultAsync(m => m.Id == id);
            if (f == null)
            {
                return NotFound();
            }

            return View(f);
        }
      
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (db.Films == null)
            {
                return Problem("Entity set 'FilmsContext.Films'  is null.");
            }
            var f = await db.Films.FindAsync(id);
            if (f != null)
            {
                db.Films.Remove(f);
             
            }
            await db.SaveChangesAsync();
            ClearPhoto();
            return RedirectToAction(nameof(Index));
        }

        private bool FilmExists(int id)
        {
            return (db.Films?.Any(e => e.Id == id)).GetValueOrDefault();
        }
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || db.Films == null)
            {
                return NotFound();
            }

            var f = await db.Films.FindAsync(id);
            if (f == null)
            {
                return NotFound();
            }
            return View(f);
        }
     
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Genre,Director,Year,Story,Photo")] Film f)
        {
            if (id != f.Id)
            {
                return NotFound();
            }
            DateTime today = DateTime.Today;
            int currentYear = today.Year;
            if (Convert.ToInt32(f.Year) < 1895 || Convert.ToInt32(f.Year) > currentYear)
                ModelState.AddModelError("", "не корректный год");

            if (ModelState.IsValid)
            {
                try
                {                   
                     db.Update(f);
                     await db.SaveChangesAsync();                  
                   
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FilmExists(f.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                ClearPhoto();
                return RedirectToAction(nameof(Index));
            }
            ClearPhoto();
            return View( f);
        }
      /*  [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit1(int id, [Bind("Id,Name,Genre,Director,Year,Story,Photo")] Film f, IFormFile photo)
        {
            if (id == null || db.Films == null)
            {
                return NotFound();
            }
        
                string oldPhoto = f.Photo;

            if (f == null)
            {
                return NotFound();
            }
            if(photo.FileName!=null)
              f.Photo = "/Posters/" + photo.FileName;
            using (var fileStream = new FileStream(_appEnvironment.WebRootPath + f.Photo, FileMode.Create))
            {
                await photo.CopyToAsync(fileStream); // копируем файл в поток             
            }
            if (id != f.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    db.Update(f);
                    await db.SaveChangesAsync();                   
                     deletePhoto(oldPhoto);                  
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FilmExists(f.Id))
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
            return View("Edit1", f);
        }*/

        [HttpPost]
       public async Task<IActionResult> Poster(int? id, IFormFile photo)
        {
            
            if (id == null || db.Films == null)
            {
                return NotFound();
            }
            var f = await db.Films.FindAsync(id);           
            if (f == null)
            {
                return NotFound();
            }
            f.Photo = "/Posters/" + photo.FileName;
            using (var fileStream = new FileStream(_appEnvironment.WebRootPath + f.Photo, FileMode.Create))
            {
                await photo.CopyToAsync(fileStream); // копируем файл в поток
            }
            return View("Edit",f);
       }
        public void deletePhoto(string s)
        {
            try
            {
                if (System.IO.File.Exists(s))
                {
                    System.IO.File.Delete(s);
                }
            }
            catch  {   }
        }  
        void ClearPhoto()
        {
            foreach(var f1 in Directory.GetFiles(_appEnvironment.WebRootPath + "/Posters/"))
            {
                bool q = false;
                foreach (var f in db.Films)
                {
                    if(f.Photo == "/Posters/"+Path.GetFileName(f1))
                        q=true;
                }
                if(!q)
                {
                    deletePhoto(f1);
                   
                }
            }
        }

    }

}