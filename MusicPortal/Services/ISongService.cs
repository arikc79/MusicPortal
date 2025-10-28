using System.Collections.Generic;
using System.Threading.Tasks;
using MusicPortal.Models;

namespace MusicPortal.Services
{
    public interface ISongService
    {
        Task<IEnumerable<Song>> GetAllAsync();
        Task<Song> GetByIdAsync(int id);
        Task AddSongAsync(Song song);
        Task<IEnumerable<Song>> GetByGenreAsync(int genreId);
        Task<IEnumerable<Song>> GetByUserAsync(int userId);
    }
}
