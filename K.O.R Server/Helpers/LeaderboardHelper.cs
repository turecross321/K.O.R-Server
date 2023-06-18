using K.O.R_Server.Types.Leaderboard;

namespace K.O.R_Server.Helpers;

public static class LeaderboardHelper
{
    public static IQueryable<LeaderboardEntry> FilterLeaderboard(IQueryable<LeaderboardEntry> entries,
        LeaderboardFilters filters)
    {
        if (filters.ByUser != null)
            entries = entries.Where(e => e.User == filters.ByUser);

        if (filters.OnlyBest == true)
        {
            List<LeaderboardEntry> bestEntries = new();

            List<string> previousUserIds = new();
            foreach (LeaderboardEntry entry in entries.OrderByDescending(e=>e.Score))
            {
                if (previousUserIds.Contains(entry.User.Id)) continue;
                
                bestEntries.Add(entry);
                previousUserIds.Add(entry.User.Id);
            }

            entries = bestEntries.AsQueryable();
        }

        return entries;
    }

    public static IQueryable<LeaderboardEntry> OrderLeaderboard(IQueryable<LeaderboardEntry> entries,
        LeaderboardOrderType order, bool descending)
    {
        return order switch
        {
            LeaderboardOrderType.Score => OrderLeaderboardByScore(entries, descending),
            LeaderboardOrderType.CreationDate => OrderLeaderboardByCreationDate(entries, descending),
            _ => entries
        };
    }

    private static IQueryable<LeaderboardEntry> OrderLeaderboardByScore(IQueryable<LeaderboardEntry> entries, bool descending)
    {
        if (descending) return entries.OrderByDescending(e => e.Score);
        return entries.OrderBy(e => e.Score);
    }
    
    private static IQueryable<LeaderboardEntry> OrderLeaderboardByCreationDate(IQueryable<LeaderboardEntry> entries, bool descending)
    {
        if (descending) return entries.OrderByDescending(e => e.CreationDate);
        return entries.OrderBy(e => e.CreationDate);
    }
}