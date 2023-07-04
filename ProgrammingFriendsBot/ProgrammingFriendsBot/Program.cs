using ProgrammingFriendsBot;

var host = DependencyInjection.SetupDependencyInjection(args);
await  host.StartAsync();

await Task.Delay(-1);

host.Dispose();