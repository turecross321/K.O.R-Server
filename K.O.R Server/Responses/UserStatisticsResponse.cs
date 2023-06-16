using K.O.R_Server.Types;

namespace K.O.R_Server.Responses;

public class UserStatisticsResponse
{
    public UserStatisticsResponse(UserStatistics statistics)
    {
        TotalPlayTime = statistics.TotalPlayTime;
        TotalStartups = statistics.TotalStartups;
        TotalJumps = statistics.TotalJumps;
        TotalDeaths = statistics.TotalDeaths;
        TotalMoney = statistics.TotalMoney;
    }

    public float TotalPlayTime { get; }
    public int TotalStartups { get; }
    public int TotalJumps { get; }
    public int TotalDeaths { get; }
    public int TotalMoney { get; }
}