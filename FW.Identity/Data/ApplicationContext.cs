using System;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using FW.Identity.Data.Models;
using Microsoft.AspNetCore.Identity;
using Npgsql;
using System.ComponentModel.DataAnnotations.Schema;

namespace FW.Identity.Data
{
    public class ApplicationContext : IdentityDbContext<ApplicationUser, ApplicationRole, Guid>
    {
        public ApplicationContext(DbContextOptions<ApplicationContext> options)
            : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.HasPostgresExtension("uuid-ossp");

            // Cопостовление класса с таблицей(изменение имени таблицы в БД)
            modelBuilder.Entity<ApplicationUser>(entity => entity.ToTable(name: "Users"));
            modelBuilder.Entity<ApplicationRole>(entity => entity.ToTable(name: "Roles"));
            modelBuilder.Entity<IdentityUserRole<Guid>>(entity => entity.ToTable(name: "UserRoles"));
            modelBuilder.Entity<IdentityUserClaim<Guid>>(entity => entity.ToTable(name: "UserClaim"));
            modelBuilder.Entity<IdentityUserLogin<Guid>>(entity => entity.ToTable("UserLogins"));
            modelBuilder.Entity<IdentityUserToken<Guid>>(entity => entity.ToTable("UserTokens"));
            modelBuilder.Entity<IdentityRoleClaim<Guid>>(entity => entity.ToTable("RoleClaims"));

            // настройка Свойств таблицы Users

            // Переопределение первичного ключа (т.к. тип Id Guid, то для переопределения необходимо изменить тип на необходимый, например string)
            modelBuilder.Entity<ApplicationUser>().HasKey(p => p.Id);
            //modelBuilder.Entity<ApplicationUser>().HasKey(p => p.PhoneNumber);
            //modelBuilder.Entity<ApplicationUser>().HasKey(p => new { p.Id, p.PhoneNumber }); // составной ключ

            // Исключение свойства (не будет добавлено в таблицу)
            modelBuilder.Entity<ApplicationUser>().Ignore(p => p.ForTestIgnore);

            modelBuilder.Entity<ApplicationUser>()
                .Property(p => p.Id)
                .HasColumnType("uuid")
                .HasDefaultValueSql("uuid_generate_v4()")
                .IsRequired();

            modelBuilder.Entity<ApplicationUser>()
                .Property(p => p.EmailConfirmed)
                .HasDefaultValue(false);

            modelBuilder.Entity<ApplicationUser>()
                .Property(p => p.PhoneNumberConfirmed)
                .HasDefaultValue(false);

            modelBuilder.Entity<ApplicationUser>()
                .Property(p => p.TwoFactorEnabled)
                .HasDefaultValue(false);

            modelBuilder.Entity<ApplicationUser>()
                .Property(p => p.LockoutEnabled)
                .HasDefaultValue(false);

            modelBuilder.Entity<ApplicationUser>()
                .Property(p => p.AccessFailedCount)
                .HasDefaultValue(0);

            modelBuilder.Entity<ApplicationUser>()
                .Property(p => p.UserName)
                .HasColumnName("FirstName")   // Сопоставление свойств(изменение имени столбца в БД)
                .IsRequired();                // Определение NOT NULL свойства

            modelBuilder.Entity<ApplicationUser>()
                .Property(p => p.PasswordHash)
                .IsRequired();

            modelBuilder.Entity<ApplicationUser>()
                .Property(p => p.Email)
                .IsRequired();

            modelBuilder.Entity<ApplicationUser>().Property(p => p.LastName)
                .IsRequired(false)
                .HasColumnType("character varying(256)")
                .IsUnicode(false);


            // ToDo: реализовать роли
            // 1. повар (доступ к рецептам),
            // 2. менеджер склада (доступ к складу продуктов),
            // 3. администратор (полный доступ)

            // Инициализация ролей
            Guid AdminRoleId = Guid.Parse("8624d8bb-b590-4ff6-9820-81bd7168ad7d");
            Guid UserRoleId = Guid.Parse("58b68001-05a9-4a8a-b051-8bb8c54b9d26");
            modelBuilder.Entity<ApplicationRole>().HasData(new
            {
                Id = AdminRoleId,
                Name = "admin",
                NormalizedName = "ADMIN"
            });
            modelBuilder.Entity<ApplicationRole>().HasData(new
            {
                Id = UserRoleId,
                Name = "user",
                NormalizedName = "USER"
            });


            // Инициализация пользователей
            // Normalize: нормализация не позволяет регистрировать имена пользователей и т.д., которые отличаются только регистром.
            var admin = new ApplicationUser
            {
                Id = Guid.Parse("01010101-0101-0101-0101-010101010101"),
                UserName = "admin",
                LastName = "",
                NormalizedUserName = "ADMIN",
                Email = "admin@mail.ru",
                NormalizedEmail = "ADMIN@MAIL.RU",
                PhoneNumber = "89000000000",
                EmailConfirmed = true,
                PhoneNumberConfirmed = true,
                TwoFactorEnabled = false,
                SecurityStamp = new Guid().ToString(),
            };
            admin.PasswordHash = new PasswordHasher<ApplicationUser>().HashPassword(admin, "123456");

            var ivan = new ApplicationUser
            {
                Id = Guid.Parse("00000000-0000-0000-0000-000000000001"),
                UserName = "Ivan",
                LastName = "",
                NormalizedUserName = "IVAN",
                Email = "Ivan@mail.ru",
                NormalizedEmail = "IVAN@MAIL.RU",
                PhoneNumber = "89000000001",
                EmailConfirmed = true,
                PhoneNumberConfirmed = true,
                TwoFactorEnabled = false,
                SecurityStamp = new Guid().ToString(),
            };
            ivan.PasswordHash = new PasswordHasher<ApplicationUser>().HashPassword(ivan, "i123456789");

            var haruko = new ApplicationUser
            {
                Id = Guid.Parse("00000000-0000-0000-0000-000000000002"),
                UserName = "Haruko",
                LastName = "",
                NormalizedUserName = "HARUKO",
                Email = "Haruko@mail.jp",
                NormalizedEmail = "HARUKO@MAIL.RU",
                PhoneNumber = "89000000002",
                EmailConfirmed = true,
                PhoneNumberConfirmed = true,
                TwoFactorEnabled = false,
                SecurityStamp = new Guid().ToString(),
            };
            haruko.PasswordHash = new PasswordHasher<ApplicationUser>().HashPassword(haruko, "h123456789");

            var gabriele = new ApplicationUser
            {
                Id = Guid.Parse("00000000-0000-0000-0000-000000000003"),
                UserName = "Gabriele",
                LastName = "",
                NormalizedUserName = "GABRIELE",
                Email = "Gabriele@mail.it",
                NormalizedEmail = "GABRIELE@MAIL.RU",
                PhoneNumber = "89000000003",
                EmailConfirmed = true,
                PhoneNumberConfirmed = true,
                TwoFactorEnabled = false,
                SecurityStamp = new Guid().ToString(),
            };
            gabriele.PasswordHash = new PasswordHasher<ApplicationUser>().HashPassword(gabriele, "g123456789");


            // Добавление пользователей
            modelBuilder.Entity<ApplicationUser>().HasData(admin);
            modelBuilder.Entity<ApplicationUser>().HasData(ivan);
            modelBuilder.Entity<ApplicationUser>().HasData(haruko);
            modelBuilder.Entity<ApplicationUser>().HasData(gabriele);


            // Добавление ролей
            modelBuilder.Entity<IdentityUserRole<Guid>>().HasData(new IdentityUserRole<Guid>
            {
                RoleId = AdminRoleId,
                UserId = admin.Id
            });
            modelBuilder.Entity<IdentityUserRole<Guid>>().HasData(new IdentityUserRole<Guid>
            {
                RoleId = UserRoleId,
                UserId = ivan.Id
            });
            modelBuilder.Entity<IdentityUserRole<Guid>>().HasData(new IdentityUserRole<Guid>
            {
                RoleId = UserRoleId,
                UserId = haruko.Id
            });
            modelBuilder.Entity<IdentityUserRole<Guid>>().HasData(new IdentityUserRole<Guid>
            {
                RoleId = UserRoleId,
                UserId = gabriele.Id
            });
        }
    }
}
