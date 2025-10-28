using System.Collections.Generic;
using System.Threading.Tasks;
using MusicPortal.Models;

namespace MusicPortal.Services
{
    public interface IGenreService
    {
        Task<IEnumerable<Genre>> GetAllAsync();
        Task AddGenreAsync(Genre genre);
        Task DeleteAsync(int id);
    }
}
