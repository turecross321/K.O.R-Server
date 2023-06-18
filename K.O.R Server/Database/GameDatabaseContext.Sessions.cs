using K.O.R_Server.Types;

namespace K.O.R_Server.Database;

public partial class GameDatabaseContext
{
    private const int DefaultSessionExpirySeconds = 86400; // 1 day
    private const int SessionLimit = 3; // limit of how many sessions a user can have simultaneously 
    public GameSession CreateSession(GameUser user, SessionType type, long? expirationSeconds = null, string? id = null)
    {
        double sessionExpirationSeconds = expirationSeconds ?? DefaultSessionExpirySeconds;
        
        GameSession session = new()
        {
            Id = id ?? GenerateGuid(),
            SessionType = type,
            User = user,
            CreationDate = DateTimeOffset.UtcNow,
            ExpiryDate = DateTimeOffset.UtcNow.AddSeconds(sessionExpirationSeconds)
        };

        IEnumerable<GameSession> sessionsToDelete = _realm.All<GameSession>()
            .Where(s => s.User == user && s._SessionType == (int)type)
            .AsEnumerable()
            .SkipLast(SessionLimit - 1);

        _realm.Write(() =>
        {
            foreach (GameSession gameSession in sessionsToDelete)
            {
                _realm.Remove(gameSession);
            }
            
            _realm.Add(session);
        });
        
        return session;
    }
    public void RemoveSession(GameSession session)
    {
        _realm.Write(() =>
        {
            _realm.Remove(session);
        });
    }
    public GameSession? GetSessionWithId(string id)
    {
        return _realm.All<GameSession>().FirstOrDefault(s => s.Id == id);
    }
}