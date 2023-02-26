using VideoGameStore.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace VideoGameStore.Data
{
    public class AppDbContext:IdentityDbContext<ApplicationUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Publisher_VideoGame>().HasKey(am => new
            {
                am.PublisherId,
                am.VideoGameId
            });

            // Defining relationship of one to many, one game has many publishers
            modelBuilder.Entity<Publisher_VideoGame>().HasOne(m => m.VideoGame).WithMany(am => am.Publishers_VideoGames).HasForeignKey(m => m.VideoGameId);

            // Defining relationship of one to many, one publisher has many games
            modelBuilder.Entity<Publisher_VideoGame>().HasOne(m => m.Publisher).WithMany(am => am.Publishers_VideoGames).HasForeignKey(m => m.PublisherId);

            base.OnModelCreating(modelBuilder);

        }

        public DbSet<Publisher> Publishers { get; set; }

        public DbSet<VideoGame> VideoGames { get; set; }

        public DbSet<Publisher_VideoGame> Publishers_VideoGames { get; set; }

        public DbSet<Developer> Developers { get; set; }

        //Orders related tables
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }

        public DbSet<ShoppingCartItem> ShoppingCartItems { get; set; }   

    }
}
