using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace System
{
    /// <summary>
    /// Converts UUID values to base64 strings for database storage.
    /// </summary>
    public sealed class UUIDToBase64Converter : ValueConverter<UUID, string>
    {
        private static readonly ConverterMappingHints defaultHints = new(size: 24);

        /// <summary>
        /// Initializes a new instance of the <see cref="UUIDToBase64Converter"/> class.
        /// </summary>
        public UUIDToBase64Converter() : this(null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UUIDToBase64Converter"/> class with the specified mapping hints.
        /// </summary>
        /// <param name="mappingHints">The converter mapping hints to merge with the default hints.</param>
        public UUIDToBase64Converter(ConverterMappingHints? mappingHints = null) : base(
                    convertFromProviderExpression: x => UUID.FromByteArray(Convert.FromBase64String(x)),
                    convertToProviderExpression: x => Convert.ToBase64String(x.ToByteArray()),
                    mappingHints: defaultHints.With(mappingHints))
        {
        }
    }
}