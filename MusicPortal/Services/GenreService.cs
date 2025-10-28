using System.Collections.Generic;
using System.Threading.Tasks;
using MusicPortal.Data;
using MusicPortal.Models;

namespace MusicPortal.Services
{
    // Сервіс відповідає за управління жанрами
    public class GenreService : IGenreService
    {
        private readonly IRepository<Genre> _genreRepo;

        public GenreService(IRepository<Genre> genreRepo)
        {
            _genreRepo = genreRepo;
        }

        public async Task<IEnumerable<Genre>> GetAllAsync() => await _genreRepo.GetAllAsync();

        public async Task AddGenreAsync(Genre genre)
        {
            await _genreRepo.AddAsync(genre);
            await _genreRepo.SaveAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var genre = await _genreRepo.GetByIdAsync(id);
            if (genre != null)
            {
                _genreRepo.Delete(genre);
                await _genreRepo.SaveAsync();
            }
        }
    }
}
