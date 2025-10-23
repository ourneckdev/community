using System.Data;
using System.Reflection;
using Microsoft.Data.SqlClient;

namespace community.tests.common;

public static class ExceptionsHelper
{
    public static SqlException CreateSqlException(int number, string errorMessage = "the mock error message")
    {
        var collectionConstructor = typeof(SqlErrorCollection)
            .GetConstructor(BindingFlags.NonPublic | BindingFlags.Instance,
                null,
                Type.EmptyTypes,
                null);
        var addMethod = typeof(SqlErrorCollection).GetMethod("Add", BindingFlags.NonPublic | BindingFlags.Instance);
        var errorCollection = (SqlErrorCollection)collectionConstructor?.Invoke(null)!;
        var errorConstructor = typeof(SqlError).GetConstructor(BindingFlags.NonPublic | BindingFlags.Instance, null,
        [
            typeof(int), typeof(byte), typeof(byte), typeof(string), typeof(string), typeof(string),
            typeof(int), typeof(Exception), typeof(int)
        ], null);
        var error = errorConstructor?.Invoke([
            number, (byte)0, (byte)0, "server", "errMsg", "proccedure", 100, new Exception(), 0
        ]);
        addMethod?.Invoke(errorCollection, [error]);
        var constructor = typeof(SqlException).GetConstructor(BindingFlags.NonPublic | BindingFlags.Instance,
            null,
            [typeof(string), typeof(SqlErrorCollection), typeof(Exception), typeof(Guid)],
            null);
        return (SqlException)constructor?.Invoke([errorMessage, errorCollection, new DataException(), Guid.NewGuid()])!;
    }
}