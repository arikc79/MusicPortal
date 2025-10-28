using Microsoft.EntityFrameworkCore;
using MusicPortal.Models;

namespace MusicPortal.Data
{
    // Головний клас, який відповідає за з'єднання з БД
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        { }

        // Набори сутностей (таблиці)
        public DbSet<User> Users { get; set; }
        public DbSet<Song> Songs { get; set; }
        public DbSet<Genre> Genres { get; set; }

        // Додаткові налаштування моделей
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Унікальність імен користувачів
            modelBuilder.Entity<User>()
                .HasIndex(u => u.UserName)
                .IsUnique();

            // Видалення пісень при видаленні жанру
            modelBuilder.Entity<Genre>()
                .HasMany(g => g.Songs)
                .WithOne(s => s.Genre)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
