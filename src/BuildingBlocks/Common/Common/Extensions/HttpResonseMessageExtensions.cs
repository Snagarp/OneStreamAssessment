using Common.Validation;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Common.Extensions
{
    /// <summary>
    /// Provides extension methods for working with <see cref="HttpResponseMessage"/> and deserializing JSON content.
    /// </summary>
    public static partial class HttpResonseMessageExtensions
    {
        /// <summary>
        /// Deserializes the JSON content of an <see cref="HttpResponseMessage"/> into an object of type <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">The type to which the JSON content should be deserialized.</typeparam>
        /// <param name="message">The <see cref="HttpResponseMessage"/> containing the JSON content.</param>
        /// <param name="options">Optional <see cref="JsonSerializerOptions"/> to customize the deserialization process.</param>
        /// <returns>The deserialized object of type <typeparamref name="T"/>, or <c>null</c> if deserialization fails or content is empty.</returns>
        /// <exception cref="ArgumentNullException">Thrown if the <paramref name="message"/> is null.</exception>
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

                return JsonSerializer.Deserialize<T>(str, options);
            }
            return default;
        }
    }
}
