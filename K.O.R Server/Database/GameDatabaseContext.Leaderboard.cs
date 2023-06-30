using K.O.R_Server.Helpers;
using K.O.R_Server.Requests;
using K.O.R_Server.Types;
using K.O.R_Server.Types.Leaderboard;

namespace K.O.R_Server.Database;

public partial class GameDatabaseContext
{
    public LeaderboardEntry CreateLeaderboardEntry(GameUser user, CreateLeaderboardEntryRequest request)
    {
        LeaderboardEntry entry = new()
        {
            Id = GenerateGuid(),
            User = user,
            Score = request.Score,
            Time = request.Time,
            CreationDate = DateTimeOffset.UtcNow
        };

        _realm.Write(() =>
        {
            _realm.Add(entry);
        });

        return entry;
    }

    private readonly LeaderboardFilters _placeFilters = new() { OnlyBest = true };

    public int FindPlaceForEntry(LeaderboardEntry entry)
    {
        IQueryable<LeaderboardEntry> entries = _realm.All<LeaderboardEntry>();
        IQueryable<LeaderboardEntry> filteredEntries = LeaderboardHelper.FilterLeaderboard(entries, _placeFilters);
        IQueryable<LeaderboardEntry> orderedEntries = LeaderboardHelper.OrderLeaderboard(filteredEntries, LeaderboardOrderType.Score, true);

        int i = 0;
        foreach (LeaderboardEntry orderedEntry in orderedEntries)
        {
            if (orderedEntry.Id == entry.Id) return i + 1;
            i++;
        }

        return -1;
    }

    public (LeaderboardEntry[], int) GetLeaderboard(LeaderboardFilters filters, LeaderboardOrderType order, bool descending, int from, int count)
    {
        IQueryable<LeaderboardEntry> entries = _realm.All<LeaderboardEntry>();
        IQueryable<LeaderboardEntry> filteredEntries = LeaderboardHelper.FilterLeaderboard(entries, filters);
        IQueryable<LeaderboardEntry> orderedEntries = LeaderboardHelper.OrderLeaderboard(filteredEntries, order, descending);
        IQueryable<LeaderboardEntry> paginatedEntries = PaginationHelper.PaginateLeaderboard(orderedEntries, from, count);
        
        return (paginatedEntries.ToArray(), filteredEntries.Count());
    }
}