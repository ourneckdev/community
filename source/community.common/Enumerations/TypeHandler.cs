using System.Data;
using Dapper;

namespace community.common.Enumerations;

/// <summary>
///     Dapper type handler
/// </summary>
/// <typeparam name="T"></typeparam>
public class TypeHandler<T> : SqlMapper.TypeHandler<T> where T : IEnumeration
{
    /// <summary>
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public override T Parse(object value)
    {
        return (T)value;
    }

    /// <summary>
    /// </summary>
    /// <param name="parameter"></param>
    /// <param name="value"></param>
    public override void SetValue(IDbDataParameter parameter, T? value)
    {
        parameter.DbType = DbType.String;
        parameter.Value = $"{value}";
    }
}