using System.ComponentModel.DataAnnotations;

namespace MyMvcProject.Models
{
    public class Music
    {
        public int Id { get; set; }
        [MaxLength(50)]
        public required string Name { get; set; }
        [MaxLength(255)]
        public required string Property { get; set; }
        [MaxLength(100)]
        public required string PosterPath { get; set; }
        [MaxLength(100)]
        public required string MusicPath { get; set; }

        public required int View { get; set; }
        public required int Like { get; set; }
        [MaxLength(255)]
        public required string Time { get; set; }


        public int AlbumId { get; set; }     // 🔗 Foreign Key
        public Album? Album { get; set; }    // 🔁 Navigation Property
        public List<MusicInteraction> Interactions { get; } = new List<MusicInteraction>(); // اضافه شده
    }
}
