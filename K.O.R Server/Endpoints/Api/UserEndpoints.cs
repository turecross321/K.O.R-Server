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
    public UserResponse? GetUser(RequestContext context, GameDatabaseContext database, string id)
    {
        GameUser? user = database.GetUserWithId(id);
        if (user == null) return null;

        return new UserResponse(user);
    }
}