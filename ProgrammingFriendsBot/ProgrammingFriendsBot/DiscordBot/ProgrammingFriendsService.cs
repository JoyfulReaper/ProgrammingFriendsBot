﻿using DSharpPlus;
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
            Intents = DiscordIntents.AllUnprivileged | DiscordIntents.MessageContents
        });

        discord.GuildMemberAdded += async (s, e) => {
            var programmersRole = e.Guild.Roles
                .Where(r => r.Value.Name == "Programmers")
                .Select(r => r.Value)
                .FirstOrDefault();

            if (programmersRole is not null)
            {
                await e.Member.GrantRoleAsync(programmersRole, "User Joined The Server");
            }

            var generalChannel = e.Guild.Channels
                .Where(c => c.Value.Name
                .Equals("general", StringComparison.InvariantCultureIgnoreCase))
                .Select(c => c.Value)
                .FirstOrDefault();

            if (generalChannel is not null)
            {
                await generalChannel.SendMessageAsync($"Everyone please welcome {e.Member.Nickname}!");
            }
        };

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
}