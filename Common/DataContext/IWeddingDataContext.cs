using Common.Classes;

namespace Common.DataContext;

public interface IWeddingDataContext
{
    public Task<bool> UpdateRsvp(Guest guest);
    public Task<IEnumerable<Guest>> GetGuestsByInvitationCode(string code);
}
