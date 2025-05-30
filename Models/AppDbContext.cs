using Microsoft.EntityFrameworkCore;
using MyMvcProject.Models;

namespace MyMvcProject.Models
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Album> Albums { get; set; }
        public DbSet<Music> Musics { get; set; }
        public DbSet<MusicInteraction> MusicInteractions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // تنظیم charset و collation برای کل دیتابیس
            modelBuilder.HasCharSet("utf8mb4")
                        .UseCollation("utf8mb4_persian_ci");

            // تنظیمات برای جدول Albums
            modelBuilder.Entity<Album>(entity =>
            {
                entity.ToTable("albums");
                entity.HasCharSet("utf8mb4").UseCollation("utf8mb4_persian_ci");
                entity.Property(a => a.Id).HasColumnName("id");
                entity.Property(a => a.Name).HasColumnName("Name");
                entity.Property(a => a.PosterPath).HasColumnName("PosterPath");
                entity.Metadata.SetAnnotation("MySql:Engine", "InnoDB");
            });

            // تنظیمات برای جدول Musics
            modelBuilder.Entity<Music>(entity =>
            {
                entity.ToTable("musics");
                entity.HasCharSet("utf8mb4").UseCollation("utf8mb4_persian_ci");
                entity.Property(m => m.Id).HasColumnName("Id");
                entity.Property(m => m.Name).HasColumnName("Name");
                entity.Property(m => m.Property).HasColumnName("Property");
                entity.Property(m => m.AlbumId).HasColumnName("AlbumId");
                entity.Property(m => m.PosterPath).HasColumnName("PosterPath");
                entity.Property(m => m.MusicPath).HasColumnName("MusicPath");
                entity.Property(m => m.View).HasColumnName("View");
                entity.Property(m => m.Like).HasColumnName("Like");
                entity.Property(m => m.Time).HasColumnName("Time");
                entity.Metadata.SetAnnotation("MySql:Engine", "InnoDB");

                // رابطه با Albums
                entity.HasOne(m => m.Album)
                      .WithMany(a => a.Musics)
                      .HasForeignKey(m => m.AlbumId)
                      .HasConstraintName("FK_musics_albums")
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // تنظیمات برای جدول MusicInteractions
            modelBuilder.Entity<MusicInteraction>(entity =>
            {
                entity.ToTable("MusicInteractions");
                entity.HasCharSet("utf8mb4").UseCollation("utf8mb4_persian_ci");

                // ایندکس منحصربه‌فرد
                entity.HasIndex(i => new { i.MusicId, i.DeviceId })
                      .IsUnique()
                      .HasDatabaseName("unique_interaction");

                // رابطه با musics
                entity.HasOne(i => i.Music)
                      .WithMany(m => m.Interactions)
                      .HasForeignKey(i => i.MusicId)
                      .HasConstraintName("FK_MusicInteractions_musics")
                      .OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}