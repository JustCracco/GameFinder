using GameFinder_WebAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace GameFinder_WebAPI.Database
{
    public class GameDb : DbContext
    {
        public GameDb(DbContextOptions<GameDb> options) : base(options) { }

        public DbSet<Game> Games { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            => optionsBuilder.UseInMemoryDatabase(databaseName: "GameList");
        

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Game>().HasData(new Game
            {
                Id = 1,
                Name = "Final Fantasy 7",
                Production = "Square Enix",
                Date = "31/01/1997",
                Vote = 92
            });

            modelBuilder.Entity<Game>().HasData(new Game
            {
                Id = 2,
                Name = "The Last of Us",
                Production = "Naughty Dog",
                Date = "14/06/2013",
                Vote = 95
            });

            modelBuilder.Entity<Game>().HasData(new Game
            {
                Id = 3,
                Name = "Fallout 4",
                Production = "Bethesda",
                Date = "10/11/2015",
                Vote = 84
            });
        }
    }   
}
