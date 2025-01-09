using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace System
{
    /// <summary>
    /// Extension methods for configuring UUID conversions in Entity Framework Core.
    /// </summary>
    public static class ModelBuilderExtensions
    {
        /// <summary>
        /// Configures all UUID properties to use binary storage format (16 bytes).
        /// </summary>
        /// <param name="modelBuilder">The model builder instance.</param>
        /// <returns>The model builder instance for chaining.</returns>
        public static ModelBuilder UseUUIDToBytesConverter(this ModelBuilder modelBuilder)
        {
            foreach (IMutableEntityType entityType in modelBuilder.Model.GetEntityTypes())
            {
                IEnumerable<IMutableProperty> properties = entityType.GetProperties()
                    .Where(p => p.ClrType == typeof(UUID));

                foreach (IMutableProperty? property in properties)
                {
                    property.SetValueConverter(new UUIDToBytesConverter());
                }
            }

            return modelBuilder;
        }

        /// <summary>
        /// Configures all UUID properties to use string storage format (32 characters).
        /// </summary>
        /// <param name="modelBuilder">The model builder instance.</param>
        /// <returns>The model builder instance for chaining.</returns>
        public static ModelBuilder UseUUIDToStringConverter(this ModelBuilder modelBuilder)
        {
            foreach (IMutableEntityType entityType in modelBuilder.Model.GetEntityTypes())
            {
                IEnumerable<IMutableProperty> properties = entityType.GetProperties()
                    .Where(p => p.ClrType == typeof(UUID));

                foreach (IMutableProperty? property in properties)
                {
                    property.SetValueConverter(new UUIDToStringConverter());
                }
            }

            return modelBuilder;
        }

        /// <summary>
        /// Configures all UUID properties to use base64 storage format (24 characters).
        /// </summary>
        /// <param name="modelBuilder">The model builder instance.</param>
        /// <returns>The model builder instance for chaining.</returns>
        public static ModelBuilder UseUUIDToBase64Converter(this ModelBuilder modelBuilder)
        {
            foreach (IMutableEntityType entityType in modelBuilder.Model.GetEntityTypes())
            {
                IEnumerable<IMutableProperty> properties = entityType.GetProperties()
                    .Where(p => p.ClrType == typeof(UUID));

                foreach (IMutableProperty? property in properties)
                {
                    property.SetValueConverter(new UUIDToBase64Converter());
                }
            }

            return modelBuilder;
        }
    }
}