using dotenv.net;
using DSharpPlus;
using DSharpPlus.Commands;
using DSharpPlus.Commands.Processors.SlashCommands;
using DSharpPlus.Commands.Processors.SlashCommands.InteractionNamingPolicies;
using DSharpPlus.Entities;
using GrindBot.DiscordClient.Common;
using GrindBot.Domain.Common;
using Serilog;

#region env and logging

var solutionDir = Directory.GetParent(Directory.GetCurrentDirectory())?.Parent;
DotEnv.Fluent().WithTrimValues().WithEnvFiles($"{solutionDir}/.env").WithOverwriteExistingVars().Load();

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .MinimumLevel.Override("Microsoft", Serilog.Events.LogEventLevel.Warning)
    .MinimumLevel.Override("System", Serilog.Events.LogEventLevel.Warning)
    .Enrich.FromLogContext()
    .WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}")
    .CreateLogger();

#endregion

var guildIds = "DISCORD_GUILD_IDS".FromEnv().Split(';').Select(ulong.Parse).ToArray();

var eventHandlerTypes = typeof(Program).Assembly.GetTypes()
    .Where(x => x.GetInterfaces().Contains(typeof(IEventHandler)))
    .ToList();

const DiscordIntents intents = DiscordIntents.AllUnprivileged | DiscordIntents.MessageContents | DiscordIntents.GuildMembers;
var token = "DISCORD_BOT_TOKEN".FromEnv();

var discord = DiscordClientBuilder.CreateDefault(token, intents)
    .ConfigureLogging(loggingBuilder => loggingBuilder.AddSerilog())
    .ConfigureServices(services => services.AddApplicationServices())
    .ConfigureEventHandlers(builder => builder.AddEventHandlers(eventHandlerTypes))
    .UseCommands((_, commands) =>
    {
        commands.AddProcessor(new SlashCommandProcessor(new SlashCommandConfiguration
        {
            NamingPolicy = new SnakeCaseNamingFixer(),
        }));
        commands.AddChecks(typeof(Program).Assembly);
        commands.AddCommands(typeof(Program).Assembly, guildIds);
    })
    .UseZlibCompression()
    .Build();

await discord.ConnectAsync(new DiscordActivity("The Grind"), DiscordUserStatus.Online);
await Task.Delay(-1);
