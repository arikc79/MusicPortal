using MusicPortal.Data;
using MusicPortal.Models;

namespace MusicPortal.Services
{
    public class SongService : ISongService
    {
        private readonly IRepository<Song> _repository;

        public SongService(IRepository<Song> repository)
        {
            _repository = repository;
        }

        //  Отримати всі пісні
        public async Task<IEnumerable<Song>> GetAllAsync()
        {
            return await _repository.GetAllAsync();
        }

        //  Отримати пісню за Id
        public async Task<Song?> GetByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        //  Отримати пісні за жанром
        public async Task<IEnumerable<Song>> GetByGenreAsync(int genreId)
        {
            var allSongs = await _repository.GetAllAsync();
            return allSongs.Where(s => s.GenreId == genreId);
        }

        //  Додати нову пісню
        public async Task AddSongAsync(Song song)
        {
            await _repository.AddAsync(song);
        }

        //  Видалити пісню 
        public async Task DeleteAsync(int id)
        {
            var song = await _repository.GetByIdAsync(id);
            if (song != null)
            {
                await _repository.DeleteAsync(song);
            }
        }
    }
}
