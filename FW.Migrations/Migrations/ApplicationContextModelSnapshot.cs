﻿// <auto-generated />
using System;
using FW.EntityFramework;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
#nullable disable

namespace FW.Migrations
{
    [DbContext(typeof(ApplicationContext))]
    partial class ApplicationContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("FoodWarehouse.Infrastructure.Data.Categories", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime>("ModifiedOn")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");
                    b.Property<Guid>("UserId")
                     .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.ToTable("Categories");
                });

            modelBuilder.Entity("FoodWarehouse.Infrastructure.Data.ChangesProducts", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime>("ModifiedOn")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid>("ProductId")
                        .HasColumnType("uuid");

                    b.Property<double>("Quantity")
                        .HasColumnType("double precision");
                    b.Property<Guid>("UserId")
                     .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("ProductId");

                    b.ToTable("ChangesProducts");
                });

            modelBuilder.Entity("FoodWarehouse.Infrastructure.Data.Dishes", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("ModifiedOn")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");
                    b.Property<Guid>("UserId")
                     .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.ToTable("Dishes");
                });

            modelBuilder.Entity("FoodWarehouse.Infrastructure.Data.Ingredients", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime>("ModifiedOn")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");
                    b.Property<Guid>("UserId")
                     .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.ToTable("Ingredients");
                });

            modelBuilder.Entity("FoodWarehouse.Infrastructure.Data.Products", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid>("CategoryId")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("ExpirationDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid>("IngredientId")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("ModifiedOn")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<double>("Quantity")
                        .HasColumnType("double precision");

                    b.Property<Guid>("WarehouseId")
                        .HasColumnType("uuid");
                    b.Property<Guid>("UserId")
                     .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("CategoryId");

                    b.HasIndex("IngredientId");

                    b.HasIndex("WarehouseId");

                    b.ToTable("Products");
                });

            modelBuilder.Entity("FoodWarehouse.Infrastructure.Data.Recipes", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid>("DishesId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("IngredientId")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("ModifiedOn")
                        .HasColumnType("timestamp with time zone");

                    b.Property<double>("Quantity")
                        .HasColumnType("double precision");
                    b.Property<Guid>("UserId")
                     .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("DishesId");

                    b.HasIndex("IngredientId");

                    b.ToTable("Recipes");
                });

            //modelBuilder.Entity("FoodWarehouse.Infrastructure.Data.Users", b =>
            //    {
            //        b.Property<Guid>("Id")
            //            .ValueGeneratedOnAdd()
            //            .HasColumnType("uuid");

            //        b.Property<string>("Email")
            //            .IsRequired()
            //            .HasColumnType("text");

            //        b.Property<DateTime>("ModifiedOn")
            //            .HasColumnType("timestamp with time zone");

            //        b.Property<string>("Name")
            //            .IsRequired()
            //            .HasColumnType("text");

            //        b.Property<string>("Password")
            //            .IsRequired()
            //            .HasColumnType("text");
            //       // b.Property<string>("Token")
            //       //    .IsRequired()
            //       //    .HasColumnType("text");

            //        b.HasKey("Id");

            //        b.ToTable("Users");
            //    });

            modelBuilder.Entity("FoodWarehouse.Infrastructure.Data.Warehouses", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("ModifiedOn")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<Guid>("UserId")
                       .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Warehouses");
                });

            modelBuilder.Entity("FoodWarehouse.Infrastructure.Data.ChangesProducts", b =>
                {
                    b.HasOne("FoodWarehouse.Infrastructure.Data.Products", "Products")
                        .WithMany()
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Products");
                });

            modelBuilder.Entity("FoodWarehouse.Infrastructure.Data.Products", b =>
                {
                    b.HasOne("FoodWarehouse.Infrastructure.Data.Categories", "Categories")
                        .WithMany()
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("FoodWarehouse.Infrastructure.Data.Ingredients", "Ingredients")
                        .WithMany()
                        .HasForeignKey("IngredientId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("FoodWarehouse.Infrastructure.Data.Warehouses", "Warehouses")
                        .WithMany()
                        .HasForeignKey("WarehouseId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Categories");

                    b.Navigation("Ingredients");

                    b.Navigation("Warehouses");
                });

            modelBuilder.Entity("FoodWarehouse.Infrastructure.Data.Recipes", b =>
                {
                    b.HasOne("FoodWarehouse.Infrastructure.Data.Dishes", "Dishes")
                        .WithMany()
                        .HasForeignKey("DishesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("FoodWarehouse.Infrastructure.Data.Ingredients", "Ingredients")
                        .WithMany()
                        .HasForeignKey("IngredientId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Dishes");

                    b.Navigation("Ingredients");
                });

            //modelBuilder.Entity("FoodWarehouse.Infrastructure.Data.Warehouse", b =>
            //    {
            //        b.HasOne("FoodWarehouse.Infrastructure.Data.Users", "Users")
            //            .WithMany()
            //            .HasForeignKey("UserId")
            //            .OnDelete(DeleteBehavior.Cascade)
            //            .IsRequired();

            //        b.Navigation("Users");
            //    });
#pragma warning restore 612, 618
        }
    }
}