using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using MusicPortal.Filters;
using MusicPortal.Models;
using MusicPortal.Services;

namespace MusicPortal.Controllers
{
    [Culture]
    public class SongsController : Controller
    {
        private readonly ISongService _songService;
        private readonly IGenreService _genreService;
        private readonly IWebHostEnvironment _env;

        public SongsController(ISongService songService, IGenreService genreService, IWebHostEnvironment env)
        {
            _songService = songService;
            _genreService = genreService;
            _env = env;
        }

        //  Перегляд пісень дозволений всім
        [AllowAnonymous]
        public async Task<IActionResult> Index(int? genreId)
        {
            var songs = genreId == null
                ? await _songService.GetAllAsync()
                : await _songService.GetByGenreAsync(genreId.Value);

            ViewBag.Genres = await _genreService.GetAllAsync();
            return View(songs);
        }

        //  Завантаження пісень — тільки для авторизованих
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Upload()
        {
            ViewBag.Genres = await _genreService.GetAllAsync();
            return View();
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Upload(IFormFile file, int genreId)
        {
            if (file == null || file.Length == 0)
            {
                ViewBag.Error = "Файл не вибрано!";
                return View();
            }

            var uploads = Path.Combine(_env.WebRootPath, "uploads");
            Directory.CreateDirectory(uploads);
            var path = Path.Combine(uploads, file.FileName);

            using (var stream = new FileStream(path, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            var userId = int.Parse(User.Claims.First(c => c.Type == "UserId").Value);

            var song = new Song
            {
                Title = file.FileName,
                FilePath = "/uploads/" + file.FileName,
                GenreId = genreId,
                UserId = userId
            };

            await _songService.AddSongAsync(song);
            return RedirectToAction("Index");
        }

        //  Гості не можуть качати
        [Authorize]
        public async Task<IActionResult> Download(int id)
        {
            var song = await _songService.GetByIdAsync(id);
            if (song == null) return NotFound();

            var path = Path.Combine(_env.WebRootPath, song.FilePath.TrimStart('/'));
            var bytes = await System.IO.File.ReadAllBytesAsync(path);
            return File(bytes, "audio/mpeg", song.Title);
        }
    }
}
