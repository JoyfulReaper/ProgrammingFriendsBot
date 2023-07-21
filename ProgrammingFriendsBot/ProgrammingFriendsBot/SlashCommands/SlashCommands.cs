using System;
using DSharpPlus;
using OpenAI_API;
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

    [SlashCommand("chat-gpt", "Ask chat-gpt a question")]
    public async Task ChatGPT(InteractionContext ctx, [Option("Query", "What would you like to search?")] string query)
    {
        await ctx.DeferAsync();

        // fill this with actual apiKey

        var apiKey = "fillthisinplease";
        var api = new OpenAIAPI(apiKey);

        // make a chat
        var chat = api.Chat.CreateConversation();

        chat.AppendSystemMessage("Type in the query");
        chat.AppendUserInput(query);

        string response = await chat.GetResponseFromChatbotAsync();

        var Message = new DiscordEmbedBuilder()
        {
            Title = "Query response to " + query.ToLower() + ".",
            Description = response,
            Color = DiscordColor.Green
        };

        await ctx.EditResponseAsync(new DiscordWebhookBuilder().AddEmbed(Message));
    }


}


