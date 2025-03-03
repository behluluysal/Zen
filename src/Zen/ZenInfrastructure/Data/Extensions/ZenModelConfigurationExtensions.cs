using Microsoft.EntityFrameworkCore;
using System.Reflection;
using Zen.Domain;
using Zen.Domain.Outbox;
using Zen.Domain.Utilities;
using Zen.Infrastructure.Data.Security;

namespace Zen.Infrastructure.Data.Extensions;

internal static class ZenModelConfigurationExtensions
{
    public static void ConfigureZenModel(this ModelBuilder modelBuilder, IColumnEncryptionService? columnEncryptionService)
    {
        if (columnEncryptionService != null)
        {
            foreach (var entity in modelBuilder.Model.GetEntityTypes())
            {
                // Configure encrypted properties.
                var properties = entity.ClrType.GetProperties()
                    .Where(p => p.GetCustomAttribute<EncryptedAttribute>() != null);

                foreach (var property in properties)
                {
                    var propertyBuilder = modelBuilder.Entity(entity.ClrType).Property(property.Name);
                    if (property.PropertyType == typeof(string))
                    {
                        propertyBuilder.HasConversion(new EncryptedStringConverter(columnEncryptionService));
                    }
                    else if (property.PropertyType == typeof(decimal))
                    {
                        propertyBuilder.HasConversion(new EncryptedDecimalConverter(columnEncryptionService));
                        propertyBuilder.HasColumnType("nvarchar(max)");
                    }
                }
            }
        }

        // Configure optimistic concurrency.
        foreach (var entity in modelBuilder.Model.GetEntityTypes())
        {
            if (typeof(IConcurrencyAware).IsAssignableFrom(entity.ClrType))
            {
                modelBuilder.Entity(entity.ClrType)
                    .Property("RowVersion")
                    .IsRowVersion();
            }
        }

        // Configure outbox messages
        modelBuilder.Entity<OutboxMessage>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(om => om.Type).IsRequired();
            entity.Property(om => om.Content).IsRequired();
            entity.Property(om => om.OccurredOnUtc).IsRequired();
            entity.Property(om => om.ProcessedOnUtc).IsRequired(false);
            entity.Property(om => om.Error).IsRequired(false);
            entity.Property(om => om.RetryCount).IsRequired();
        });
    }
}

