namespace community.data.entities.Search;

/// <summary>
///     Search for an existing community using supplied properties
/// </summary>
/// <param name="Name">The name of the community to attepmt to match</param>
/// <param name="AddressLine1"></param>
/// <param name="City"></param>
/// <param name="StateCode"></param>
/// <param name="PostalCode"></param>
/// <param name="PhoneNumber"></param>
public record FindCommunityRecord(
    string? Name,
    string? AddressLine1,
    string? City,
    string? StateCode,
    string? PostalCode,
    string? PhoneNumber)
{
    /// <summary>
    /// Builds the sql statement based on available parameters.
    /// </summary>
    /// <returns></returns>
    public string BuildQuery()
    {
        var baseSql = !string.IsNullOrEmpty(Name)
            ? """ 
              select c.id, c.name, cast(null as uuid) address_id, cast(null as uuid) contact_id
               from community c
              where levenshtein(c.name, @Name, 1, 0, 4) <= 3
              """
            : "";

        var addressSql = !string.IsNullOrEmpty(AddressLine1) ||
                         !string.IsNullOrEmpty(City) ||
                         !string.IsNullOrEmpty(StateCode) ||
                         !string.IsNullOrEmpty(PostalCode)
            ? """
              select c.id, c.name, a.id address_id, cast(null as uuid) contact_id
                from community_address a
                join community c on a.community_id = c.id
               where (@name is null or levenshtein(c.name, cast(@Name as citext), 1, 0, 4) <= 3)
                 and (@StateCode is null or a.state_code = cast(@StateCode as citext))
                 and (@City is null or a.city = cast(@City as citext))
                 and (@PostalCode is null or a.postal_code = @PostalCode)
                 and (@AddressLine1 is null or levenshtein(address_1, cast(@AddressLine1 as citext), 5, 0, 4) <= 4)
              """
            : "";

        var contactSql = !string.IsNullOrEmpty(PhoneNumber)
            ? """
              select c.id, c.name, cast(null as uuid) address_id, t.id contact_id
               from contact t 
               join contact_method m on t.contact_method_id = m.id
                and m.contact_type = 'phone'
               join community c on c.id = t.community_id
                and entity_type = 0
              where (@name is null or levenshtein(c.name, cast(@Name as citext), 1, 0, 4) <= 3)
                and (@PhoneNumber is null or t.value = @PhoneNumber)
              """
            : "";

        return baseSql +
               ((baseSql != "" || addressSql != "") && contactSql != "" ? " union " : "") +
               addressSql + 
               ((baseSql != "" || addressSql != "") && contactSql != "" ? " union " : "") +
               contactSql;
    }
}