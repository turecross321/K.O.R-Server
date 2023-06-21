namespace K.O.R_Server.Helpers;

public static partial class UserHelper
{
    public static bool IsUsernameLegal(string username)
    {
        return UsernameRegex().IsMatch(username);
    }

    [System.Text.RegularExpressions.GeneratedRegex("^[A-Za-z][A-Za-z0-9-_]{2,11}$")]
    private static partial System.Text.RegularExpressions.Regex UsernameRegex();
}