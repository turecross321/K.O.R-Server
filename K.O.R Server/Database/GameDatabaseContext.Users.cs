using System;
using System.Linq;
using K.O.R_Server.Requests.Account;
using K.O.R_Server.Types;
using MongoDB.Bson;

namespace K.O.R_Server.Database;

public partial class GameDatabaseContext
{
    public GameUser CreateUser(RegistrationRequest request)
    {
        GameUser user = new()
        {
            Username = request.Username,
            Email = request.Email,
            PasswordBcrypt = BCrypt.Net.BCrypt.HashPassword(request.PasswordSha512.ToLower(), WorkFactor),
            CreationDate = DateTimeOffset.UtcNow,
        };

        _realm.Write(() =>
        {
            _realm.Add(user);
        });

        return user;
    }

    public void RemoveUser(GameUser user)
    {
        _realm.Write(() =>
        {
            _realm.RemoveRange(user.Sessions);
            _realm.Remove(user);
        });
    }

    public void SetUsername(GameUser user, string username)
    {
        _realm.Write(() =>
        {
            user.Username = username;
        });
    }
    
    public void SetUserEmail(GameUser user, string email)
    {
        _realm.Write(() =>
        {
            user.Email = email;
        });
    }
    
    private const int WorkFactor = 10;
    public bool ValidatePassword(GameUser user, string hash)
    {
        if (BCrypt.Net.BCrypt.PasswordNeedsRehash(user.PasswordBcrypt, WorkFactor))
        {
            SetUserPassword(user, BCrypt.Net.BCrypt.HashPassword(hash.ToLower(), WorkFactor));
        }

        return BCrypt.Net.BCrypt.Verify(hash.ToLower(), user.PasswordBcrypt); 
    }

    public void SetUserPassword(GameUser user, string hash)
    {
        string passwordBcrypt = BCrypt.Net.BCrypt.HashPassword(hash.ToLower(), WorkFactor);
        
        _realm.Write(() =>
        {
            user.PasswordBcrypt = passwordBcrypt;
        });
    }
    
    public GameUser? GetUserWithEmail(string email)
    {
        return _realm.All<GameUser>().FirstOrDefault(u => u.Email == email);
    }
    
    public GameUser? GetUserWithId(ObjectId id)
    {
        return _realm.All<GameUser>().FirstOrDefault(u => u.Id == id);
    }
        
    public GameUser? GetUserWithUsername(string username)
    {
        return _realm.All<GameUser>().FirstOrDefault(u => u.Username == username);
    }
}