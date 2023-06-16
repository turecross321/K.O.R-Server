#pragma warning disable CS8618
namespace K.O.R_Server.Requests.Account;

// ReSharper disable once ClassNeverInstantiated.Global
public class SetPasswordRequest
{
    public string NewPasswordSha512 { get; set; }
}