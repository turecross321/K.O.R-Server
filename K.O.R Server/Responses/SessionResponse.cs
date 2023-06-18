using K.O.R_Server.Responses.Users;
using K.O.R_Server.Types;

namespace K.O.R_Server.Responses;

public class SessionResponse
{
    public SessionResponse(GameSession session)
    {
        Id = session.Id;
        ExpiryDate = session.ExpiryDate;
        User = new FullUserResponse(session.User);
    }

    public string Id { get; }
    public DateTimeOffset ExpiryDate { get; }
    public FullUserResponse User { get; }
}