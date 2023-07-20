using System;
using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.Interactivity.Extensions;
using DSharpPlus.SlashCommands;
namespace ProgrammingFriendsBot.AllSlashCommands;

public class SlashCommands : ApplicationCommandModule
{
    [SlashCommand("titlehere", "description here")]
    public async Task TestCommand(InteractionContext ctx)
    {
        // if you want to give a user options to select
        //[Option("User", "User you want to ban")] DiscordUser user,
        //[Option("Reason", "Why we are banning this user")] string reason = null
        // defer message allows you to create response later on
        await ctx.DeferAsync();

        var message = new DiscordEmbedBuilder()
        {
            Title = "test test",
            Description = "description here",
            Color = DiscordColor.Red
        };

        await ctx.EditResponseAsync(new DiscordWebhookBuilder().AddEmbed(message));
        

    }
}


