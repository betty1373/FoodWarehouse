using FW.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Linq;
using System.Linq.Expressions;

namespace FW.EntityFramework
{
    /// <summary>
    ///  Служебный класс для работы с контекстом и соединением к БД
    /// </summary>
    public class ApplicationContext : DbContext
    {
        public DbSet<Warehouses> Warehouses { get; set; }
        public DbSet<Categories> Categories { get; set; }
        public DbSet<Products> Products { get; set; }
        public DbSet<ChangesProducts> ChangesProducts { get; set; }
        public DbSet<Dishes> Dishes { get; set; }
        public DbSet<Recipes> Recipes { get; set; }
        public DbSet<Ingredients> Ingredients { get; set; }
        
        public ApplicationContext() { }
        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
        {
        }
       
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                entityType.AddProperty("ModifiedOn", typeof(DateTime));
                entityType.AddProperty("UserId",typeof(Guid));
              //  entityType.AddProperty("IsActive", typeof(bool?));
                modelBuilder.Entity(entityType.Name).Property<bool?>("IsActive").HasDefaultValue(true);
            }
            modelBuilder.Model.GetEntityTypes().ToList()
                .ForEach(entityType =>
                {
                    modelBuilder.Entity(entityType.ClrType).Property<bool?>("IsActive");
                    var parameter = Expression.Parameter(entityType.ClrType, "e");
                    var body = Expression.Equal(
                        Expression.Call(typeof(EF), nameof(EF.Property), new[] { typeof(bool) }, parameter, Expression.Constant("IsActive")),
                    Expression.Constant(true));
                    modelBuilder.Entity(entityType.ClrType).HasQueryFilter(Expression.Lambda(body, parameter));
                });
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            foreach (var entityEntry in ChangeTracker.Entries())
            {
                switch(entityEntry.State)
                {
                    case EntityState.Added :
                        entityEntry.Property("ModifiedOn").CurrentValue = DateTime.UtcNow;
                        break;
                    case EntityState.Modified:
                        entityEntry.Property("ModifiedOn").CurrentValue = DateTime.UtcNow;
                        break;
                    case EntityState.Deleted:
                        entityEntry.Property("ModifiedOn").CurrentValue = DateTime.UtcNow;
                        entityEntry.State = EntityState.Modified;
                        entityEntry.Property("IsActive").CurrentValue = false;
                        break;
                }                
            }
            return base.SaveChangesAsync(cancellationToken);
        }
    }

    /// <summary>
    ///  Класс для внешнего взаимодействия с БД
    /// </summary>
    //public class DBworker
    //{
    //    /// <summary>
    //    ///  Пример функции вывода данных
    //    /// </summary>
    //    public void OutAllProducts()
    //    {
    //        using (ApplicationContext? context = new ApplicationContext())
    //        {
    //            Console.WriteLine("___Products___");
    //            foreach (Products i in context.Products)
    //            {
    //                Console.WriteLine($"{i.Name} - {i.Quantity}");
    //            }
    //        }
    //    }
   // }
}
