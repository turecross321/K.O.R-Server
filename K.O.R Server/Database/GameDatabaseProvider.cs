using System;
using System.Collections.Generic;
using Bunkum.RealmDatabase;
using K.O.R_Server.Types;
using Realms;

namespace K.O.R_Server.Database;

public class GameDatabaseProvider : RealmDatabaseProvider<GameDatabaseContext>
{
    protected override ulong SchemaVersion => 4;

    protected override List<Type> SchemaTypes => new()
    {
        typeof(GameUser),
        typeof(GameSession)
    };

    protected override string Filename => "database.realm";
    
    public override void Warmup()
    {
        using GameDatabaseContext context = GetContext();
    }

    protected override void Migrate(Migration migration, ulong oldVersion)
    {
        
    }
}