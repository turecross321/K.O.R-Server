using System.Net;
using Bunkum.CustomHttpListener.Parsing;
using Bunkum.HttpServer;
using Bunkum.HttpServer.Endpoints;
using Bunkum.HttpServer.Responses;
using K.O.R_Server.Database;
using K.O.R_Server.Types;

namespace K.O.R_Server.Endpoints;

public class StatisticsEndpoints : EndpointGroup
{
    [ApiEndpoint("statistics/set", Method.Post, ContentType.Json)]
    public Response SetStatistics(RequestContext context, GameDatabaseContext database, GameUser user, UserStatistics body)
    {
        database.SetUserStatistics(user, body);
        return HttpStatusCode.Created;
    }
}