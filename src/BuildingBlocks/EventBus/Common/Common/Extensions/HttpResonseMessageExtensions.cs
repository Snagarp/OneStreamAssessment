//2023 (c) TD Synnex - All Rights Reserved.

using Common.Validation;

using System.Text.Json;
using System.Text.Json.Serialization;

namespace Common.Extensions
{
    public static partial class HttpResonseMessageExtensions
    {
        public static async Task<T?> AsJsonAsync<T>(this HttpResponseMessage message, JsonSerializerOptions? options = null) where T : class, new()
        {
            ArgumentGuard.NotNull(message, nameof(message));

            options ??= new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = true,

            };
            options.Converters.Add(new JsonStringEnumConverter());

            if (message.IsSuccessStatusCode)
            {
                var str = await message.Content.ReadAsStringAsync();
                if (string.IsNullOrEmpty(str)) return null;

                return JsonSerializer.Deserialize<T>(str, options)!;
            }

            return default;
        }
    }
}
