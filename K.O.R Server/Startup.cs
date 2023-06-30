using Bunkum.HttpServer;
using K.O.R_Server;
using K.O.R_Server.Database;
using K.O.R_Server.Requests.Account;
using K.O.R_Server.Types;

// using GameDatabaseProvider provider = new();
// provider.Initialize();
//
// using GameDatabaseContext context = provider.GetContext();
//
// GameUser user = context.CreateUser(new RegistrationRequest
// {
//     Username = Random.Shared.Next().ToString(),
//     Email = Random.Shared.Next() + "@jvyden.xyz",
//     PasswordSha512 = "ab18168b6eec69df586491b6e5eaf28c6b6948a6e43defab2592dfeb69a0307c40c0f74094485ecc305a4df68d70cf49fcf0eb355f151ee368569332be18a317"
// });
//
// Console.WriteLine(user.Username);
//
// GameSession session = context.CreateSession(user, SessionType.Api);
// Console.WriteLine(session.Id);
//
// return;

BunkumConsole.AllocateConsole();

GameServer server = new();
server.Initialize();

server.Start();
await Task.Delay(-1);