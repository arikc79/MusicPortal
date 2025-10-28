using System.ComponentModel.DataAnnotations;

namespace MusicPortal.Models.ViewModels
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Вкажіть ім’я користувача")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Довжина імені — від 3 до 50 символів")]
        public string UserName { get; set; } = null!;

        [Required(ErrorMessage = "Вкажіть Email")]
        [EmailAddress(ErrorMessage = "Невірний формат Email")]
        public string Email { get; set; } = null!;

        [Required(ErrorMessage = "Вкажіть пароль")]
        [DataType(DataType.Password)]
        [StringLength(30, MinimumLength = 6, ErrorMessage = "Пароль має бути від 6 до 30 символів")]
        public string Password { get; set; } = null!;
    }
}
