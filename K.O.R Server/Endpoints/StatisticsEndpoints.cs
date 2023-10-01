using System.Net;
using Bunkum.Core;
using Bunkum.Core.Endpoints;
using Bunkum.Core.Responses;
using Bunkum.Listener.Protocol;
using Bunkum.Protocols.Http;
using K.O.R_Server.Database;
using K.O.R_Server.Types;

namespace K.O.R_Server.Endpoints;

public class StatisticsEndpoints : EndpointGroup
{
    [ApiEndpoint("statistics/set", HttpMethods.Post, ContentType.Json)]
    public Response SetStatistics(RequestContext context, GameDatabaseContext database, GameUser user, UserStatistics body)
    {
        database.SetUserStatistics(user, body);
        return HttpStatusCode.Created;
    }
}