
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyMvcProject.Models
{
    public class MusicInteraction
    {
        public int Id { get; set; }

        [Required]
        public int MusicId { get; set; }

        [Required]
        [MaxLength(50)]
        public required string DeviceId { get; set; }

        [Required]
        public bool IsLiked { get; set; }

        [Required]
        public bool IsViewed { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; }

        [ForeignKey("MusicId")]
        public  Music Music { get; set; }
    }
}