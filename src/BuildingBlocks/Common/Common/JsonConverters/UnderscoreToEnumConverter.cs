#pragma warning disable CA1304, CS8602
using Common.Validation;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Common.JsonConverters
{
    /// <summary>
    /// A JSON converter that converts between an enum and an underscore-separated string.
    /// </summary>
    /// <typeparam name="TEnum">The enum type being converted.</typeparam>
    public class UnderscoreToEnumConverter<TEnum> : JsonConverter<TEnum> where TEnum : Enum
    {
        /// <summary>
        /// Reads the JSON data as an underscore-separated string and converts it to the corresponding enum value.
        /// </summary>
        /// <param name="reader">The <see cref="Utf8JsonReader"/> to read JSON data from.</param>
        /// <param name="typeToConvert">The type being converted (enum).</param>
        /// <param name="options">Serialization options for JSON processing.</param>
        /// <returns>The corresponding enum value.</returns>
        /// <exception cref="JsonException">Thrown when the provided string cannot be parsed into an enum value.</exception>
        public override TEnum Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            // Get the string value from JSON
            string underscoreString = reader.GetString()!;

            // Convert the underscore-separated string to camelCase
            string camelCaseString = UnderscoreToEnumConverter<TEnum>.ConvertToTitleCase(underscoreString);

            // Try to parse the camelCase string to the enum value
            if (Enum.TryParse(typeof(TEnum), camelCaseString, false, out var enumValue))
            {
                return (TEnum)enumValue;
            }
            else
            {
                throw new JsonException($"Invalid enum value: {underscoreString}");
            }
        }

        /// <summary>
        /// Writes the enum value as an underscore-separated string in JSON format.
        /// </summary>
        /// <param name="writer">The <see cref="Utf8JsonWriter"/> to write JSON data to.</param>
        /// <param name="value">The enum value to write.</param>
        /// <param name="options">Serialization options for JSON processing.</param>
        public override void Write(Utf8JsonWriter writer, TEnum value, JsonSerializerOptions options)
        {
            ArgumentGuard.NotNull(writer, nameof(writer));

            // Convert the enum value to an underscore-separated string
            string underscoreString = UnderscoreToEnumConverter<TEnum>.ConvertToUnderscoreSeparated(value.ToString());

            // Write the string value to JSON
            writer.WriteStringValue(underscoreString);
        }

        /// <summary>
        /// Converts a camelCase string to an underscore-separated lowercase string.
        /// </summary>
        /// <param name="titleCaseString">The camelCase string to convert.</param>
        /// <returns>The underscore-separated lowercase string.</returns>
        private static string ConvertToUnderscoreSeparated(string titleCaseString)
        {
            var result = new StringBuilder();

            for (int i = 0; i < titleCaseString.Length; i++)
            {
                char currentChar = titleCaseString[i];

                // Add an underscore before uppercase letters (except the first character)
                if (i > 0 && char.IsUpper(currentChar))
                {
                    result.Append('_');
                }

                // Convert the character to lowercase and append
                result.Append(char.ToLower(currentChar));
            }

            return result.ToString();
        }

        /// <summary>
        /// Converts an underscore-separated string to title case (camelCase).
        /// </summary>
        /// <param name="input">The underscore-separated string.</param>
        /// <returns>The title case string.</returns>
        private static string ConvertToTitleCase(string input)
        {
            var words = input.Split('_');
            for (var i = 0; i < words.Length; i++)
            {
                words[i] = char.ToUpper(words[i][0]) + words[i][1..].ToLower();
            }
            return string.Join("", words);
        }
    }
}
