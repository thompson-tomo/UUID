using Dapper;
using Microsoft.Data.SqlClient;
using System;
using System.Data;
using Xunit;

namespace UUIDSerializationDapperTests
{
    public class Base64UUIDHandlerTests
    {
        private readonly Base64UUIDHandler _handler;

        public Base64UUIDHandlerTests()
        {
            _handler = new Base64UUIDHandler();
            SqlMapper.AddTypeHandler(_handler);
        }

        [Fact]
        public void SetValue_ShouldSetCorrectDbType()
        {
            SqlParameter parameter = new();
            UUID uuid = UUID.New();

            _handler.SetValue(parameter, uuid);

            Assert.Equal(DbType.StringFixedLength, parameter.DbType);
            Assert.Equal(uuid.ToBase64(), parameter.Value);
        }

        [Fact]
        public void SetValue_ShouldHandleEmpty()
        {
            SqlParameter parameter = new();

            _handler.SetValue(parameter, UUID.Empty);

            Assert.Equal("AAAAAAAAAAAAAAAAAAAAAA==", parameter.Value);
        }

        [Fact]
        public void Parse_ShouldHandleValidBase64()
        {
            UUID originalUuid = UUID.New();
            string base64 = originalUuid.ToBase64();

            UUID result = _handler.Parse(base64);

            Assert.Equal(originalUuid, result);
        }

        [Fact]
        public void Parse_ShouldHandleNull()
        {
            Assert.Throws<ArgumentNullException>(() => _handler.Parse(null));
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        public void Parse_ShouldHandleEmptyOrWhitespace(string value)
        {
            Assert.Throws<FormatException>(() => _handler.Parse(value));
        }

        [Fact]
        public void Parse_ShouldHandleDBNull()
        {
            Assert.Throws<InvalidCastException>(() => _handler.Parse(DBNull.Value));
        }

        [Theory]
        [InlineData("invalid-base64")]
        [InlineData("123")]
        [InlineData("====")]
        public void Parse_ShouldHandleInvalidBase64(string invalidValue)
        {
            Assert.Throws<FormatException>(() => _handler.Parse(invalidValue));
        }
    }

    public class BinaryUUIDHandlerTests
    {
        private readonly BinaryUUIDHandler _handler;

        public BinaryUUIDHandlerTests()
        {
            _handler = new BinaryUUIDHandler();
            SqlMapper.AddTypeHandler(_handler);
        }

        [Fact]
        public void SetValue_ShouldSetCorrectDbType()
        {
            SqlParameter parameter = new();
            UUID uuid = UUID.New();

            _handler.SetValue(parameter, uuid);

            Assert.Equal(DbType.Binary, parameter.DbType);
            Assert.Equal(16, ((byte[])parameter.Value).Length);
        }

        [Fact]
        public void SetValue_ShouldHandleEmpty()
        {
            SqlParameter parameter = new();

            _handler.SetValue(parameter, UUID.Empty);

            byte[] bytes = (byte[])parameter.Value;
            Assert.Equal(new byte[16], bytes);
        }

        [Fact]
        public void Parse_ShouldHandleValidByteArray()
        {
            UUID originalUuid = UUID.New();
            byte[] bytes = originalUuid.ToByteArray();

            UUID result = _handler.Parse(bytes);

            Assert.Equal(originalUuid, result);
        }

        [Fact]
        public void Parse_ShouldHandleNull()
        {
            Assert.Throws<ArgumentNullException>(() => _handler.Parse(null));
        }

        [Fact]
        public void Parse_ShouldHandleDBNull()
        {
            Assert.Throws<InvalidCastException>(() => _handler.Parse(DBNull.Value));
        }

        [Theory]
        [InlineData(8)]  // Too short
        [InlineData(24)] // Too long
        public void Parse_ShouldHandleInvalidByteArrayLength(int length)
        {
            byte[] invalidBytes = new byte[length];
            Assert.Throws<ArgumentException>(() => _handler.Parse(invalidBytes));
        }
    }

    public class StringUUIDHandlerTests
    {
        private readonly StringUUIDHandler _handler;

        public StringUUIDHandlerTests()
        {
            _handler = new StringUUIDHandler();
            SqlMapper.AddTypeHandler(_handler);
        }

        [Fact]
        public void SetValue_ShouldSetCorrectDbType()
        {
            SqlParameter parameter = new();
            UUID uuid = UUID.New();

            _handler.SetValue(parameter, uuid);

            Assert.Equal(DbType.StringFixedLength, parameter.DbType);
            Assert.Equal(uuid.ToString(), parameter.Value);
        }

        [Fact]
        public void SetValue_ShouldHandleEmpty()
        {
            SqlParameter parameter = new();

            _handler.SetValue(parameter, UUID.Empty);

            Assert.Equal("00000000000000000000000000000000", parameter.Value);
        }

        [Fact]
        public void Parse_ShouldHandleValidString()
        {
            UUID originalUuid = UUID.New();
            string str = originalUuid.ToString();

            UUID result = _handler.Parse(str);

            Assert.Equal(originalUuid, result);
        }

        [Fact]
        public void Parse_ShouldHandleNull()
        {
            Assert.Throws<ArgumentNullException>(() => _handler.Parse(null));
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        public void Parse_ShouldHandleEmptyOrWhitespace(string value)
        {
            Assert.Throws<FormatException>(() => _handler.Parse(value));
        }

        [Fact]
        public void Parse_ShouldHandleDBNull()
        {
            Assert.Throws<InvalidCastException>(() => _handler.Parse(DBNull.Value));
        }

        [Theory]
        [InlineData("invalid-uuid")]
        [InlineData("123")]
        [InlineData("00000000-0000-0000-0000-00000000000Z")] // Invalid character
        public void Parse_ShouldHandleInvalidString(string invalidValue)
        {
            Assert.Throws<FormatException>(() => _handler.Parse(invalidValue));
        }
    }
}