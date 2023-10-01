using Bunkum.Core;
using Bunkum.Core.Endpoints;
using Bunkum.Listener.Protocol;
using K.O.R_Server.Database;
using K.O.R_Server.Responses.Users;
using K.O.R_Server.Types;

namespace K.O.R_Server.Endpoints.Api;

public class UserEndpoints : EndpointGroup
{
    [ApiEndpoint("users/id/{id}", ContentType.Json)]
    [Authentication(false)]
    public FullUserResponse? GetUserWithId(RequestContext context, GameDatabaseContext database, string id)
    {
        GameUser? user = database.GetUserWithId(id);
        if (user == null) return null;

        return new FullUserResponse(user);
    }
    
    [ApiEndpoint("users/username/{username}", ContentType.Json)]
    [Authentication(false)]
    public FullUserResponse? GetUserWithUsername(RequestContext context, GameDatabaseContext database, string username)
    {
        GameUser? user = database.GetUserWithUsername(username);
        if (user == null) return null;

        return new FullUserResponse(user);
    }
}