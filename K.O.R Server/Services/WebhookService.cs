using Bunkum.HttpServer;
using Bunkum.HttpServer.Services;
using Discord;
using Discord.Webhook;
using K.O.R_Server.Configuration;
using K.O.R_Server.Types.Leaderboard;
using NotEnoughLogs;

namespace K.O.R_Server.Services;

/// <summary>
/// A service for posting updates to a Discord channel via Webhooks.
/// </summary>
/// <remarks>
/// This service is always available, even when disabled in <see cref="GameServerConfig"/>.
/// When disabled, calls to this service will simply do nothing.
/// Calls are fire-and-forget - they will not block the caller's thread.
/// </remarks>
public class WebhookService : EndpointService
{
    private readonly GameServerConfig _config;
    private readonly DiscordWebhookClient? _client;

    public WebhookService(LoggerContainer<BunkumContext> logger, GameServerConfig config) : base(logger)
    {
        this._config = config;
        
        if (config.DiscordEnabled) 
            this._client = new DiscordWebhookClient(config.DiscordWebhookLink);
    }

    private void PostEmbedToWebhook(Embed embed)
    {
        if (this._client == null) return;

        Task.Factory.StartNew(async () =>
        {
            try
            {
                await this._client.SendMessageAsync(embeds: new[] { embed });
            }
            catch(Exception e)
            {
                this.Logger.LogWarning(BunkumContext.Service, $"Discord webhook failed to send: {e}");
            }
        });
    }

    public void AnnounceLeaderboardEntry(LeaderboardEntry entry, int place)
    {
        TimeSpan time = TimeSpan.FromSeconds(entry.Time);
        
        EmbedBuilder builder = new EmbedBuilder()
            .WithTimestamp(entry.CreationDate)
            .WithTitle($"{entry.User.Username} just achieved #{place}!")
            .WithDescription($"{entry.Score:N0} points in {time:mm\\:ss}");

        switch (place)
        {
            case 1:
                builder.WithColor(255, 210, 52);
                break;
            case 2:
                builder.WithColor(242, 242, 242);
                break;
            case 3:
                builder.WithColor(255, 136, 69);
                break;
            default:
                builder.WithColor(171, 171, 171);
                break;
        }

        this.PostEmbedToWebhook(builder.Build());
    }
}