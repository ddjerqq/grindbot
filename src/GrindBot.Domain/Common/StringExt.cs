namespace GrindBot.Domain.Common;

public static class StringExt
{
    extension(string str)
    {
        private static Dictionary<char, char> GeorgianMap => new()
        {
            ['ა'] = 'a',
            ['ბ'] = 'b',
            ['გ'] = 'g',
            ['დ'] = 'd',
            ['ე'] = 'e',
            ['ვ'] = 'v',
            ['ზ'] = 'z',
            ['თ'] = 'T',
            ['ი'] = 'i',
            ['კ'] = 'k',
            ['ლ'] = 'l',
            ['მ'] = 'm',
            ['ნ'] = 'n',
            ['ო'] = 'o',
            ['პ'] = 'p',
            ['ჟ'] = 'J',
            ['რ'] = 'r',
            ['ს'] = 's',
            ['ტ'] = 't',
            ['უ'] = 'u',
            ['ფ'] = 'f',
            ['ქ'] = 'q',
            ['ღ'] = 'R',
            ['ყ'] = 'y',
            ['შ'] = 'S',
            ['ჩ'] = 'C',
            ['ც'] = 'c',
            ['ძ'] = 'Z',
            ['წ'] = 'w',
            ['ჭ'] = 'W',
            ['ხ'] = 'x',
            ['ჯ'] = 'j',
            ['ჰ'] = 'h',
        };

        public string GeoToLatin()
        {
            var charArray = str
                .Select(c => string.GeorgianMap.TryGetValue(c, out var latinChar) ? latinChar : c)
                .ToArray();
            return new string(charArray);
        }
        
        public string LatinToGeo()
        {
            var reverseMap = string.GeorgianMap.ToDictionary(kvp => kvp.Value, kvp => kvp.Key);
            var charArray = str
                .Select(c => reverseMap.TryGetValue(c, out var geoChar) ? geoChar : c)
                .ToArray();
            return new string(charArray);
        }

        public string FromEnv(string? defaultValue = null) =>
            Environment.GetEnvironmentVariable(str) ?? defaultValue ?? throw new InvalidOperationException($"'{str}' is not set.");
    }
}