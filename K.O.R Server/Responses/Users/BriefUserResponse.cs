using K.O.R_Server.Types;

namespace K.O.R_Server.Responses.Users;

public class BriefUserResponse
{
    public BriefUserResponse(GameUser user)
    {
        Id = user.Id;
        Username = user.Username;
        SelectedSkin = user.SelectedSkin;
    }

    public string Id { get; set; }
    public string Username { get; set; }
    public int SelectedSkin { get; set; }
}