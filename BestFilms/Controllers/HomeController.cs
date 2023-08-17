using BestFilms.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace BestFilms.Controllers
{
    public class HomeController : Controller
    {
  
        FilmsContext db;
        public HomeController(FilmsContext context)
        {          
                db = context;           
        }

        public async Task<IActionResult> Index()
        {
            IEnumerable<Film> f = await Task.Run(() => db.Films);
            ViewBag.Films = f;
            return View();
        }

    
    }
}