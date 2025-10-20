using System.ComponentModel.DataAnnotations.Schema;
using community.common.BaseClasses;

namespace community.data.entities.Lookups;

/// <summary>
///     Defines standard and community specific address types.
/// </summary>
[Table("address_type")]
public sealed class AddressType : BaseLookupCommunityEntity
{
}