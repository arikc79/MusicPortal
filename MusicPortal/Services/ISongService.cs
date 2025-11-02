using MusicPortal.Models;

namespace MusicPortal.Services
{
    public interface ISongService
    {
        Task<IEnumerable<Song>> GetAllAsync();
        Task<Song?> GetByIdAsync(int id);
        Task<IEnumerable<Song>> GetByGenreAsync(int genreId);
        Task AddSongAsync(Song song);

     
        Task DeleteAsync(int id);
    }
}
