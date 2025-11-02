using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MusicPortal.Models
{
    public class Genre
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Назва жанру обов’язкова")]
        [MaxLength(100, ErrorMessage = "Назва не повинна перевищувати 100 символів")]
        public string Name { get; set; } = null!;

    
        public ICollection<Song> Songs { get; set; } = new List<Song>();
    }
}
