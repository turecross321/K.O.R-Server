using System;
using Bunkum.CustomHttpListener.Request;
using Bunkum.HttpServer.Authentication;
using Bunkum.HttpServer.Database;
using K.O.R_Server.Database;
using K.O.R_Server.Types;
using static K.O.R_Server.Helpers.SessionHelper;

namespace K.O.R_Server.Authentication;

public class SessionProvider : IAuthenticationProvider<GameUser, GameSession>
{
    public GameUser? AuthenticateUser(ListenerContext request, Lazy<IDatabaseContext> db)
    {
        GameUser? user = AuthenticateToken(request, db)?.User;
        if (user == null) return null;

        user.RateLimitUserId = user.Id;
        return user;
    }

    public GameSession? AuthenticateToken(ListenerContext request, Lazy<IDatabaseContext> db)
    {
        string? id = request.RequestHeaders["Authorization"];
        if (id == null) return null;

        GameDatabaseContext database = (GameDatabaseContext)db.Value;

        GameSession? session = database.GetSessionWithId(id);
        if (session == null) return null;

        if (session.ExpiryDate < DateTimeOffset.UtcNow)
        {
            database.RemoveSession(session);
            return null;
        }
        
        if (!IsSessionAllowedToAccessEndpoint(session, request.Uri.AbsolutePath)) return null;
        return session;
    }
}