using System.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Npgsql;

namespace community.data.postgres.Contexts;

/// <summary>
///     Initializes a standalone context for interacting with user data.
/// </summary>
public class LookupsDapperContext(IConfiguration configuration, ILogger<LookupsDapperContext> logger)
    : ILookupsDapperContext
{
    /// <summary>
    ///     Creates a connection to the underlying data store.
    /// </summary>
    /// <returns></returns>
    public NpgsqlConnection CreateConnection()
    {
        var connectionString = configuration.GetConnectionString("community");

        if (string.IsNullOrEmpty(connectionString))
            throw new Exception("Connection string is empty");

        logger.LogInformation("Connection string: {FirstOrDefault}", connectionString?.Split(';').FirstOrDefault());
        return new NpgsqlConnection(connectionString);
    }
}