using System;
using K.O.R_Server.Types;

namespace K.O.R_Server.Responses;

public class SessionResponse
{
    public SessionResponse(GameSession session)
    {
        Id = session.Id.ToString();
        ExpiryDate = session.ExpiryDate;
    }

    public string Id { get; init; }
    public DateTimeOffset ExpiryDate { get; init; }
}