using Common.Classes;
using Common.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Data.SqlClient;

namespace Common.DataContext;

[CatchErrors]
public class SqlServiceWeddingDataContext : IWeddingDataContext
{
    public ILogger Logger;
    private readonly SqlConnection connection;
    
    public SqlServiceWeddingDataContext(IConfiguration config, ILoggerFactory loggerFactory)
    {
        Logger = loggerFactory.CreateLogger<SqlServiceWeddingDataContext>();
        connection = new SqlConnection(config.GetConnectionString("Database"));
    }

    public async Task<bool> UpdateRsvp(Guest guest)
    {
        using SqlCommand command = new SqlCommand("spUpdateRsvp", connection);
        command.CommandType = System.Data.CommandType.StoredProcedure;

        command.Parameters.Add("@ID", System.Data.SqlDbType.Int).Value = guest.Id;
        command.Parameters.Add("@PHONE", System.Data.SqlDbType.VarChar).Value = guest.Phone;
        command.Parameters.Add("@EMAIL", System.Data.SqlDbType.VarChar).Value = guest.Email;
        command.Parameters.Add("@RSVP", System.Data.SqlDbType.Bit).Value = guest.RsvpStatus;
        command.Parameters.Add("@DIETARY_RESTRICTIONS", System.Data.SqlDbType.NVarChar).Value = guest.DietaryRestrictions;

        await command.ExecuteNonQueryAsync();
        // Attribute will catch any errors and return false
        return true;
    }

    public async Task<IEnumerable<Guest>> GetGuestsByInvitationCode(string code)
    {
        List<Guest> guests = new List<Guest>();
        using SqlCommand command = new SqlCommand("SELECT Id, FirstName, LastName, Phone, Email, RsvpStatus, RsvpDate, DietaryRestrictions, " +
            "DietaryRestrictionsUpdated FROM fnGetGuestsByInvitationVode(@CODE)", connection);

        command.Parameters.Add("@CODE", System.Data.SqlDbType.Char).Value = code;
        
        using SqlDataReader reader = await command.ExecuteReaderAsync();
        while (reader.Read())
        {
            int i = 0;
            guests.Add(new Guest(reader, ref i));
        }

        return guests;
    }
}
