using Microsoft.EntityFrameworkCore;
using System.Reflection;
using Zen.Domain.Utilities;
using Zen.Infrastructure.Data.Security;

namespace Zen.Infrastructure.Data;

/// <summary>
/// A base DbContext for Zen applications.
/// Microservices can inherit from this context to gain common configurations,
/// such as optimistic concurrency support and shared conventions.
/// </summary>
public abstract class ZenDbContext(DbContextOptions options) : DbContext(options)
{
    public static IColumnEncryptionService? ColumnEncryptionService { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        if (ColumnEncryptionService != null)
        {
            foreach (var entity in modelBuilder.Model.GetEntityTypes())
            {
                // Find properties with the [Encrypted] attribute.
                var properties = entity.ClrType.GetProperties()
                    .Where(p => p.GetCustomAttribute<EncryptedAttribute>() != null);

                foreach (var property in properties)
                {
                    var propertyBuilder = modelBuilder.Entity(entity.ClrType).Property(property.Name);
                    if (property.PropertyType == typeof(string))
                    {
                        var converter = new EncryptedStringConverter(ColumnEncryptionService);
                        propertyBuilder.HasConversion(converter);
                    }
                    else if (property.PropertyType == typeof(decimal))
                    {
                        var converter = new EncryptedDecimalConverter(ColumnEncryptionService);
                        propertyBuilder.HasConversion(converter);
                        propertyBuilder.HasColumnType("nvarchar(max)");
                    }
                }
            }
        }
    }
}
