namespace GrindBot.Domain.Common;

public static class DateTimeExt
{
    extension(DateTime date)
    {
        public string FormatRelative()
        {
            var nextMs = new DateTimeOffset(date).ToUnixTimeMilliseconds() / 1000;
            return $"<t:{nextMs}:R>";
        }
    }
    
    extension(DateTime? date)
    {
        public string FormatRelative()
        {
            return date is null ? "never" : date.Value.FormatRelative();
        }
    }
}