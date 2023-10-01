using System.Net;
using Bunkum.Core;
using Bunkum.Core.Endpoints;
using Bunkum.Core.Responses;
using Bunkum.Listener.Protocol;
using Bunkum.Protocols.Http;
using K.O.R_Server.Database;
using K.O.R_Server.Requests;
using K.O.R_Server.Responses.Leaderboard;
using K.O.R_Server.Services;
using K.O.R_Server.Types;
using K.O.R_Server.Types.Leaderboard;

namespace K.O.R_Server.Endpoints.Api;

public class LeaderboardEndpoints : EndpointGroup
{
    [ApiEndpoint("leaderboard/create", HttpMethods.Post, ContentType.Json)]
    public Response CreateLeaderboardEntry(RequestContext context, GameDatabaseContext database, CreateLeaderboardEntryRequest body, GameUser user, WebhookService webhook)
    {
        LeaderboardEntry entry = database.CreateLeaderboardEntry(user, body);

        int place = database.FindPlaceForEntry(entry);
        if (place <= 5 && place != -1) webhook.AnnounceLeaderboardEntry(entry, place);

        LeaderboardEntryResponse response = new(entry);
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
        
        string? beforeDateString = context.QueryString["before"];
        DateTimeOffset? before = null;
        if (beforeDateString != null) before = DateTimeOffset.FromUnixTimeSeconds(long.Parse(beforeDateString));
        
        string? afterDateString = context.QueryString["after"];
        DateTimeOffset? after = null;
        if (afterDateString != null) after = DateTimeOffset.FromUnixTimeSeconds(long.Parse(afterDateString));

        LeaderboardFilters filters = new()
        {
            ByUser = byUser,
            OnlyBest = onlyBest,
            Before = before,
            After = after
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