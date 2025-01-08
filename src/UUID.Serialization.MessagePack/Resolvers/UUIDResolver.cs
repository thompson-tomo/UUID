using MessagePack;
using MessagePack.Formatters;

namespace System
{
    /// <summary>
    /// MessagePack resolver for UUID type.
    /// Provides formatter resolution for UUID serialization and deserialization.
    /// </summary>
    /// <remarks>
    /// This resolver is implemented as a singleton to ensure consistent formatter usage
    /// across the application. It should be registered with MessagePackSerializer options
    /// before performing any serialization operations.
    /// 
    /// Example usage:
    /// <code>
    /// var options = MessagePackSerializerOptions.Standard
    ///     .WithResolver(CompositeResolver.Create(
    ///         UUIDResolver.Instance,
    ///         StandardResolver.Instance
    ///     ));
    /// </code>
    /// </remarks>
    public class UUIDResolver : IFormatterResolver
    {
        /// <summary>
        /// Singleton instance of the resolver.
        /// Use this instance when configuring MessagePack serialization options.
        /// </summary>
        /// <remarks>
        /// The instance is created as a static readonly field to ensure thread-safe
        /// singleton initialization and access.
        /// </remarks>
        public static IFormatterResolver Instance { get; } = new UUIDResolver();

        /// <summary>
        /// Private constructor to enforce singleton pattern.
        /// </summary>
        private UUIDResolver()
        {
        }

        /// <summary>
        /// Gets the formatter for type T.
        /// </summary>
        /// <typeparam name="T">Type to get formatter for.</typeparam>
        /// <returns>
        /// Returns an IMessagePackFormatter{T} instance if T is UUID;
        /// otherwise, returns null.
        /// </returns>
        /// <remarks>
        /// This method uses a generic cache to store and retrieve formatter instances,
        /// ensuring that only one formatter instance is created per type.
        /// The cache is implemented using a static generic class to provide type-safe
        /// caching without the need for type checking at runtime.
        /// </remarks>
        public IMessagePackFormatter<T> GetFormatter<T>()
        {
            return Cache<T>.Formatter;
        }

        /// <summary>
        /// Static cache class for storing formatter instances.
        /// </summary>
        /// <typeparam name="T">Type of the formatter to cache.</typeparam>
        /// <remarks>
        /// Uses type-level generic caching to ensure thread-safe, efficient
        /// formatter instance storage and retrieval.
        /// </remarks>
        private static class Cache<T>
        {
            /// <summary>
            /// Cached formatter instance for type T.
            /// </summary>
            /// <remarks>
            /// Initialized only once when the generic type is first used,
            /// providing lazy initialization with thread safety.
            /// </remarks>
            public static readonly IMessagePackFormatter<T> Formatter;

            /// <summary>
            /// Static constructor to initialize the formatter cache.
            /// </summary>
            static Cache()
            {
                if (typeof(T) == typeof(UUID))
                {
                    Formatter = (IMessagePackFormatter<T>)(object)new UUIDFormatter();
                }
            }
        }
    }
}