using K.O.R_Server.Types.Leaderboard;

namespace K.O.R_Server.Responses.Leaderboard;

public class LeaderboardWrapper
{
    public LeaderboardWrapper(IEnumerable<LeaderboardEntry> entries, int totalEntries)
    {
        Entries = entries.Select(e=>new LeaderboardEntryResponse(e)).ToArray();
        Count = totalEntries;
    }

    public LeaderboardEntryResponse[] Entries { get; set; }
    public int Count { get; set; }
}