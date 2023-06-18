using Bunkum.RealmDatabase;

namespace K.O.R_Server.Database;

// ReSharper disable once ClassNeverInstantiated.Global
public partial class GameDatabaseContext : RealmDatabaseContext
{
    private static string GenerateGuid()
    {
        Guid guid = Guid.NewGuid();
        return guid.ToString();
    }
}