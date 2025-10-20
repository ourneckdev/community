namespace community.common.Enumerations;
//
// /// <summary>
// ///     With shared tables, we need to know what type of entity is referenced by row.  The Entity Type allows us to type it
// ///     as a community or user record.
// /// </summary>
// public readonly record struct EntityType : IEnumeration
// {
//     private readonly string _value;
//
//     /// <summary>
//     ///     Record relates to a community
//     /// </summary>
//     public static EntityType Community => nameof(Community);
//
//     /// <summary>
//     ///     Record relates to a user
//     /// </summary>
//     public static EntityType User => nameof(User);
//
//     /// <summary>
//     ///     Constructor
//     /// </summary>
//     /// <param name="value"></param>
//     private EntityType(string value)
//     {
//         _value = value;
//     }
//
//     /// <summary>
//     ///     Converts a string to an entity type.
//     /// </summary>
//     /// <param name="value"></param>
//     /// <returns></returns>
//     public static implicit operator EntityType(string value) => new EntityType(value);
//
//     /// <summary>
//     ///     Converts an entity type to a string
//     /// </summary>
//     /// <param name="entityType"></param>
//     /// <returns></returns>
//     public static implicit operator string(EntityType entityType) => entityType._value;
// }

/// <summary>
/// 
/// </summary>
public enum EntityType
{
    /// <summary>
    /// 
    /// </summary>
    Community,
    /// <summary>
    /// 
    /// </summary>
    User
}