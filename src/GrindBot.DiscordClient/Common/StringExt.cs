namespace GrindBot.DiscordClient.Common;

public static class StringExt
{
    extension(string str)
    {
        public string FromEnv(string? defaultValue = null) =>
            Environment.GetEnvironmentVariable(str) ?? defaultValue ?? throw new InvalidOperationException($"'{str}' is not set.");
    }
}