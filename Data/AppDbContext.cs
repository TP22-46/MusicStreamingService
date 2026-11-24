using Microsoft.EntityFrameworkCore;
using MusicStreamingService.Models;

namespace MusicStreamingService.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Song> Songs => Set<Song>();
        public DbSet<Mood> Moods => Set<Mood>();
        public DbSet<User> Users => Set<User>();
        public DbSet<Favorite> Favorites => Set<Favorite>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasIndex(u => u.Phone).IsUnique();
            modelBuilder.Entity<Mood>().HasData(
                new Mood { Id = 1, Name = "Chill" },
                new Mood { Id = 2, Name = "Workout" },
                new Mood { Id = 3, Name = "Focus" }
            );

            modelBuilder.Entity<Song>().HasData(
                new Song { Id = 1, Title = "Soul", Artist = "Artist 1", Genre = "Pop", MoodId = 1, CoverUrl="/images/cat.jpg" },
                new Song { Id = 2, Title = "Fire", Artist = "Artist 2", Genre = "Rock", MoodId = 2 },
                new Song { Id = 3, Title = "Water", Artist = "Artist 3", Genre = "Jazz", MoodId = 3 }
            );
        }
    }
}
