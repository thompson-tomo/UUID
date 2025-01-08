using System.Data;
using static Dapper.SqlMapper;

namespace System
{
    /// <summary>
    /// Handles UUID serialization as Base64 string data for Dapper.
    /// </summary>
    /// <remarks>
    /// This handler stores UUIDs as 24-character Base64 strings in the database,
    /// providing a more compact string representation compared to hexadecimal.
    /// </remarks>
    public class Base64UUIDHandler : TypeHandler<UUID>
    {
        /// <summary>
        /// Parses a Base64 string value from the database into a UUID.
        /// </summary>
        /// <param name="value">The Base64 string value from the database.</param>
        /// <returns>A UUID instance.</returns>
        public override UUID Parse(object value)
        {
            return UUID.FromBase64((string)value);
        }

        /// <summary>
        /// Sets the parameter value for database operations.
        /// </summary>
        /// <param name="parameter">The database parameter to set.</param>
        /// <param name="value">The UUID value to set.</param>
        public override void SetValue(IDbDataParameter parameter, UUID value)
        {
            parameter.Size = 24;
            parameter.Value = value.ToBase64();
            parameter.DbType = DbType.StringFixedLength;
        }
    }
}
