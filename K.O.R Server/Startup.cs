using Bunkum.Core;
using K.O.R_Server;

BunkumConsole.AllocateConsole();

GameServer server = new();
server.Initialize();

server.Start();
await Task.Delay(-1);