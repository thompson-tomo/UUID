using System.Data;
using static Dapper.SqlMapper;

namespace System
{
    /// <summary>
    /// Handles UUID serialization as binary data for Dapper.
    /// </summary>
    /// <remarks>
    /// This handler stores UUIDs as 16-byte binary values in the database,
    /// providing the most efficient storage option.
    /// </remarks>
    public class BinaryUUIDHandler : TypeHandler<UUID>
    {
        /// <summary>
        /// Parses a binary value from the database into a UUID.
        /// </summary>
        /// <param name="value">The binary value from the database.</param>
        /// <returns>A UUID instance.</returns>
        public override UUID Parse(object value)
        {
            return UUID.FromByteArray((byte[])value);
        }

        /// <summary>
        /// Sets the parameter value for database operations.
        /// </summary>
        /// <param name="parameter">The database parameter to set.</param>
        /// <param name="value">The UUID value to set.</param>
        public override void SetValue(IDbDataParameter parameter, UUID value)
        {
            parameter.Size = 16;
            parameter.DbType = DbType.Binary;
            parameter.Value = value.ToByteArray();
        }
    }
}