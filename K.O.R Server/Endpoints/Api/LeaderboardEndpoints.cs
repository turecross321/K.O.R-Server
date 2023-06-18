using System.Net;
using Bunkum.CustomHttpListener.Parsing;
using Bunkum.HttpServer;
using Bunkum.HttpServer.Endpoints;
using Bunkum.HttpServer.Responses;
using K.O.R_Server.Database;
using K.O.R_Server.Requests;
using K.O.R_Server.Responses.Leaderboard;
using K.O.R_Server.Types;
using K.O.R_Server.Types.Leaderboard;

namespace K.O.R_Server.Endpoints.Api;

public class LeaderboardEndpoints : EndpointGroup
{
    [ApiEndpoint("leaderboard/create", Method.Post, ContentType.Json)]
    public Response CreateLeaderboardEntry(RequestContext context, GameDatabaseContext database, CreateLeaderboardEntryRequest body, GameUser user)
    {
        LeaderboardEntryResponse response = new(database.CreateLeaderboardEntry(user, body));
        return new Response(response, ContentType.Json, HttpStatusCode.Created);
    }

    [ApiEndpoint("leaderboard", ContentType.Json)]
    [Authentication(false)]
    public LeaderboardWrapper GetLeaderboard(RequestContext context, GameDatabaseContext database)
    {
        int from = int.Parse(context.QueryString["from"] ?? "0");
        int count = int.Parse(context.QueryString["count"] ?? "9");

        string? byUserId = context.QueryString["byUser"];
        GameUser? byUser = null;
        if (byUserId != null) byUser = database.GetUserWithId(byUserId);
        
        if (bool.TryParse(context.QueryString["onlyBest"], out bool onlyBest));

        LeaderboardFilters filters = new()
        {
            ByUser = byUser,
            OnlyBest = onlyBest
        };
        
        string? orderString = context.QueryString["orderBy"];

        LeaderboardOrderType order = orderString switch
        {
            "score" => LeaderboardOrderType.Score,
            "creationDate" => LeaderboardOrderType.CreationDate,
            _ => LeaderboardOrderType.Score
        };
        
        bool descending = bool.Parse(context.QueryString["descending"] ?? "false");

        (LeaderboardEntry[] entries, int totalEntries) =
            database.GetLeaderboard(filters, order, descending, from, count);

        return new LeaderboardWrapper(entries, totalEntries);
    }
}