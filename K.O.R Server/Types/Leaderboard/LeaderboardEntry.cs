using Realms;

namespace K.O.R_Server.Types.Leaderboard;

public class LeaderboardEntry : RealmObject
{
    public string Id { get; init; } = null!;
    public GameUser User { get; init; } = null!;
    public int Score { get; init; }
    public float Time { get; init; }
    public DateTimeOffset CreationDate { get; init; }
}