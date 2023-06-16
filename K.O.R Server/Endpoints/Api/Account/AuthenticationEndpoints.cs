using System.Net;
using Bunkum.CustomHttpListener.Parsing;
using Bunkum.HttpServer;
using Bunkum.HttpServer.Endpoints;
using Bunkum.HttpServer.Responses;
using K.O.R_Server.Database;
using K.O.R_Server.Requests.Account;
using K.O.R_Server.Responses;
using K.O.R_Server.Types;

namespace K.O.R_Server.Endpoints.Api.Account;

public class AuthenticationEndpoints : EndpointGroup
{
    private readonly Response _invalidCredentialsResponse = new ("The email address or password was incorrect.", ContentType.Plaintext, HttpStatusCode.Forbidden);
    
    [ApiEndpoint("account/logIn", Method.Post, ContentType.Json)]
    [Authentication(false)]
    public Response LogIn(RequestContext context, GameDatabaseContext database, AuthenticationRequest body)
    {
        GameUser? user = database.GetUserWithEmail(body.Email);
        if (user == null) return _invalidCredentialsResponse;

        if (!database.ValidatePassword(user, body.PasswordSha512)) return _invalidCredentialsResponse;

        GameSession session = database.CreateSession(user, SessionType.Api);
        return new Response(new SessionResponse(session), ContentType.Json, HttpStatusCode.Created);
    }

    [ApiEndpoint("account/logOut", Method.Post)]
    public Response LogOut(RequestContext context, GameDatabaseContext database, GameSession token)
    {
        database.RemoveSession(token);
        return HttpStatusCode.OK;
    }
}