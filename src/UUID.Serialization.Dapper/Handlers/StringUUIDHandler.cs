using System.Data;
using static Dapper.SqlMapper;

namespace System
{
    /// <summary>
    /// Handles UUID serialization as string data for Dapper.
    /// </summary>
    /// <remarks>
    /// This handler stores UUIDs as 32-character fixed-length strings in the database,
    /// providing human-readable values at the cost of more storage space.
    /// </remarks>
    public class StringUUIDHandler : TypeHandler<UUID>
    {
        /// <summary>
        /// Parses a string value from the database into a UUID.
        /// </summary>
        /// <param name="value">The string value from the database.</param>
        /// <returns>A UUID instance.</returns>
        public override UUID Parse(object value)
        {
            return UUID.Parse((string)value);
        }

        /// <summary>
        /// Sets the parameter value for database operations.
        /// </summary>
        /// <param name="parameter">The database parameter to set.</param>
        /// <param name="value">The UUID value to set.</param>
        public override void SetValue(IDbDataParameter parameter, UUID value)
        {
            parameter.Size = 32;
            parameter.Value = value.ToString();
            parameter.DbType = DbType.StringFixedLength;
        }
    }
}