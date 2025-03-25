using Microsoft.EntityFrameworkCore;
using MCMProject;
using Microsoft.Extensions.Options;

namespace MCMProject
{
    public class MovieDbContext : DbContext
    {
        public DbSet<Movie> Movies { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // For now, use an in-memory database or a placeholder connection string
            /*optionsBuilder.UseMySql("Server=localhost;Database=MovieCollection;User=root;Password=mypassword;",
                                    ServerVersion.AutoDetect("Server=localhost;Database=MovieCollection;User=root;Password=mypassword;"));*/
            optionsBuilder.UseInMemoryDatabase("MovieCollection");
        }
    }
}