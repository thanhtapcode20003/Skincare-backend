using Microsoft.EntityFrameworkCore;
using SkinCare_Data.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkinCare_Data
{
    public class SkinCare_DBContext : DbContext
    {
        public SkinCare_DBContext(DbContextOptions<SkinCare_DBContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<SkinType> SkinTypes { get; set; }
        public DbSet<SkinCareRoutine> SkinCareRoutines { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<Quizz> Quizzes { get; set; }
        public DbSet<QuizzAnswer> QuizzAnswers { get; set; }
        public DbSet<QuizzQuestion> QuizzQuestions { get; set; }
        public DbSet<RatingsFeedback> RatingsFeedbacks { get; set; }
        public DbSet<Promotion> Promotions { get; set; }
        public DbSet<Blog> Blogs { get; set; }
        public DbSet<Report> Reports { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // User relationships
            modelBuilder.Entity<User>()
                .HasMany(u => u.Orders)
                .WithOne(o => o.User)
                .HasForeignKey(o => o.UserId)
                .OnDelete(DeleteBehavior.Restrict);


            modelBuilder.Entity<User>()
                .HasMany(u => u.Quizzes)
                .WithOne(q => q.User)
                .HasForeignKey(q => q.UserId)
                .OnDelete(DeleteBehavior.Restrict);


            // Role relationships
            modelBuilder.Entity<Role>()
                .HasMany<User>()
                .WithOne(u => u.Role)
                .HasForeignKey(u => u.RoleId);
            modelBuilder.Entity<Role>().HasData(
                 new Role { RoleId = 1, RoleName = "Customer" },
                 new Role { RoleId = 2, RoleName = "Staff" },
                 new Role { RoleId = 3, RoleName = "Manager" }
              );

            // SkinType relationships
            modelBuilder.Entity<SkinType>()
                .HasMany<Product>()
                .WithOne(p => p.SkinType)
                .HasForeignKey(p => p.SkinTypeId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Product>()
                .HasOne(p => p.SkinType)
                .WithMany()
                .HasForeignKey(p => p.SkinTypeId)
                .OnDelete(DeleteBehavior.Restrict);

            // Order relationships
            modelBuilder.Entity<Order>()
                .HasMany(o => o.OrderDetails)
                .WithOne(od => od.Order)
                .HasForeignKey(od => od.OrderId);

            // Product relationships
            modelBuilder.Entity<Product>()
                .HasMany<RatingsFeedback>()
                .WithOne(rf => rf.Product)
                .HasForeignKey(rf => rf.ProductId)
                .OnDelete(DeleteBehavior.Restrict);


            //Promotions
            modelBuilder.Entity<Promotion>()
                .Property(p => p.DiscountPercentage)
                .HasColumnType("decimal(5, 2)"); // Adjust precision and scale as needed

            // Quiz relationships
            modelBuilder.Entity<Quizz>()
                .HasMany(q => q.QuizzAnswers)
                .WithOne(qa => qa.Quizz)
                .HasForeignKey(qa => qa.QuizId);

            modelBuilder.Entity<Quizz>()
                .HasMany(q => q.QuizzQuestions)
                .WithOne(qq => qq.Quizz)
                .HasForeignKey(qq => qq.QuizzId);

            // Blog relationships
            modelBuilder.Entity<Blog>()
                .HasOne(b => b.User)
                .WithMany()
                .HasForeignKey(b => b.UserId);

            modelBuilder.Entity<Blog>()
                .HasOne(b => b.Category)
                .WithMany()
                .HasForeignKey(b => b.CategoryId);

            // Report relationships
            modelBuilder.Entity<Report>()
                .HasOne(r => r.User)
                .WithMany()
                .HasForeignKey(r => r.UserId);


        }
    }
}
    
    
