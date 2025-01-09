using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace System
{
    /// <summary>
    /// Converts UUID values to byte arrays for database storage.
    /// </summary>
    public sealed class UUIDToBytesConverter : ValueConverter<UUID, byte[]>
    {
        private static readonly ConverterMappingHints defaultHints = new(size: 16);

        /// <summary>
        /// Initializes a new instance of the <see cref="UUIDToBytesConverter"/> class.
        /// </summary>
        public UUIDToBytesConverter() : this(null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UUIDToBytesConverter"/> class with the specified mapping hints.
        /// </summary>
        /// <param name="mappingHints">The converter mapping hints to merge with the default hints.</param>
        public UUIDToBytesConverter(ConverterMappingHints? mappingHints = null) : base(
                    convertFromProviderExpression: x => UUID.FromByteArray(x),
                    convertToProviderExpression: x => x.ToByteArray(),
                    mappingHints: defaultHints.With(mappingHints))
        {
        }
    }
}