using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace System
{
    /// <summary>
    /// Extension methods for configuring UUID conversions in Entity Framework Core.
    /// </summary>
    public static class PropertyBuilderExtensions
    {
        /// <summary>
        /// Configures a specific property to use binary storage for UUID values.
        /// </summary>
        /// <typeparam name="TEntity">The entity type containing the property.</typeparam>
        /// <param name="propertyBuilder">The property builder.</param>
        /// <returns>The property builder for chaining.</returns>
        public static PropertyBuilder<UUID> UseUUIDAsBinary<TEntity>(this PropertyBuilder<UUID> propertyBuilder)
        {
            return propertyBuilder.HasConversion<UUIDToBytesConverter>();
        }

        /// <summary>
        /// Configures a specific property to use string storage for UUID values.
        /// </summary>
        /// <typeparam name="TEntity">The entity type containing the property.</typeparam>
        /// <param name="propertyBuilder">The property builder.</param>
        /// <returns>The property builder for chaining.</returns>
        public static PropertyBuilder<UUID> UseUUIDAsString<TEntity>(this PropertyBuilder<UUID> propertyBuilder)
        {
            return propertyBuilder.HasConversion<UUIDToStringConverter>();
        }

        /// <summary>
        /// Configures a specific property to use base64 string storage for UUID values.
        /// </summary>
        /// <typeparam name="TEntity">The entity type containing the property.</typeparam>
        /// <param name="propertyBuilder">The property builder.</param>
        /// <returns>The property builder for chaining.</returns>
        public static PropertyBuilder<UUID> UseUUIDAsBase64<TEntity>(this PropertyBuilder<UUID> propertyBuilder)
        {
            return propertyBuilder.HasConversion<UUIDToBase64Converter>();
        }
    }
}