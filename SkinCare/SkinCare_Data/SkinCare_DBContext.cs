using Microsoft.EntityFrameworkCore;
using SkinCare_Data.Data;
using System;

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

            // Cấu hình cho TotalAmount trong Order
            modelBuilder.Entity<Order>()
                .Property(o => o.TotalAmount)
                .HasColumnType("decimal(18,2)");

            // Cấu hình cho Price trong Product
            modelBuilder.Entity<Product>()
                .Property(p => p.Price)
                .HasColumnType("decimal(18,2)");

            // Cấu hình cho Price trong OrderDetail
            modelBuilder.Entity<OrderDetail>()
                .Property(od => od.Price)
                .HasColumnType("decimal(18,2)");

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
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired(false);

            // Category relationships
            modelBuilder.Entity<Category>()
                .HasMany(c => c.SubCategories)
                .WithOne(c => c.ParentCategory)
                .HasForeignKey(c => c.ParentCategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Category>()
                .HasOne(c => c.ParentCategory)
                .WithMany(c => c.SubCategories)
                .HasForeignKey(c => c.ParentCategoryId)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired(false);

            modelBuilder.Entity<Category>()
                .HasMany<Product>()
                .WithOne(p => p.Category)
                .HasForeignKey(p => p.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Product>()
                .HasOne(p => p.Category)
                .WithMany()
                .HasForeignKey(p => p.CategoryId)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired(false);

            // SkinCareRoutine relationships
            modelBuilder.Entity<SkinCareRoutine>()
                .HasMany<Product>()
                .WithOne(p => p.Routine)
                .HasForeignKey(p => p.RoutineId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Product>()
                .HasOne(p => p.Routine)
                .WithMany()
                .HasForeignKey(p => p.RoutineId)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired(false);

            // Product and RatingsFeedback relationship
            modelBuilder.Entity<Product>()
                .HasMany(p => p.RatingsFeedbacks)
                .WithOne(rf => rf.Product)
                .HasForeignKey(rf => rf.ProductId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<RatingsFeedback>()
                .HasOne(rf => rf.Product)
                .WithMany(p => p.RatingsFeedbacks)
                .HasForeignKey(rf => rf.ProductId)
                .OnDelete(DeleteBehavior.Restrict);

            // Order and OrderDetail relationship
            modelBuilder.Entity<Order>()
                .HasMany(o => o.OrderDetails)
                .WithOne(od => od.Order)
                .HasForeignKey(od => od.OrderId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<OrderDetail>()
                .HasOne(od => od.Product)
                .WithMany()
                .HasForeignKey(od => od.ProductId)
                .OnDelete(DeleteBehavior.Restrict);

            // Promotions
            modelBuilder.Entity<Promotion>()
                .Property(p => p.DiscountPercentage)
                .HasColumnType("decimal(5, 2)");

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