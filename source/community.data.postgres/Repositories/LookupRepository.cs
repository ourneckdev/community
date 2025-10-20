using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;
using community.common.BaseClasses;
using community.data.postgres.Contexts;
using community.data.postgres.Interfaces;
using Dapper;
using Microsoft.AspNetCore.Http;

namespace community.data.postgres.Repositories;

/// <summary>
///     Implements the necessary queries to interact with retrieving data to populate drop down lists
/// </summary>
/// <param name="context">The Dapper context</param>
/// <param name="contextAccessor">context access for accessing properties from the http pipeline</param>
public class LookupRepository(ILookupsDapperContext context, IHttpContextAccessor contextAccessor)
    : BaseRepository(contextAccessor), ILookupRepository
{
    /// <inheritdoc />
    public async ValueTask<IEnumerable<T>> ListAsync<T>(Guid? communityId = null)
        where T : BaseLookupEntity
    {
        using var connection = context.CreateConnection();
        var sql = HasCommunityIdColumn<T>()
            ? $"""
              select *
                from {GetTableName<T>()}
               where is_active
                 and (community_id is null or (@cid != emptyGuid() and community_id = @cid))
              """
            : $"""
              select *
               from {GetTableName<T>()}
              where is_active
              """;
        return await connection.QueryAsync<T>(
            sql, new { cid = communityId.GetValueOrDefault() });
    }
    
    
    private string GetTableName<T>()
    {
        var type = typeof(T);
        var tableAttr = type.GetCustomAttribute<TableAttribute>();
        
        if (tableAttr == null) return type.Name;
        
        var tableName = tableAttr.Name;
        return tableName;
    }

    private bool HasCommunityIdColumn<T>()
    {
        return typeof(T).BaseType == typeof(BaseLookupCommunityEntity);
    }
}