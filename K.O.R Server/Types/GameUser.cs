using Bunkum.Core.RateLimit;
using K.O.R_Server.Types.Leaderboard;
using Realms;
// ReSharper disable UnassignedGetOnlyAutoProperty
#pragma warning disable CS8618

namespace K.O.R_Server.Types;

public class GameUser : RealmObject, IRateLimitUser
{
    [PrimaryKey] [Required] public string Id { get; init; }
    public string Username { get; set; }
    public string Email { get; set; }
    public string PasswordBcrypt { get; set; }
    public DateTimeOffset CreationDate { get; init; }
    public int SelectedSkin { get; set; }
    public UserStatistics Statistics { get; set; }
    [Backlink(nameof(GameSession.User))] public IQueryable<GameSession> Sessions { get; }
    [Backlink(nameof(LeaderboardEntry.User))] public IQueryable<LeaderboardEntry> LeaderboardEntries { get; }

    // Defined in authentication provider. Avoids Realm threading nonsense.
    public bool RateLimitUserIdIsEqual(object obj)
    {
        if (obj is not string id) return false;
        return Id == id;
    }

    [Ignored] public object RateLimitUserId { get; internal set; } = null!;
}