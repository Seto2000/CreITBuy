using CreITBuy.Infrastructure.Data.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
#nullable disable
namespace CreITBuy.Infrastructures.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<Image> Images { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<JobRequest> JobRequests { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<Item> Items { get; set; }
        public DbSet<Card> Cards { get; set; }
        public DbSet<UserJobRequest> UserJobRequests { get; set; }
        public DbSet<ProductImage> ProductImages { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<ProductImage>()
                .HasKey(pi => new {pi.ProductId, pi.ImageId });

            builder.Entity<ProductImage>()
                .HasOne(pi=>pi.Product)
                .WithMany(p=>p.ProductImages)
                .HasForeignKey(pi=>pi.ProductId);

            builder.Entity<ProductImage>()
                .HasOne(pi=>pi.Image)
                .WithMany(i=>i.ProductImages)
                .HasForeignKey(pi=>pi.ImageId);

            builder.Entity<UserJobRequest>().HasOne(x => x.JobRequest).WithMany().OnDelete(DeleteBehavior.NoAction);
        }
    }
}