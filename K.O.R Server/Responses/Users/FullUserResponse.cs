using K.O.R_Server.Types;

namespace K.O.R_Server.Responses.Users;

public class FullUserResponse
{
    public FullUserResponse(GameUser user)
    {
        Id = user.Id;
        Username = user.Username;
        CreationDate = user.CreationDate.ToUnixTimeSeconds();
        SelectedSkin = user.SelectedSkin;
        Statistics = new UserStatisticsResponse(user.Statistics);
    }

    public string Id { get; }
    public string Username { get; }
    public long CreationDate { get; }
    public int SelectedSkin { get; }
    public UserStatisticsResponse Statistics { get; }
}