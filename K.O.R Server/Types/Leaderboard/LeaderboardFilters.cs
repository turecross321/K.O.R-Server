namespace K.O.R_Server.Types.Leaderboard;

public class LeaderboardFilters
{
    public GameUser? ByUser { get; init; }
    public bool? OnlyBest { get; init; }
    public DateTimeOffset? Before { get; init; }
    public DateTimeOffset? After { get; init; }
}