using DSharpPlus;
using Microsoft.Extensions.Hosting;

namespace ProgrammingFriendsBot.DiscordBot;

internal class ProgrammingFriendsService : IHostedService
{
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        var discord = new DiscordClient(new DiscordConfiguration() {
            Token = "TOKENHEREFORNOW",
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