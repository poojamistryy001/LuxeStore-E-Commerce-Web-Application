using LuxeStore.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace LuxeStore.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductVariant> ProductVariants { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Product>()
                .Property(p => p.Price)
                .HasPrecision(18, 2);

            builder.Entity<Product>()
                .Property(p => p.OldPrice)
                .HasPrecision(18, 2);

            builder.Entity<Order>()
                .Property(o => o.TotalAmount)
                .HasPrecision(18, 2);

            builder.Entity<CartItem>()
                .Property(c => c.UnitPrice)
                .HasPrecision(18, 2);

            builder.Entity<OrderItem>()
                .Property(o => o.UnitPrice)
                .HasPrecision(18, 2);

            builder.Entity<OrderItem>()
                .Property(o => o.TotalPrice)
                .HasPrecision(18, 2);

            // Seed Categories
            builder.Entity<Category>().HasData(
                new Category { Id = 1, Name = "Footwear", Description = "Shoes, boots, sneakers" },
                new Category { Id = 2, Name = "Clothing", Description = "Tops, trousers, coats" },
                new Category { Id = 3, Name = "Accessories", Description = "Watches, belts, scarves" },
                new Category { Id = 4, Name = "Bags", Description = "Totes, backpacks, clutches" }
            );

            // Seed Products
            builder.Entity<Product>().HasData(
                new Product { Id = 1, Name = "Heritage Oxford Shoes", Description = "Premium leather oxfords with hand-stitched detailing.", Price = 8299, OldPrice = 10999, CategoryId = 1, StockQuantity = 50 },
                new Product { Id = 2, Name = "Urban Sneakers", Description = "Lightweight everyday sneakers with cushioned sole.", Price = 4499, OldPrice = 5999, CategoryId = 1, StockQuantity = 80 },
                new Product { Id = 3, Name = "Merino Wool Coat", Description = "Italian merino wool blend trench coat.", Price = 12450, OldPrice = 15999, CategoryId = 2, StockQuantity = 30 },
                new Product { Id = 4, Name = "Slim Chino Trousers", Description = "Stretch slim-fit chinos, wrinkle resistant.", Price = 3199, OldPrice = 4299, CategoryId = 2, StockQuantity = 100 },
                new Product { Id = 5, Name = "Silk Scarf", Description = "Pure silk scarf with hand-rolled edges.", Price = 2199, CategoryId = 3, StockQuantity = 60 },
                new Product { Id = 6, Name = "Aviator Sunglasses", Description = "UV400 polarized lenses in gold metal frame.", Price = 3699, OldPrice = 4500, CategoryId = 3, StockQuantity = 45 },
                new Product { Id = 7, Name = "Leather Tote Bag", Description = "Full-grain vegetable-tanned leather tote.", Price = 9100, OldPrice = 11500, CategoryId = 4, StockQuantity = 25 },
                new Product { Id = 8, Name = "Canvas Backpack", Description = "Waxed canvas backpack with laptop sleeve, 25L.", Price = 6749, CategoryId = 4, StockQuantity = 40 }
            );
        }
    }
}