namespace community.common.Enumerations;
//
// /// <summary>
// ///     Identifies the type of UserContactMethod record the data relates to
// /// </summary>
// public readonly record struct ContactType : IEnumeration
// {
//     private readonly string _value;
//
//     /// <summary>
//     ///     Phone number
//     /// </summary>
//     public static ContactType Phone => nameof(Phone);
//
//     /// <summary>
//     ///     Email address
//     /// </summary>
//     public static ContactType Email => nameof(Email);
//
//     private ContactType(string value)
//     {
//         _value = value;
//     }
//
//     /// <summary>
//     ///     converts a string representation to a ContactType
//     /// </summary>
//     /// <param name="value"></param>
//     /// <returns></returns>
//     public static implicit operator ContactType(string value)
//     {
//         return new ContactType(value);
//     }
//
//     /// <summary>
//     ///     converts a contact type to a string
//     /// </summary>
//     /// <param name="contactType"></param>
//     /// <returns></returns>
//     public static implicit operator string(ContactType contactType)
//     {
//         return contactType._value;
//     }
// }

/// <summary>
/// 
/// </summary>
public enum ContactType
{
    /// <summary>
    /// 
    /// </summary>
    Phone,
    
    /// <summary>
    /// 
    /// </summary>
    Email
}