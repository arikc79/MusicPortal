using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MusicPortal.Models;
using MusicPortal.Services;

namespace MusicPortal.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly IUserService _userService;
        private readonly IGenreService _genreService;
        private readonly ISongService _songService;

        public AdminController(IUserService userService, IGenreService genreService, ISongService songService)
        {
            _userService = userService;
            _genreService = genreService;
            _songService = songService;
        }

        // --------------------- Admin Dashboard ---------------------
        public async Task<IActionResult> Index()
        {
            ViewBag.Users = await _userService.GetAllAsync();
            ViewBag.Genres = await _genreService.GetAllAsync();
            ViewBag.Songs = await _songService.GetAllAsync();
            return View();
        }

        // --------------------- Add Genre ---------------------
        [HttpPost]
        public async Task<IActionResult> AddGenre(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return RedirectToAction("Index");

            await _genreService.AddGenreAsync(new Genre { Name = name });
            return RedirectToAction("Index");
        }

        // --------------------- Activate User ---------------------
        [HttpPost]
        public async Task<IActionResult> ActivateUser(int id)
        {
            await _userService.ActivateAsync(id);
            return RedirectToAction("Index");
        }

        // --------------------- Delete User ---------------------
        [HttpPost]
        public async Task<IActionResult> DeleteUser(int id)
        {
            await _userService.DeleteAsync(id);
            return RedirectToAction("Index");
        }

        // --------------------- Delete Song ---------------------
        [HttpPost]
        public async Task<IActionResult> DeleteSong(int id)
        {
            await _songService.DeleteAsync(id);
            return RedirectToAction("Index");
        }

        // --------------------- Delete Genre (✅ повністю працює) ---------------------
        [HttpPost]
        public async Task<IActionResult> DeleteGenre(int id)
        {
            await _genreService.DeleteAsync(id);
            return RedirectToAction("Index");
        }
    }
}
