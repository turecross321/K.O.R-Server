using K.O.R_Server.Responses.Users;
using K.O.R_Server.Types.Leaderboard;

namespace K.O.R_Server.Responses.Leaderboard;

public class LeaderboardEntryResponse
{
    public LeaderboardEntryResponse(LeaderboardEntry entry)
    {
        Id = entry.Id;
        User = new BriefUserResponse(entry.User);
        Time = entry.Time;
        Score = entry.Score;
        CreationDate = entry.CreationDate;
    }

    public string Id { get; }
    public BriefUserResponse User { get; }
    public float Time { get; }
    public int Score { get; }
    public DateTimeOffset CreationDate { get; }
}