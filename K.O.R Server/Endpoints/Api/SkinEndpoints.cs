using System.Net;
using Bunkum.CustomHttpListener.Parsing;
using Bunkum.HttpServer;
using Bunkum.HttpServer.Endpoints;
using Bunkum.HttpServer.Responses;
using K.O.R_Server.Database;
using K.O.R_Server.Requests;
using K.O.R_Server.Types;

namespace K.O.R_Server.Endpoints.Api;

public class SkinEndpoints : EndpointGroup
{
    [ApiEndpoint("skins/set", Method.Post, ContentType.Json)]
    public Response SetSelectedSkin(RequestContext context, GameDatabaseContext database, GameUser user, SetSkinRequest body)
    {
        database.SetUserSelectedSkin(user, body.SelectedSkin);
        return HttpStatusCode.Created;
    }
}