using Common.Extensions;
using System.Data.SqlClient;

namespace Common.Classes;

public class Guest
{
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Phone { get; set; }
    public string Email { get; set; }
    public bool RsvpStatus { get; set; }
    public DateTime? RsvpDate { get; set; }
    public string DietaryRestrictions { get; set; }
    public DateTime? DietaryRestrictionsUpdated { get; set; }

    public Guest() { }

    public Guest(SqlDataReader reader, ref int i)
    {
        Id = reader.GetInt32(i++);
        FirstName = reader.GetString(i++);
        LastName = reader.GetString(i++);
        Phone = reader.GetNullableValue<string>(i++);
        Email = reader.GetNullableValue<string>(i++);
        RsvpStatus = reader.GetBoolean(i++);
        RsvpDate = reader.GetNullableValue<DateTime?>(i++);
        DietaryRestrictions = reader.GetNullableValue<string>(i++);
        DietaryRestrictionsUpdated = reader.GetNullableValue<DateTime?>(i++);
    }
}
