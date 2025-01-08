using MessagePack;
using MessagePack.Resolvers;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace UUIDSerializationMessagePackTests
{
    [MessagePackObject]
    public class NestedTestObject
    {
        [Key(0)]
        public UUID UUID { get; set; }
    }

    [MessagePackObject]
    public class ComplexTestObject
    {
        [Key(0)]
        public UUID Id { get; set; }

        [Key(1)]
        public string Name { get; set; }

        [Key(2)]
        public NestedTestObject Nested { get; set; }
    }

    public class UUIDFormatterTests
    {
        private readonly MessagePackSerializerOptions _options;

        public UUIDFormatterTests()
        {
            _options = MessagePackSerializerOptions.Standard
                .WithResolver(CompositeResolver.Create(
                    UUIDResolver.Instance,
                    StandardResolver.Instance
                ));
        }

        [Fact]
        public void Serialize_UUID_ReturnsCorrectBytes()
        {
            // Arrange
            UUID uuid = new();
            byte[] expectedTimestampBytes = BitConverter.GetBytes(uuid.Timestamp);
            byte[] expectedRandomBytes = BitConverter.GetBytes(uuid.Random);
            byte[] uuidBytes = new byte[16];
            Buffer.BlockCopy(expectedTimestampBytes, 0, uuidBytes, 0, 8);
            Buffer.BlockCopy(expectedRandomBytes, 0, uuidBytes, 8, 8);

            // Act
            byte[] result = MessagePackSerializer.Serialize(uuid, _options);

            // Assert
            // MessagePack format: [bin8(196) | length(16) | data(16 bytes)]
            Assert.Equal(18, result.Length); // 2 bytes header + 16 bytes data
            Assert.Equal(196, result[0]); // bin8 type
            Assert.Equal(16, result[1]); // length
            Assert.Equal(uuidBytes, result.AsSpan(2).ToArray()); // actual UUID bytes
        }

        [Fact]
        public void Deserialize_ValidBytes_ReturnsCorrectUUID()
        {
            // Arrange
            UUID uuid = new();
            byte[] bytes = MessagePackSerializer.Serialize(uuid, _options);

            // Act
            UUID result = MessagePackSerializer.Deserialize<UUID>(bytes, _options);

            // Assert
            Assert.Equal(uuid, result);
            Assert.Equal(uuid.Timestamp, result.Timestamp);
            Assert.Equal(uuid.Random, result.Random);
        }

        [Fact]
        public void Deserialize_NullValue_ThrowsException()
        {
            // Arrange
            byte[] nullBytes = MessagePackSerializer.Serialize<byte[]?>(null, _options);

            // Act & Assert
            Assert.Throws<MessagePackSerializationException>(() =>
                MessagePackSerializer.Deserialize<UUID>(nullBytes, _options));
        }

        [Fact]
        public void Deserialize_InvalidLength_ThrowsException()
        {
            // Arrange
            byte[] invalidBytes = MessagePackSerializer.Serialize(new byte[8], _options); // Wrong length

            // Act & Assert
            Assert.Throws<MessagePackSerializationException>(() =>
                MessagePackSerializer.Deserialize<UUID>(invalidBytes, _options));
        }

        [Fact]
        public void RoundTrip_MultipleUUIDs_PreservesValues()
        {
            // Arrange
            UUID[] uuids = new[]
            {
                new UUID(),
                new UUID(),
                new UUID()
            };

            // Act
            byte[] serialized = MessagePackSerializer.Serialize(uuids, _options);
            UUID[] deserialized = MessagePackSerializer.Deserialize<UUID[]>(serialized, _options);

            // Assert
            Assert.Equal(uuids.Length, deserialized.Length);
            for (int i = 0; i < uuids.Length; i++)
            {
                Assert.Equal(uuids[i], deserialized[i]);
                Assert.Equal(uuids[i].Timestamp, deserialized[i].Timestamp);
                Assert.Equal(uuids[i].Random, deserialized[i].Random);
            }
        }

        [Fact]
        public void Serialize_UUIDInComplexObject_SerializesCorrectly()
        {
            // Arrange
            ComplexTestObject complexObject = new()
            {
                Name = "Test",
                Id = new UUID(),
                Nested = new NestedTestObject { UUID = new UUID() }
            };

            // Act
            byte[] serialized = MessagePackSerializer.Serialize(complexObject, _options);
            ComplexTestObject deserialized = MessagePackSerializer.Deserialize<ComplexTestObject>(serialized, _options);

            // Assert
            Assert.NotNull(deserialized);
            Assert.Equal(complexObject.Id, deserialized.Id);
            Assert.Equal(complexObject.Name, deserialized.Name);
            Assert.Equal(complexObject.Nested.UUID, deserialized.Nested.UUID);
        }

        [Fact]
        public void Serialize_UUIDDictionary_SerializesCorrectly()
        {
            // Arrange
            Dictionary<UUID, string> dictionary = new()
            {
                { new UUID(), "Value1" },
                { new UUID(), "Value2" }
            };

            // Act
            byte[] serialized = MessagePackSerializer.Serialize(dictionary, _options);
            Dictionary<UUID, string> deserialized = MessagePackSerializer.Deserialize<Dictionary<UUID, string>>(serialized, _options);

            // Assert
            Assert.Equal(dictionary.Count, deserialized.Count);
            foreach (KeyValuePair<UUID, string> kvp in dictionary)
            {
                Assert.True(deserialized.ContainsKey(kvp.Key));
                Assert.Equal(kvp.Value, deserialized[kvp.Key]);
            }
        }

        [Fact]
        public void Deserialize_InvalidMessagePackFormat_ThrowsException()
        {
            // Arrange
            byte[] invalidFormat = new byte[] { 0xC1 }; // Invalid MessagePack format

            // Act & Assert
            Assert.Throws<MessagePackSerializationException>(() =>
                MessagePackSerializer.Deserialize<UUID>(invalidFormat, _options));
        }

        [Fact]
        public void Serialize_LargeNumberOfUUIDs_HandlesCorrectly()
        {
            // Arrange
            const int count = 1000;
            UUID[] uuids = Enumerable.Range(0, count).Select(_ => new UUID()).ToArray();

            // Act
            byte[] serialized = MessagePackSerializer.Serialize(uuids, _options);
            UUID[] deserialized = MessagePackSerializer.Deserialize<UUID[]>(serialized, _options);

            // Assert
            Assert.Equal(count, deserialized.Length);
            for (int i = 0; i < count; i++)
            {
                Assert.Equal(uuids[i], deserialized[i]);
            }
        }

        [Theory]
        [InlineData(0UL, 0UL)]
        [InlineData(ulong.MaxValue, ulong.MaxValue)]
        [InlineData(ulong.MinValue, ulong.MinValue)]
        public void RoundTrip_SpecialValues_PreservesValues(ulong timestamp, ulong random)
        {
            // Arrange
            UUID uuid = new(timestamp, random);

            // Act
            byte[] serialized = MessagePackSerializer.Serialize(uuid, _options);
            UUID deserialized = MessagePackSerializer.Deserialize<UUID>(serialized, _options);

            // Assert
            Assert.Equal(uuid, deserialized);
            Assert.Equal(timestamp, deserialized.Timestamp);
            Assert.Equal(random, deserialized.Random);
        }
    }
}