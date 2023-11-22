using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DbViewerExample.EntityFramework;

public class EntityFrameworkDbContext : DbContext
{
    public const string ConnectionString = "Host=localhost;Database=my_db;Username=postgres;Password=postgres";

    public static readonly Type[] EntityTypes = typeof(EntityFrameworkDbContext)
        .GetProperties(BindingFlags.Public | BindingFlags.Instance)
        .Select(x => x.PropertyType)
        .Where(x => x.IsGenericType && x.GetGenericTypeDefinition() == typeof(DbSet<>))
        .Select(x => x.GetGenericArguments()[0])
        .ToArray();

    public DbSet<TestTable> Tests { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql(ConnectionString);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        foreach (var type in EntityTypes)
        {
            var entity = modelBuilder.Entity(type);

            var keyProperties = ExtractPropertiesWithAttribute<IdentityAttribute>(entity);
            entity.HasKey(keyProperties.Select(x => x.Name).ToArray());


            var indexedProperties = ExtractPropertiesWithAttribute<IndexedAttribute>(entity);
            if (indexedProperties.Any())
                entity.HasIndex(indexedProperties.Select(x => x.Name).ToArray());
        }
    }

    private static PropertyInfo[] ExtractPropertiesWithAttribute<TAttribute>(EntityTypeBuilder entityTypeBuilder)
        where TAttribute : Attribute
    {
        return entityTypeBuilder.Metadata.ClrType
            .GetProperties(BindingFlags.Public | BindingFlags.Instance)
            .Where(x => x.CanRead && x.CanWrite && x.GetCustomAttributes<TAttribute>().Any())
            .ToArray();
    }
}