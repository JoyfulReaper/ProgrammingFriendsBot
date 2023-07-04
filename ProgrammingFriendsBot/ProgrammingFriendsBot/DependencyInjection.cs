using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ProgrammingFriendsBot.DiscordBot;

namespace ProgrammingFriendsBot;

internal static class DependencyInjection
{
    internal static IHost SetupDependencyInjection(string[] args)
    {
        IHost host = Host.CreateDefaultBuilder(args)
            .ConfigureServices((context, services) => {
                services.AddHostedService<ProgrammingFriendsService>();
            }).ConfigureAppConfiguration((context, config) => {
                config.AddUserSecrets<Program>();
            }).Build();

        return host;
    }
}
