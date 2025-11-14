using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Npgsql;

namespace community.data.postgres.Contexts;

/// <summary>
///     Context responsible for handling database connections to the underlying data store.
/// </summary>
public class AuthorizationDapperContext(IConfiguration configuration, ILogger<AuthorizationDapperContext> logger)
    : IAuthorizationDapperContext
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