using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Common.Validation
{
    /// <summary>
    /// Validates JSON data using basic validation rules
    /// </summary>
    public static class JsonValidator
    {
        /// <summary>Validates the specified JSON string for safe text.</summary>
        /// <param name="json">The JSON as string.</param>
        /// <param name="maxSize">The maximum length of string items.</param>
        /// <returns>True if JSON is valid</returns>
        public static bool IsSafeText(string json, int maxSize = 1024)
        {
            if (string.IsNullOrWhiteSpace(json))
            {
                return true;
            }

            JToken jToken;

            using (var stringReader = new StringReader(json))
            using (var jsonReader = new JsonTextReader(stringReader))
            {

                try
                {
                    jToken = JToken.Load(jsonReader);
                }
                catch (JsonReaderException)
                {
                    return false;
                }
            }

            return jToken is JObject jObject && IsSafeText(jObject, maxSize);
        }

        /// <summary>Validates specified JSON content for safe text.</summary>
        /// <param name="json">The JSON object.</param>
        /// <param name="maxSize">The maximum length of string items.</param>
        /// <returns><c>true</c> if the specified json is valid; otherwise, <c>false</c>.</returns>
        public static bool IsSafeText(JObject json, int maxSize = 1024)
        {
            ArgumentGuard.NotNull(json, nameof(json));

            return ValidateToken(json, DataValidator.IsSafeText, maxSize);
        }
        private static bool ValidateToken(JToken token, Func<string, int, int, bool> validation, int maxSize)
        {
            switch (token)
            {
                case JValue jValue when jValue.Type == JTokenType.String:
                    var value = jValue.Value<string>();
                    return string.IsNullOrEmpty(value) || validation(value, 1, maxSize);

                case JObject obj:
                    return obj.Properties().All(property => ValidateToken(property.Value, validation, maxSize));

                case JArray array:
                    return array.All(item => ValidateToken(item, validation, maxSize));

                default:
                    return true;
            }
        }
    }
}
