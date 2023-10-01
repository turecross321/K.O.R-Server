using System.Net;
using Bunkum.Core;
using Bunkum.Core.Endpoints;
using Bunkum.Core.Responses;
using Bunkum.Listener.Protocol;
using Bunkum.Protocols.Http;
using K.O.R_Server.Database;
using K.O.R_Server.Requests;
using K.O.R_Server.Types;

namespace K.O.R_Server.Endpoints.Api;

public class SkinEndpoints : EndpointGroup
{
    [ApiEndpoint("skins/set", HttpMethods.Post)]
    public Response SetSelectedSkin(RequestContext context, GameDatabaseContext database, GameUser user, SetSkinRequest body)
    {
        database.SetUserSelectedSkin(user, body.SelectedSkin);
        return HttpStatusCode.Created;
    }
}