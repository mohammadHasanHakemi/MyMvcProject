// مطمئن شوید namespace درست است
namespace MyMvcProject.Models 
{
    public class AlbumMusicViewModel
    {
        public required List<Album> Albums { get; set; }
        public required List<Music> Musics { get; set; }
    }
}