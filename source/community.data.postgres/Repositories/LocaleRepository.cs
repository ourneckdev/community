using community.common.BaseClasses;
using community.data.entities.Locales;
using community.data.postgres.Contexts;
using community.data.postgres.Interfaces;
using Dapper;
using Microsoft.AspNetCore.Http;
using TimeZone = community.data.entities.Locales.TimeZone;

namespace community.data.postgres.Repositories;

/// <summary>
/// </summary>
/// <param name="context"></param>
/// <param name="contextAccessor">context access for accessing properties from the http pipeline</param>
public class LocalRepository(ILookupsDapperContext context, IHttpContextAccessor contextAccessor)
    : BaseRepository(contextAccessor), ILocaleRepository
{
    /// <inheritdoc />
    public async ValueTask<IEnumerable<Country>> ListCountriesAsync()
    {
        using var connection = context.CreateConnection();
        return await connection.QueryAsync<Country>(
            """
            select code
                 , name
                 , iso_2
                 , numeric_code
              from countries
             where is_active
             order by sort_order, name
            """);
    }

    /// <inheritdoc />
    public async ValueTask<IEnumerable<State>> ListStatesAsync(string countryCode)
    {
        using var connection = context.CreateConnection();
        return await connection.QueryAsync<State>(
            """
            select code
                 , country_code
                 , ansi_code
                 , name
                 , fips_code 
            from states 
            where country_code = @countryCode 
            and is_active
            """, new { countryCode });
    }

    /// <inheritdoc />
    public async ValueTask<IEnumerable<County>> ListCountiesAsync(string countryCode, string stateCode)
    {
        using var connection = context.CreateConnection();
        return await connection.QueryAsync<County>(
            """
            select code
                 , country_code
                 , state_code
                 , name
              from counties 
             where country_code = @countryCode 
               and state_code = @stateCode
               and is_active
            """, new { countryCode, stateCode });
    }

    /// <inheritdoc />
    public async ValueTask<IEnumerable<TimeZone>> ListTimeZonesAsync(string countryCode)
    {
        using var connection = context.CreateConnection();
        return await connection.QueryAsync<TimeZone>(
            """
            with tz as (select abbrev as code
                 , name
                 , utc_offset
                 , row_number() over (partition by utc_offset order by name) rnk
              from pg_catalog.pg_timezone_names
             where name like @code || '%'
            order by utc_offset desc, name)
            select code
                 , name
                 , utc_offset
              from tz 
             --where rnk = 1
            """, new { code = countryCode.Substring(0, 2) });
    }

    /// <inheritdoc />
    public async ValueTask<TimeZone?> GetTimeZoneAsync(string countryCode, string name,
        CancellationToken cancellationToken = default)
    {
        using var connection = context.CreateConnection();

        return await connection.QueryFirstOrDefaultAsync<TimeZone>(
            new CommandDefinition(
                """
                with tz as (select abbrev as code
                     , name
                     , utc_offset
                     , row_number() over (partition by utc_offset order by name) rnk
                  from pg_catalog.pg_timezone_names
                 where name like @code || '%'
                order by utc_offset desc, name)
                select code
                     , name
                     , utc_offset
                  from tz 
                 where name = @name
                """, name, cancellationToken: cancellationToken));
    }
}