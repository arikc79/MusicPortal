using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MusicPortal.Data
{
    // Реалізація універсального репозиторію
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly ApplicationDbContext _context;
        private readonly DbSet<T> _dbSet;

        public Repository(ApplicationDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>(); // Повертає DbSet для сутності T
        }

        // Отримати всі записи
        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        // Отримати один запис за Id
        public async Task<T> GetByIdAsync(int id)
        {
            return await _dbSet.FindAsync(id)!;
        }

        // Додати новий запис
        public async Task AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
        }

        // Оновити запис
        public void Update(T entity)
        {
            _dbSet.Update(entity);
        }

        // Видалити запис
        public void Delete(T entity)
        {
            _dbSet.Remove(entity);
        }

        // Зберегти зміни до БД
        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
