using Realms;

namespace K.O.R_Server.Types;

public class UserStatistics : EmbeddedObject
{
    public float TotalPlayTime { get; set; }
    public int TotalStartups { get; set; }
    public int TotalJumps { get; set; }
    public int TotalDeaths { get; set; }
    public int TotalMoney { get; set; }
}