using Microsoft.EntityFrameworkCore;

namespace System
{
    /// <summary>
    /// Extension methods for configuring UUID conversions in Entity Framework Core.
    /// </summary>
    public static class ModelConfigurationBuilderExtensions
    {
        /// <summary>
        /// Configures all UUID properties to use binary storage format (16 bytes).
        /// </summary>
        /// <param name="configurationBuilder">The model configuration builder instance.</param>
        /// <returns>The model configuration builder instance for chaining.</returns>
        public static ModelConfigurationBuilder UseUUIDAsBinary(this ModelConfigurationBuilder configurationBuilder)
        {
            configurationBuilder
                .Properties<UUID>()
                .HaveConversion<UUIDToBytesConverter>();

            return configurationBuilder;
        }

        /// <summary>
        /// Configures all UUID properties to use string storage format (32 characters).
        /// </summary>
        /// <param name="configurationBuilder">The model configuration builder instance.</param>
        /// <returns>The model configuration builder instance for chaining.</returns>
        public static ModelConfigurationBuilder UseUUIDAsString(this ModelConfigurationBuilder configurationBuilder)
        {
            configurationBuilder
                .Properties<UUID>()
                .HaveConversion<UUIDToStringConverter>();

            return configurationBuilder;
        }

        /// <summary>
        /// Configures all UUID properties to use base64 storage format (24 characters).
        /// </summary>
        /// <param name="configurationBuilder">The model configuration builder instance.</param>
        /// <returns>The model configuration builder instance for chaining.</returns>
        public static ModelConfigurationBuilder UseUUIDAsBase64(this ModelConfigurationBuilder configurationBuilder)
        {
            configurationBuilder
                .Properties<UUID>()
                .HaveConversion<UUIDToBase64Converter>();

            return configurationBuilder;
        }
    }
}