using Bunkum.HttpServer.Authentication;
using Realms;
#pragma warning disable CS8618

namespace K.O.R_Server.Types;

public class GameSession : RealmObject, IToken
{
    [PrimaryKey] [Required] public string Id { get; init; }
    
    // Realm can't store enums, use recommended workaround
    // ReSharper disable once InconsistentNaming (can't fix due to conflict with SessionType)
    // ReSharper disable once MemberCanBePrivate.Global
    internal int _SessionType { get; set; }
    public SessionType SessionType
    {
        get => (SessionType)_SessionType;
        set => _SessionType = (int)value;
    }
    public GameUser User { get; init; }
    public DateTimeOffset CreationDate { get; init; }
    public DateTimeOffset ExpiryDate { get; init; }
}