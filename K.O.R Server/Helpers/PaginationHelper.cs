using K.O.R_Server.Types.Leaderboard;

namespace K.O.R_Server.Helpers;

public static class PaginationHelper
{
    private const int MaxEntryCount = 100;
    public static IQueryable<LeaderboardEntry> PaginateLeaderboard(IEnumerable<LeaderboardEntry> objects, int from, int count)
    {
        return objects.Skip(from).Take(Math.Min(count, MaxEntryCount)).AsQueryable();
    }
}