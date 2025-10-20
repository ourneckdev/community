using community.common.Enumerations;

namespace community.common.Definitions;

/// <summary>
///     Fast lookup of contact methods
/// </summary>
public static class ContactMethods
{
    /// <summary>
    ///     Maps the database guids to a string for display.
    /// </summary>
    public static Dictionary<Guid, (string, ContactType)> Values => new(6)
    {
        { Guid.Parse("01950294-15e0-7bd7-b232-4cfe690f1bb5"), (Strings.ContactMethod_MobilePhone, ContactType.Phone) },
        { Guid.Parse("01950294-15e2-70e1-b1b1-172ea8b81d87"), (Strings.ContactMethod_HomePhone, ContactType.Phone) },
        { Guid.Parse("01950294-15e2-7287-a4c7-c17d6c38d4d8"), (Strings.ContactMethod_WorkPhone, ContactType.Phone) },
        { Guid.Parse("01950294-15e2-732f-ac22-1ff1ab6ae67c"), (Strings.ContactMethod_EmergencyContact, ContactType.Phone) },
        { Guid.Parse("01950294-15e2-7374-969a-dd871e567604"), (Strings.ContactMethod_PersonalEmail, ContactType.Email) },
        { Guid.Parse("01950294-15e2-73a1-8772-dc729ab46f37"), (Strings.ContactMethod_WorkEmail, ContactType.Email) }
    };
}