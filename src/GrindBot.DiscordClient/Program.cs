using dotenv.net;
using DSharpPlus;
using DSharpPlus.Commands;
using DSharpPlus.Commands.Processors.SlashCommands;
using DSharpPlus.Commands.Processors.SlashCommands.InteractionNamingPolicies;
using DSharpPlus.Entities;
using GrindBot.DiscordClient.Common;
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

var guildId = ulong.Parse("DISCORD_GUILD_ID".FromEnv());

var eventHandlerTypes = typeof(Program).Assembly.GetTypes()
    .Where(x => x.GetInterfaces().Contains(typeof(IEventHandler)))
    .ToList();

var intents = DiscordIntents.AllUnprivileged | DiscordIntents.MessageContents;
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
        commands.AddCommands(typeof(Program).Assembly, guildId);
    })
    .UseZlibCompression()
    .Build();

await discord.ConnectAsync(new DiscordActivity("The Grind"), DiscordUserStatus.Online);
await Task.Delay(-1);


// var guildId = ulong.Parse("DISCORD_GUILD_ID".FromEnv());
//
// slash.SlashCommandErrored += async (s, e) =>
// {
//     if (e.Exception is SlashExecutionChecksFailedException slex)
//     {
//         foreach (var check in slex.FailedChecks)
//             if (check is SlashCommands.RequireUserIdAttribute att)
//                 await e.Context.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, new DiscordInteractionResponseBuilder().WithContent($"Only <@{att.UserId}> can run this command!"));
//     }
// };
//
//
// await discord.ConnectAsync();
// await Task.Delay(-1);
//
//
// public class SlashCommands : ApplicationCommandModule
// {
//     // DI example
//     // public Database Database { private get; set; }
//
//     // Arguments
//     // If you want the user to be able to give more data to the command, you can add some arguments.
//     //
//     // Arguments must have the Option attribute, and can be of type:
//     //
//     // string
//     // long or long?
//     // bool or bool?
//     // double or double?
//     // DiscordUser - This can be cast to DiscordMember if the command is run in a guild
//     // DiscordChannel
//     // DiscordRole
//     // DiscordAttachment
//     // SnowflakeObject - This can accept both a user and a role; you can cast it DiscordUser, DiscordMember or DiscordRole to get the actual object
//     // Enum - This can used for choices through an enum; read further
//
//     [Command("test", "A slash command made to test the DSharpPlusSlashCommands library!")]
//     public async Task TestCommand(InteractionContext ctx)
//     {
//         await ctx.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, new DiscordInteractionResponseBuilder().WithContent("Success!"));
//     }
//
//     [Command("delaytest", "A slash command made to test the DSharpPlus Slash Commands extension!")]
//     public async Task DelayTestCommand(InteractionContext ctx)
//     {
//         await ctx.CreateResponseAsync(InteractionResponseType.DeferredChannelMessageWithSource);
//
//         //Some time consuming task like a database call or a complex operation
//
//         await ctx.EditResponseAsync(new DiscordWebhookBuilder().WithContent("Thanks for waiting!"));
//     }
//
//     //Attribute choices
//     [Command("ban", "Bans a user")]
//     public async Task Ban(InteractionContext ctx, [Option("user", "User to ban")] DiscordUser user,
//         [Choice("None", 0)] [Choice("1 Day", 1)] [Choice("1 Week", 7)] [Option("deletedays", "Number of days of message history to delete")]
//         long deleteDays = 0)
//     {
//         await ctx.Guild.BanMemberAsync(user.Id, (int)deleteDays);
//         await ctx.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, new DiscordInteractionResponseBuilder().WithContent($"Banned {user.Username}"));
//     }
//
//     [Command("enum", "Test enum")]
//     public async Task EnumCommand(InteractionContext ctx, [Option("enum", "enum option")] MyEnum myEnum = MyEnum.option1)
//     {
//         await ctx.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, new DiscordInteractionResponseBuilder().WithContent(myEnum.GetName()));
//     }
//
//     [Command("choiceprovider", "test")]
//     public async Task ChoiceProviderCommand(InteractionContext ctx,
//         [ChoiceProvider(typeof(TestChoiceProvider))] [Option("option", "option")]
//         string option)
//     {
//         await ctx.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, new DiscordInteractionResponseBuilder().WithContent(option));
//     }
//
//     [Command("admin", "runs sneaky admin things")]
//     [RequireUserId(0000000000000)]
//     public async Task Admin(InteractionContext ctx)
//     {
//         //secrets }
//     }
//
//
//     //Enum choices
//     public enum MyEnum
//     {
//         [ChoiceName("Option 1")]
//         option1,
//
//         [ChoiceName("Option 2")]
//         option2,
//
//         [ChoiceName("Option 3")]
//         option3
//     }
//
//     //ChoiceProvider choices
//     public class TestChoiceProvider : IChoiceProvider
//     {
//         public async Task<IEnumerable<DiscordApplicationCommandOptionChoice>> Provider()
//         {
//             return new DiscordApplicationCommandOptionChoice[]
//             {
//                 //You would normally use a database call here
//                 new DiscordApplicationCommandOptionChoice("testing", "testing"),
//                 new DiscordApplicationCommandOptionChoice("testing2", "test option 2"),
//             };
//         }
//     }
//
//
//     public class RequireUserIdAttribute : SlashCheckBaseAttribute
//     {
//         public ulong UserId;
//
//         public RequireUserIdAttribute(ulong userId)
//         {
//             this.UserId = userId;
//         }
//
//         public override async Task<bool> ExecuteChecksAsync(InteractionContext ctx)
//         {
//             if (ctx.User.Id == UserId)
//                 return true;
//             else
//                 return false;
//         }
//     }
// }