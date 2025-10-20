using System.Data;

namespace community.common.Interfaces;

/// <summary>
///     Common interface to use for all potential dapper implementations.
/// </summary>
public interface IDapperContext
{
    /// <summary>
    ///     Instantiates a connection to use for the current scope.
    /// </summary>
    /// <returns></returns>
    IDbConnection CreateConnection();
}