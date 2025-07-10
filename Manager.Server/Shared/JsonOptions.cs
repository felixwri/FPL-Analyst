using System.Text.Json;

namespace Manager.Server.Shared
{
    public class JsonOptionsProvider
    {
        public static JsonSerializerOptions Options { get; } = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = true,
        };
    }
}
