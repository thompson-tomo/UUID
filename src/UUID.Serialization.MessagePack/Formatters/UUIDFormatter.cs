using MessagePack;
using MessagePack.Formatters;
using System.Buffers;

namespace System
{
    /// <summary>
    /// MessagePack formatter for UUID type.
    /// Handles serialization and deserialization of UUID values in MessagePack format.
    /// </summary>
    /// <remarks>
    /// This formatter serializes UUID as a 16-byte binary array where:
    /// - First 8 bytes contain the timestamp
    /// - Last 8 bytes contain the random value
    /// 
    /// The formatter supports both modern (.NET 6+) and legacy (.NET Framework) serialization methods
    /// through conditional compilation.
    /// </remarks>
    public class UUIDFormatter : IMessagePackFormatter<UUID>
    {
        /// <summary>
        /// Deserializes a UUID from MessagePack format.
        /// </summary>
        /// <param name="reader">The MessagePack reader containing the serialized data.</param>
        /// <param name="options">Serializer options for customizing the deserialization process.</param>
        /// <returns>A deserialized UUID instance.</returns>
        /// <exception cref="MessagePackSerializationException">
        /// Thrown when:
        /// - The input is null
        /// - The input length is not exactly 16 bytes
        /// - The MessagePack format is invalid
        /// </exception>
        /// <remarks>
        /// The deserialization process expects a binary format with exactly 16 bytes.
        /// The bytes are interpreted as two 8-byte segments for timestamp and random values.
        /// </remarks>
        public UUID Deserialize(ref MessagePackReader reader, MessagePackSerializerOptions options)
        {
            ReadOnlySequence<byte>? bin = reader.ReadBytes();

            if (bin == null)
            {
                throw new MessagePackSerializationException($"Unexpected msgpack code {MessagePackCode.Nil} ({MessagePackCode.ToFormatName(MessagePackCode.Nil)}) encountered.");
            }

            byte[] bytes = bin.Value.ToArray();
            if (bytes.Length != 16)
            {
                throw new MessagePackSerializationException($"UUID must be exactly 16 bytes long, but was {bytes.Length} bytes.");
            }

            ulong timestamp = BitConverter.ToUInt64(bytes, 0);
            ulong random = BitConverter.ToUInt64(bytes, 8);

            return new UUID(timestamp, random);
        }

        /// <summary>
        /// Serializes a UUID to MessagePack format.
        /// </summary>
        /// <param name="writer">The MessagePack writer to write the serialized data to.</param>
        /// <param name="value">The UUID value to serialize.</param>
        /// <param name="options">Serializer options for customizing the serialization process.</param>
        /// <remarks>
        /// The serialization process writes the UUID as a 16-byte binary array.
        /// Uses different approaches based on the target framework:
        /// - For .NET 6+ and .NET Standard 2.1: Uses Span-based BitConverter methods
        /// - For other frameworks: Uses traditional byte array methods with Buffer.BlockCopy
        /// </remarks>
        public void Serialize(ref MessagePackWriter writer, UUID value, MessagePackSerializerOptions options)
        {
            const int Length = 16;
            byte[] bytes = new byte[Length];

#if NETCOREAPP || NETSTANDARD2_1
            BitConverter.TryWriteBytes(bytes.AsSpan(0), value.Timestamp);
            BitConverter.TryWriteBytes(bytes.AsSpan(8), value.Random);
#else
            byte[] timestampBytes = BitConverter.GetBytes(value.Timestamp);
            byte[] randomBytes = BitConverter.GetBytes(value.Random);
            Buffer.BlockCopy(timestampBytes, 0, bytes, 0, 8);
            Buffer.BlockCopy(randomBytes, 0, bytes, 8, 8);
#endif

            writer.Write(bytes);
        }
    }
}