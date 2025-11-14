using System.Data;
using Dapper;
using Npgsql;
using NpgsqlTypes;

namespace community.common.Repository;

/// <summary>
/// Represents a case-insensitive parameter for searching postgres.
/// </summary>
/// <param name="value"></param>
public class CitextParameter(string value) : SqlMapper.ICustomQueryParameter
{
    /// <summary>
    /// Adds a citext datatype parameter
    /// </summary>
    /// <param name="command"></param>
    /// <param name="name"></param>
    public void AddParameter(IDbCommand command, string name)
    {
        command.Parameters.Add(new NpgsqlParameter
        {
            ParameterName = name,
            NpgsqlDbType = NpgsqlDbType.Citext,
            Value = value
        });
    }
}
