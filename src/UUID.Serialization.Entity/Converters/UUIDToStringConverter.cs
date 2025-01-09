using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace System
{
    /// <summary>
    /// Converts UUID values to strings for database storage.
    /// </summary>
    public sealed class UUIDToStringConverter : ValueConverter<UUID, string>
    {
        private static readonly ConverterMappingHints defaultHints = new(size: 32);

        /// <summary>
        /// Initializes a new instance of the <see cref="UUIDToStringConverter"/> class.
        /// </summary>
        public UUIDToStringConverter() : this(null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UUIDToStringConverter"/> class with the specified mapping hints.
        /// </summary>
        /// <param name="mappingHints">The converter mapping hints to merge with the default hints.</param>
        public UUIDToStringConverter(ConverterMappingHints? mappingHints = null) : base(
                    convertFromProviderExpression: x => UUID.Parse(x),
                    convertToProviderExpression: x => x.ToString(),
                    mappingHints: defaultHints.With(mappingHints))
        {
        }
    }
}