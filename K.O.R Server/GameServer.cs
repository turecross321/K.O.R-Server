using System.Reflection;
using Bunkum.Core.Authentication;
using Bunkum.Core.RateLimit;
using Bunkum.Protocols.Http;
using K.O.R_Server.Authentication;
using K.O.R_Server.Configuration;
using K.O.R_Server.Database;
using K.O.R_Server.Middlewares;
using K.O.R_Server.Services;
using K.O.R_Server.Types;

namespace K.O.R_Server;

public class GameServer
{
    protected readonly BunkumHttpServer ServerInstance;
    protected readonly GameDatabaseProvider DatabaseProvider;

    public GameServer(BunkumHttpListener? listener = null,
        GameDatabaseProvider? databaseProvider = null,
        IAuthenticationProvider<GameSession>? authProvider = null)
    {
        databaseProvider ??= new GameDatabaseProvider();
        authProvider ??= new SessionProvider();

        DatabaseProvider = databaseProvider;

        ServerInstance = listener == null ? new BunkumHttpServer() : new BunkumHttpServer(listener);
        
        ServerInstance.UseDatabaseProvider(databaseProvider);
        ServerInstance.AddAuthenticationService(authProvider, true);

        ServerInstance.DiscoverEndpointsFromAssembly(Assembly.GetExecutingAssembly());
    }
    
    public Task StartAndBlockAsync()
    {
        return ServerInstance.StartAndBlockAsync();
    }
    
    public void Start()
    {
        ServerInstance.Start();
    }

    public void Initialize()
    {
        DatabaseProvider.Initialize();
        
        SetUpConfiguration();
        SetUpServices();
        SetUpMiddlewares();
    }

    protected virtual void SetUpConfiguration()
    {
        ServerInstance.UseJsonConfig<GameServerConfig>("gameServer.json");
    }
    
    protected virtual void SetUpServices()
    {
        ServerInstance.AddRateLimitService(new RateLimitSettings(30, 40, 0, "global"));
        ServerInstance.AddService<EmailService>();
        ServerInstance.AddService<WebhookService>();
    }

    protected virtual void SetUpMiddlewares()
    {
        ServerInstance.AddMiddleware<CrossOriginMiddleware>();
    }
}