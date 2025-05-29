// مطمئن شوید namespace درست است
namespace MyMvcProject.Models
{
    public class AddMusic
    {
        public required List<Album> Albums { get; set; }
        public required Music Music { get; set; }

    }
    
}