using Bunkum.CustomHttpListener.Parsing;
using Bunkum.HttpServer;
using Bunkum.HttpServer.Endpoints;
using K.O.R_Server.Database;
using K.O.R_Server.Responses;
using K.O.R_Server.Types;

namespace K.O.R_Server.Endpoints.Api;

public class UserEndpoints : EndpointGroup
{
    [ApiEndpoint("users/id/{id}", ContentType.Json)]
    [Authentication(false)]
    public UserResponse? GetUserWithId(RequestContext context, GameDatabaseContext database, string id)
    {
        GameUser? user = database.GetUserWithId(id);
        if (user == null) return null;

        return new UserResponse(user);
    }
    
    [ApiEndpoint("users/username/{username}", ContentType.Json)]
    [Authentication(false)]
    public UserResponse? GetUserWithUsername(RequestContext context, GameDatabaseContext database, string username)
    {
        GameUser? user = database.GetUserWithUsername(username);
        if (user == null) return null;

        return new UserResponse(user);
    }
}