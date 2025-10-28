using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MusicPortal.Data;
using MusicPortal.Models;
using Microsoft.EntityFrameworkCore;

namespace MusicPortal.Services
{
    // Сервіс відповідає за роботу з піснями: додавання, отримання, фільтрація
    public class SongService : ISongService
    {
        private readonly ApplicationDbContext _context;
        private readonly IRepository<Song> _songRepo;

        public SongService(ApplicationDbContext context, IRepository<Song> songRepo)
        {
            _context = context;
            _songRepo = songRepo;
        }

        public async Task<IEnumerable<Song>> GetAllAsync()
        {
            // Include дозволяє одразу підвантажити пов’язані сутності (User, Genre)
            return await _context.Songs.Include(s => s.User).Include(s => s.Genre).ToListAsync();
        }

        public async Task<Song> GetByIdAsync(int id) => await _songRepo.GetByIdAsync(id);

        public async Task AddSongAsync(Song song)
        {
            await _songRepo.AddAsync(song);
            await _songRepo.SaveAsync();
        }

        public async Task<IEnumerable<Song>> GetByGenreAsync(int genreId)
        {
            return await _context.Songs
                .Include(s => s.Genre)
                .Where(s => s.GenreId == genreId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Song>> GetByUserAsync(int userId)
        {
            return await _context.Songs
                .Include(s => s.User)
                .Where(s => s.UserId == userId)
                .ToListAsync();
        }
    }
}
