using System.Data.SqlClient;

namespace Common.Extensions;

public static class Extensions
{
    public static T? GetNullableValue<T>(this SqlDataReader reader, int index)
    {
        if (!reader.IsDBNull(index))
            return (T)reader.GetValue(index);
        return default;
    }

    public static void NullableValue(this SqlParameter parameter, object? value)
    {
        parameter.Value = value ?? DBNull.Value;
    }
}