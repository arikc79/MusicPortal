using MusicPortal.Data;
using MusicPortal.Models;

namespace MusicPortal.Services
{
    public class GenreService : IGenreService
    {
        private readonly IRepository<Genre> _repository;

        public GenreService(IRepository<Genre> repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<Genre>> GetAllAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<Genre?> GetByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task AddGenreAsync(Genre genre)
        {
            await _repository.AddAsync(genre);
        }

        public async Task DeleteAsync(int id)
        {
            var genre = await _repository.GetByIdAsync(id);
            if (genre != null)
            {
                await _repository.DeleteAsync(genre);
            }
        }

        public async Task UpdateAsync(Genre genre)
        {
            await _repository.UpdateAsync(genre);
        }
    }
}
