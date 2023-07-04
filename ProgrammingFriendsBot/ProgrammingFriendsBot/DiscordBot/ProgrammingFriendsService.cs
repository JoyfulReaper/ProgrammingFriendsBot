using DSharpPlus;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using ProgrammingFriendsBot.Common.Options;

namespace ProgrammingFriendsBot.DiscordBot;

internal class ProgrammingFriendsService : IHostedService
{
    private readonly ProgrammingFriendsBotOptions _options;

    public ProgrammingFriendsService(IOptions<ProgrammingFriendsBotOptions> options)
    {
        _options = options.Value;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        var discord = new DiscordClient(new DiscordConfiguration() {
            Token = _options.Token,
            TokenType = TokenType.Bot,
            Intents = DiscordIntents.AllUnprivileged | DiscordIntents.MessageContents
        });

        discord.MessageCreated += async (s, e) => {
            if (e.Message.Content.ToLower().StartsWith("ping"))
                await e.Message.RespondAsync("pong!");
        };

        await discord.ConnectAsync();
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}