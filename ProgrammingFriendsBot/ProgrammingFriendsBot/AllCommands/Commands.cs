using System;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Interactivity;
using DSharpPlus.Entities;
using DSharpPlus.Interactivity.Extensions;

namespace ProgrammingFriendsBot.AllCommands;

public class Commands : BaseCommandModule
{
    [Command("ping")]

    public async Task Respond(CommandContext ctx)
    {
        await ctx.Channel.SendMessageAsync("pong!");
    }

}

