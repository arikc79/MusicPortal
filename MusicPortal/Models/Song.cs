using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MusicPortal.Models
{
    public class Song
    {
        public int Id { get; set; }

        [Required, MaxLength(200)]
        public string Title { get; set; } = null!;

        public string FilePath { get; set; } = null!;

        [Required]
        public int GenreId { get; set; }

        [ForeignKey("GenreId")]
        public Genre Genre { get; set; } = null!;

        [Required]
        public int UserId { get; set; }

        [ForeignKey("UserId")]
        public User User { get; set; } = null!;
    }
}
