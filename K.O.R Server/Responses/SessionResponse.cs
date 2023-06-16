using K.O.R_Server.Types;

namespace K.O.R_Server.Responses;

public class SessionResponse
{
    public SessionResponse(GameSession session)
    {
        Id = session.Id;
        ExpiryDate = session.ExpiryDate;
        User = new UserResponse(session.User);
    }

    public string Id { get; }
    public DateTimeOffset ExpiryDate { get; }
    public UserResponse User { get; }
}