using Microsoft.EntityFrameworkCore;

namespace yelp.Models
{
    public class ProjectContext : DbContext
    {
        public ProjectContext(DbContextOptions<ProjectContext> options) : base(options) { }
        public DbSet<User> User { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<Listing> Listings { get; set; }
    }
}