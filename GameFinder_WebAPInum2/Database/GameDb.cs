using GameFinder_WebAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace GameFinder_WebAPI.Database
{
    public class GameDb : DbContext
    {
        public GameDb(DbContextOptions options) : base(options) { }

        public DbSet<Game> Games => Set<Game>();
    }
}
