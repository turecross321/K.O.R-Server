using System.Net;
using System.Net.Mail;
using Bunkum.CustomHttpListener.Parsing;
using Bunkum.HttpServer;
using Bunkum.HttpServer.Endpoints;
using Bunkum.HttpServer.Responses;
using K.O.R_Server.Database;
using K.O.R_Server.Helpers;
using K.O.R_Server.Requests.Account;
using K.O.R_Server.Services;
using K.O.R_Server.Types;
using static K.O.R_Server.Helpers.SessionHelper;

namespace K.O.R_Server.Endpoints.Api.Account;

public partial class AccountManagementEndpoints : EndpointGroup
{
    [System.Text.RegularExpressions.GeneratedRegex("^[a-f0-9]{128}$")]
    private static partial System.Text.RegularExpressions.Regex Sha512Regex();

    [ApiEndpoint("account/register", Method.Post, ContentType.Json)]
    [Authentication(false)]
    public Response Register(RequestContext context, GameDatabaseContext database, RegistrationRequest body, EmailService emailService)
    {
        // Check if user has sent a valid mail address
        if (MailAddress.TryCreate(body.Email, out MailAddress? _) == false)
            return new Response("Invalid Email.", ContentType.Plaintext, HttpStatusCode.BadRequest);
        
        // Check if mail address has been used before
        GameUser? user = database.GetUserWithEmail(body.Email);
        if (user != null)
            return new Response("Email is already in use.", ContentType.Plaintext, HttpStatusCode.Conflict);
        
        if (body.PasswordSha512.Length != 128 || !Sha512Regex().IsMatch(body.PasswordSha512))
            return new Response("Password is definitely not SHA512. Please hash the password.",
                ContentType.Plaintext, HttpStatusCode.BadRequest);

        database.CreateUser(body);
        return HttpStatusCode.Created;
    }
    
    [ApiEndpoint("account/setUsername", Method.Post)]
    public Response SetUsername(RequestContext context, GameDatabaseContext database, GameUser user, SetUsernameRequest body)
    {
        if (!UserHelper.IsUsernameLegal(body.NewUsername)) return new Response("Not a valid username.", ContentType.Plaintext, HttpStatusCode.BadRequest);
        
        GameUser? userWithNewName = database.GetUserWithUsername(body.NewUsername);
        if (userWithNewName != null)
        {
            return new Response("Username is not available.", ContentType.Plaintext, HttpStatusCode.Conflict);
        }
        
        database.SetUsername(user, body.NewUsername);
        return HttpStatusCode.Created;
    }

    [ApiEndpoint("account/sendEmailSession", Method.Post)]
    public Response SendEmailSession(RequestContext context, GameDatabaseContext database, GameUser user, NewEmailRequest body, EmailService emailService)
    {
        string emailSessionId = GenerateEmailSessionId(database);
        GameSession emailSession = database.CreateSession(user, SessionType.SetEmail, 600, emailSessionId); // 10 minutes

        string emailBody = $"Dear {user.Username},\n\n" +
                           "Here is your new email code: " + emailSession.Id + "\n" +
                           "If this wasn't you, change your password immediately. Code expires in 10 minutes.";
        emailService.SendEmail(body.NewEmail, "K.O.R New Email Code", emailBody);

        return HttpStatusCode.Created;
    }
    
    [ApiEndpoint("account/setEmail", Method.Post)]
    public Response SetUserEmail(RequestContext context, GameDatabaseContext database, NewEmailRequest body, GameSession session, EmailService emailService)
    {
        GameUser user = session.User;

        // Check if user has sent a valid mail address
        if (MailAddress.TryCreate(body.NewEmail, out MailAddress? _) == false)
            return new Response("Invalid Email.", ContentType.Plaintext, HttpStatusCode.BadRequest);
        
        // Check if mail address has been used before
        GameUser? userWithEmail = database.GetUserWithEmail(body.NewEmail);
        if (userWithEmail != null && userWithEmail.Id != user.Id) 
            return new Response("Email is already in use.", ContentType.Plaintext, HttpStatusCode.Forbidden);

        database.SetUserEmail(user, body.NewEmail);
        database.RemoveSession(session);

        return HttpStatusCode.Created;
    }

    [ApiEndpoint("account/sendPasswordSession", Method.Post)]
    [Authentication(false)]
    public Response SendPasswordSession(RequestContext context, GameDatabaseContext database, NewPasswordSessionRequest body, EmailService emailService)
    {
        GameUser? user = database.GetUserWithEmail(body.Email);
        if (user == null) return HttpStatusCode.Created; // trol

        string passwordSessionId = GeneratePasswordSessionId(database);
        GameSession passwordSession = database.CreateSession(user, SessionType.SetPassword, 600, passwordSessionId); // 10 minutes

        string emailBody = $"Dear {user.Username},\n\n" +
                           "Here is your password code: " + passwordSession.Id + "\n" +
                           "If this wasn't you, feel free to ignore this email. Code expires in 10 minutes.";

        emailService.SendEmail(body.Email, "K.O.R Password Code", emailBody);

        return HttpStatusCode.Created;
    }
    
    [ApiEndpoint("account/setPassword", Method.Post)]
    public Response SetUserPassword(RequestContext context, GameDatabaseContext database, NewPasswordRequest body, GameSession session)
    {
        GameUser user = session.User;

        if (body.NewPasswordSha512.Length != 128 || !Sha512Regex().IsMatch(body.NewPasswordSha512))
            return new Response("Password is definitely not SHA512. Please hash the password.",
                ContentType.Plaintext, HttpStatusCode.BadRequest);

        database.SetUserPassword(user, body.NewPasswordSha512);
        database.RemoveSession(session);

        return HttpStatusCode.Created;
    }

    [ApiEndpoint("account/sendRemovalSession", Method.Post)]
    public Response SendUserRemovalSession(RequestContext context, GameDatabaseContext database, GameUser user, GameSession session, EmailService emailService)
    {
        string removalSessionId = GenerateAccountRemovalSessionId(database);
        GameSession removalSession = database.CreateSession(user, SessionType.RemoveAccount, 600, removalSessionId); // 10 minutes

        string emailBody = $"Dear {user.Username},\n\n" +
                           "Here is your account removal code: " + removalSession.Id + "\n" +
                           "If this wasn't you, change your password immediately. Code expires in 10 minutes.";
        
        emailService.SendEmail(user.Email, "K.O.R Account Removal Code", emailBody);

        return HttpStatusCode.Created;
    }
    
    [ApiEndpoint("account/remove", Method.Post)]
    public Response RemoveAccount(RequestContext context, GameDatabaseContext database, GameUser user)
    {
        database.RemoveUser(user);
        return new Response("o7", ContentType.Plaintext);
    }
}