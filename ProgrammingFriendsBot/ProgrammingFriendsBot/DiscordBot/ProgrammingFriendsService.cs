using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using ProgrammingFriendsBot.Common.Options;
using System.Text;

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
            AutoReconnect = true,
            Intents = DiscordIntents.AllUnprivileged | DiscordIntents.MessageContents
        });

        discord.GuildMemberAdded += NewUserHandler;

        discord.MessageCreated += async (s, e) => {
            if (e.Message.Content.ToLower().StartsWith("$ping"))
                await e.Message.RespondAsync("pong!");
        };

        await discord.ConnectAsync();
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    private static async Task NewUserHandler(DiscordClient sender, GuildMemberAddEventArgs e)
    {
        var defualtChannel = e.Guild.GetDefaultChannel();

        var programmersRole = e.Guild.Roles
       .Where(r => r.Value.Name == "Programmers")
       .Select(r => r.Value)
       .FirstOrDefault();

        await e.Member.GrantRoleAsync(programmersRole, "User Joined The Server");

        await defualtChannel.SendMessageAsync($"Everyone please welcome {e.Member.Nickname}!");  

    }

}
