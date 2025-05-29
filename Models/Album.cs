using System.ComponentModel.DataAnnotations;

namespace MyMvcProject.Models
{
    public class Album
    {
     public required int Id { get; set; }

    [MaxLength(255)]
    public required string Name { get; set; }

    [MaxLength(500)]
    public required string PosterPath { get; set; }
    public ICollection<Music>? Musics { get; set; }   // Navigation to children
    }
    
}
