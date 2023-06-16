﻿using K.O.R_Server.Types;

namespace K.O.R_Server.Responses;

public class UserResponse
{
    public UserResponse(GameUser user)
    {
        Id = user.Id;
        Username = user.Username;
        CreationDate = user.CreationDate;
        SelectedSkin = user.SelectedSkin;
        Statistics = new UserStatisticsResponse(user.Statistics);
    }

    public string Id { get; }
    public string Username { get; }
    public DateTimeOffset CreationDate { get; }
    public int SelectedSkin { get; }
    public UserStatisticsResponse Statistics { get; }
}