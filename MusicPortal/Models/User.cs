using System.ComponentModel.DataAnnotations;

namespace MusicPortal.Models
{
    public enum UserRole { User, Admin }

    public class User
    {
        public int Id { get; set; }

        [Required, MaxLength(100, ErrorMessage = "Ім’я користувача занадто довге")]
        public string UserName { get; set; } = null!;

        [Required, EmailAddress(ErrorMessage = "Невірний формат Email")]
        public string Email { get; set; } = null!;

        [Required]
        public string PasswordHash { get; set; } = null!;

        public bool IsActive { get; set; } = false;
        public UserRole Role { get; set; } = UserRole.User;
    }
}
