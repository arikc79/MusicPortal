using Microsoft.AspNetCore.Mvc;
using MusicPortal.Services;
using MusicPortal.Models;
using System.Threading.Tasks;

namespace MusicPortal.Controllers
{
    public class AdminController : Controller
    {
        private readonly IUserService _userService;
        private readonly IGenreService _genreService;

        public AdminController(IUserService userService, IGenreService genreService)
        {
            _userService = userService;
            _genreService = genreService;
        }

        // --- Панель адміністратора ---
        public async Task<IActionResult> Index()
        {
            var users = await _userService.GetAllAsync();
            var genres = await _genreService.GetAllAsync();
            ViewBag.Users = users;
            ViewBag.Genres = genres;
            return View();
        }

        // --- Активація користувача ---
        public async Task<IActionResult> ActivateUser(int id)
        {
            await _userService.ActivateAsync(id);
            return RedirectToAction("Index");
        }

        // --- Додавання нового жанру ---
        [HttpPost]
        public async Task<IActionResult> AddGenre(string name)
        {
            if (!string.IsNullOrEmpty(name))
                await _genreService.AddGenreAsync(new Genre { Name = name });
            return RedirectToAction("Index");
        }

        // --- Видалення жанру ---
        public async Task<IActionResult> DeleteGenre(int id)
        {
            await _genreService.DeleteAsync(id);
            return RedirectToAction("Index");
        }
    }
}
