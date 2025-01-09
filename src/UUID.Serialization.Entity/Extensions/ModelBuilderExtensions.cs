using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace System
{
    /// <summary>
    /// Extension methods for configuring UUID value converters in Entity Framework Core.
    /// </summary>
    public static class ModelBuilderExtensions
    {
        /// <summary>
        /// Configures the model builder to use binary storage for all UUID properties.
        /// </summary>
        /// <param name="configurationBuilder">The model configuration builder.</param>
        /// <returns>The model configuration builder for chaining.</returns>
        public static ModelConfigurationBuilder UseUUIDAsBinary(this ModelConfigurationBuilder configurationBuilder)
        {
            configurationBuilder
                .Properties<UUID>()
                .HaveConversion<UUIDToBytesConverter>();

            return configurationBuilder;
        }

        /// <summary>
        /// Configures the model builder to use string storage for all UUID properties.
        /// </summary>
        /// <param name="configurationBuilder">The model configuration builder.</param>
        /// <returns>The model configuration builder for chaining.</returns>
        public static ModelConfigurationBuilder UseUUIDAsString(this ModelConfigurationBuilder configurationBuilder)
        {
            configurationBuilder
                .Properties<UUID>()
                .HaveConversion<UUIDToStringConverter>();

            return configurationBuilder;
        }

        /// <summary>
        /// Configures the model builder to use base64 string storage for all UUID properties.
        /// </summary>
        /// <param name="configurationBuilder">The model configuration builder.</param>
        /// <returns>The model configuration builder for chaining.</returns>
        public static ModelConfigurationBuilder UseUUIDAsBase64(this ModelConfigurationBuilder configurationBuilder)
        {
            configurationBuilder
                .Properties<UUID>()
                .HaveConversion<UUIDToBase64Converter>();

            return configurationBuilder;
        }

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