﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SkinCare_Data;

#nullable disable

namespace SkinCare_Data.Migrations
{
    [DbContext(typeof(SkinCare_DBContext))]
    partial class SkinCare_DBContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("SkinCare_Data.Data.Blog", b =>
                {
                    b.Property<string>("BlogId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("CategoryId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("PublishDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("BlogId");

                    b.HasIndex("CategoryId");

                    b.HasIndex("UserId");

                    b.ToTable("Blogs");
                });

            modelBuilder.Entity("SkinCare_Data.Data.Category", b =>
                {
                    b.Property<string>("CategoryId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("CategoryName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("CategoryId");

                    b.ToTable("Categories");
                });

            modelBuilder.Entity("SkinCare_Data.Data.Order", b =>
                {
                    b.Property<string>("OrderId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTime>("CreateAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("OrderStatus")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("TotalAmount")
                        .HasColumnType("int");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("OrderId");

                    b.HasIndex("UserId");

                    b.ToTable("Orders");
                });

            modelBuilder.Entity("SkinCare_Data.Data.OrderDetail", b =>
                {
                    b.Property<string>("OrderDetailId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("OrderId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<float>("Price")
                        .HasColumnType("real");

                    b.Property<string>("ProductId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("OrderDetailId");

                    b.HasIndex("OrderId");

                    b.HasIndex("ProductId");

                    b.ToTable("OrderDetails");
                });

            modelBuilder.Entity("SkinCare_Data.Data.Product", b =>
                {
                    b.Property<string>("ProductId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("CategoryId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTime>("CreateAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Image")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<float>("Price")
                        .HasColumnType("real");

                    b.Property<string>("ProductName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Quantity")
                        .HasColumnType("int");

                    b.Property<string>("RatingFeedback")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RoutineId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("SkinTypeId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("ProductId");

                    b.HasIndex("CategoryId");

                    b.HasIndex("RoutineId");

                    b.HasIndex("SkinTypeId");

                    b.ToTable("Products");
                });

            modelBuilder.Entity("SkinCare_Data.Data.Promotion", b =>
                {
                    b.Property<string>("PromotionId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<decimal>("DiscountPercentage")
                        .HasColumnType("decimal(5, 2)");

                    b.Property<DateTime>("EndDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("PName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("StartDate")
                        .HasColumnType("datetime2");

                    b.Property<bool>("Status")
                        .HasColumnType("bit");

                    b.HasKey("PromotionId");

                    b.ToTable("Promotions");
                });

            modelBuilder.Entity("SkinCare_Data.Data.Quizz", b =>
                {
                    b.Property<string>("QuizzId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("SkinTypeId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTime>("TestDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("TotalScore")
                        .HasColumnType("int");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("QuizzId");

                    b.HasIndex("SkinTypeId");

                    b.HasIndex("UserId");

                    b.ToTable("Quizzes");
                });

            modelBuilder.Entity("SkinCare_Data.Data.QuizzAnswer", b =>
                {
                    b.Property<string>("QuizAnswerId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Answer")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("QuizId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("Score")
                        .HasColumnType("int");

                    b.HasKey("QuizAnswerId");

                    b.HasIndex("QuizId");

                    b.ToTable("QuizzAnswers");
                });

            modelBuilder.Entity("SkinCare_Data.Data.QuizzQuestion", b =>
                {
                    b.Property<string>("QuestionId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Question")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("QuizzId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("QuestionId");

                    b.HasIndex("QuizzId");

                    b.ToTable("QuizzQuestions");
                });

            modelBuilder.Entity("SkinCare_Data.Data.RatingsFeedback", b =>
                {
                    b.Property<string>("FeedbackId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Comment")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreateAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("ProductId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("Rating")
                        .HasColumnType("int");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("FeedbackId");

                    b.HasIndex("ProductId");

                    b.HasIndex("UserId");

                    b.ToTable("RatingsFeedbacks");
                });

            modelBuilder.Entity("SkinCare_Data.Data.RefreshToken", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("ExpiryDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Token")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("RefreshTokens");
                });

            modelBuilder.Entity("SkinCare_Data.Data.Report", b =>
                {
                    b.Property<string>("ReportId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("CategoryId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTime>("ReportDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("ReportId");

                    b.HasIndex("CategoryId");

                    b.HasIndex("UserId");

                    b.ToTable("Reports");
                });

            modelBuilder.Entity("SkinCare_Data.Data.Role", b =>
                {
                    b.Property<int>("RoleId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("RoleId"));

                    b.Property<string>("RoleName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("RoleId");

                    b.ToTable("Roles");

                    b.HasData(
                        new
                        {
                            RoleId = 1,
                            RoleName = "Customer"
                        },
                        new
                        {
                            RoleId = 2,
                            RoleName = "Staff"
                        },
                        new
                        {
                            RoleId = 3,
                            RoleName = "Manager"
                        });
                });

            modelBuilder.Entity("SkinCare_Data.Data.SkinCareRoutine", b =>
                {
                    b.Property<string>("RoutineId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("RoutineId");

                    b.ToTable("SkinCareRoutines");
                });

            modelBuilder.Entity("SkinCare_Data.Data.SkinType", b =>
                {
                    b.Property<string>("SkinTypeId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RoutineId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("SkinTypeName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("SkinTypeId");

                    b.HasIndex("RoutineId");

                    b.ToTable("SkinTypes");
                });

            modelBuilder.Entity("SkinCare_Data.Data.User", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreateAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("Point")
                        .HasColumnType("int");

                    b.Property<int>("RoleId")
                        .HasColumnType("int");

                    b.Property<string>("SkinTypeId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserId");

                    b.HasIndex("RoleId");

                    b.HasIndex("SkinTypeId");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("SkinCare_Data.Data.Blog", b =>
                {
                    b.HasOne("SkinCare_Data.Data.Category", "Category")
                        .WithMany()
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("SkinCare_Data.Data.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Category");

                    b.Navigation("User");
                });

            modelBuilder.Entity("SkinCare_Data.Data.Order", b =>
                {
                    b.HasOne("SkinCare_Data.Data.User", "User")
                        .WithMany("Orders")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("SkinCare_Data.Data.OrderDetail", b =>
                {
                    b.HasOne("SkinCare_Data.Data.Order", "Order")
                        .WithMany("OrderDetails")
                        .HasForeignKey("OrderId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("SkinCare_Data.Data.Product", "Product")
                        .WithMany()
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Order");

                    b.Navigation("Product");
                });

            modelBuilder.Entity("SkinCare_Data.Data.Product", b =>
                {
                    b.HasOne("SkinCare_Data.Data.Category", "Category")
                        .WithMany()
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("SkinCare_Data.Data.SkinCareRoutine", "Routine")
                        .WithMany()
                        .HasForeignKey("RoutineId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("SkinCare_Data.Data.SkinType", "SkinType")
                        .WithMany()
                        .HasForeignKey("SkinTypeId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.Navigation("Category");

                    b.Navigation("Routine");

                    b.Navigation("SkinType");
                });

            modelBuilder.Entity("SkinCare_Data.Data.Quizz", b =>
                {
                    b.HasOne("SkinCare_Data.Data.SkinType", "SkinType")
                        .WithMany()
                        .HasForeignKey("SkinTypeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("SkinCare_Data.Data.User", "User")
                        .WithMany("Quizzes")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("SkinType");

                    b.Navigation("User");
                });

            modelBuilder.Entity("SkinCare_Data.Data.QuizzAnswer", b =>
                {
                    b.HasOne("SkinCare_Data.Data.Quizz", "Quizz")
                        .WithMany("QuizzAnswers")
                        .HasForeignKey("QuizId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Quizz");
                });

            modelBuilder.Entity("SkinCare_Data.Data.QuizzQuestion", b =>
                {
                    b.HasOne("SkinCare_Data.Data.Quizz", "Quizz")
                        .WithMany("QuizzQuestions")
                        .HasForeignKey("QuizzId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Quizz");
                });

            modelBuilder.Entity("SkinCare_Data.Data.RatingsFeedback", b =>
                {
                    b.HasOne("SkinCare_Data.Data.Product", "Product")
                        .WithMany()
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("SkinCare_Data.Data.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Product");

                    b.Navigation("User");
                });

            modelBuilder.Entity("SkinCare_Data.Data.Report", b =>
                {
                    b.HasOne("SkinCare_Data.Data.Category", "Category")
                        .WithMany()
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("SkinCare_Data.Data.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Category");

                    b.Navigation("User");
                });

            modelBuilder.Entity("SkinCare_Data.Data.SkinType", b =>
                {
                    b.HasOne("SkinCare_Data.Data.SkinCareRoutine", "SkinCareRoutine")
                        .WithMany()
                        .HasForeignKey("RoutineId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("SkinCareRoutine");
                });

            modelBuilder.Entity("SkinCare_Data.Data.User", b =>
                {
                    b.HasOne("SkinCare_Data.Data.Role", "Role")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("SkinCare_Data.Data.SkinType", "SkinType")
                        .WithMany()
                        .HasForeignKey("SkinTypeId");

                    b.Navigation("Role");

                    b.Navigation("SkinType");
                });

            modelBuilder.Entity("SkinCare_Data.Data.Order", b =>
                {
                    b.Navigation("OrderDetails");
                });

            modelBuilder.Entity("SkinCare_Data.Data.Quizz", b =>
                {
                    b.Navigation("QuizzAnswers");

                    b.Navigation("QuizzQuestions");
                });

            modelBuilder.Entity("SkinCare_Data.Data.User", b =>
                {
                    b.Navigation("Orders");

                    b.Navigation("Quizzes");
                });
#pragma warning restore 612, 618
        }
    }
}
