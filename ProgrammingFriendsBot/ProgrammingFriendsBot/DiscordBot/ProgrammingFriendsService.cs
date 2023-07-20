using DSharpPlus;
using DSharpPlus.Interactivity;
using DSharpPlus.CommandsNext;
using DSharpPlus.SlashCommands;
using DSharpPlus.EventArgs;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ProgrammingFriendsBot.AllCommands;
using ProgrammingFriendsBot.AllSlashCommands;
using ProgrammingFriendsBot.Common.Options;
using System.Text;

namespace ProgrammingFriendsBot.DiscordBot;

internal class ProgrammingFriendsService : IHostedService
{
    private readonly ProgrammingFriendsBotOptions _options;
    private readonly ILogger<ProgrammingFriendsService> _logger;
    public CommandsNextExtension _commands;
    public SlashCommandsExtension _slashCommands;

    public ProgrammingFriendsService(IOptions<ProgrammingFriendsBotOptions> options,
        ILogger<ProgrammingFriendsService> logger)
    {
        _options = options.Value;
        _logger = logger;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        var discord = new DiscordClient(new DiscordConfiguration() {
            Token = _options.Token,
            TokenType = TokenType.Bot,
            Intents = DiscordIntents.AllUnprivileged | DiscordIntents.MessageContents | DiscordIntents.GuildMembers
        });

        discord.GuildMemberAdded += NewUserHandler;

        // setting up commands
        var commandConfig = new CommandsNextConfiguration()
        {
            StringPrefixes = new string[] { "$" },
            EnableMentionPrefix = true,
            EnableDefaultHelp = true,
            EnableDms = true // can change this??

        };

       _commands = discord.UseCommandsNext(commandConfig);
        _slashCommands = discord.UseSlashCommands();
        _commands.RegisterCommands<Commands>();
        _slashCommands.RegisterCommands<SlashCommands>();

        

        await discord.ConnectAsync();
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    private async Task NewUserHandler(DiscordClient s, GuildMemberAddEventArgs e)
    {
        {
            _logger.LogDebug("User joined: {displayName}", e.Member.DisplayName);

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
    }
}