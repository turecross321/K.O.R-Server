using Bunkum.HttpServer.Configuration;

namespace K.O.R_Server.Configuration;

public class GameServerConfig : Config
{
    public override int CurrentConfigVersion => 2;
    public override int Version { get; set; }
    
    protected override void Migrate(int oldVer, dynamic oldConfig) {}
    
    public string EmailAddress { get; set; } = "";
    public string EmailPassword { get; set; } = "";
    public string EmailHost { get; set; } = "smtp.gmail.com";
    public int EmailHostPort { get; set; } = 587;
    public bool EmailSsl { get; set; } = true;
    
    public bool DiscordEnabled { get; set; }
    public string DiscordWebhookLink { get; set; } = "https://discord.com/api/webhooks/id/code";
}