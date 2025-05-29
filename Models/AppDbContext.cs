using Microsoft.EntityFrameworkCore;

namespace MyMvcProject.Models
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Album> Albums { get; set; }
        public DbSet<Music> Musics { get; set; }
protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    base.OnModelCreating(modelBuilder);

    // تنظیم charset و collation برای کل دیتابیس
    modelBuilder.HasCharSet("utf8mb4")
                .UseCollation("utf8mb4_persian_ci");  // تغییر collation به "utf8mb4_persian_ci"

    // تنظیمات برای جدول Albums
    modelBuilder.Entity<Album>(entity =>
    {
        entity.HasCharSet("utf8mb4").UseCollation("utf8mb4_persian_ci");  // تغییر collation برای جدول Albums
        entity.Metadata.SetAnnotation("MySql:Engine", "InnoDB");
    });

    // تنظیمات برای جدول Musics
    modelBuilder.Entity<Music>(entity =>
    {
        entity.HasCharSet("utf8mb4").UseCollation("utf8mb4_persian_ci");  // تغییر collation برای جدول Musics
        entity.Metadata.SetAnnotation("MySql:Engine", "InnoDB");

        // تعریف رابطه Foreign Key با جدول Albums
        entity.HasOne(m => m.Album)
              .WithMany(a => a.Musics)
              .HasForeignKey(m => m.AlbumId)
              .OnDelete(DeleteBehavior.NoAction);
    });
}
    }
}
