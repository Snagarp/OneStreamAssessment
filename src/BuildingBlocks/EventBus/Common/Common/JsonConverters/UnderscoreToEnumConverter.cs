//2023 (c) TD Synnex - All Rights Reserved.

#pragma warning disable CA1304, CS8602
using Common.Validation;

using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Common.JsonConverters
{
    public class UnderscoreToEnumConverter<TEnum>
        : JsonConverter<TEnum> where TEnum : Enum
    {
        public override TEnum Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            

            string underscoreString = reader.GetString()!;
            string camelCaseString = UnderscoreToEnumConverter<TEnum>.ConvertToTitleCase(underscoreString);

            if (Enum.TryParse(typeof(TEnum), camelCaseString, false, out var enumValue))
            {
                return (TEnum) enumValue;
            }
            else
            {
                throw new JsonException($"Invalid enum value: {underscoreString}");
            }
        }

        public override void Write(Utf8JsonWriter writer, TEnum value, JsonSerializerOptions options)
        {
            ArgumentGuard.NotNull(writer, nameof(writer));

            string underscoreString = UnderscoreToEnumConverter<TEnum>.ConvertToUnderscoreSeparated(value.ToString());
            writer.WriteStringValue(underscoreString);
        }

        private static string ConvertToUnderscoreSeparated(string titleCaseString)
        {
            var result = new StringBuilder();

            for (int i = 0; i < titleCaseString.Length; i++)
            {
                char currentChar = titleCaseString[i];

                // Check if the character is uppercase (excluding the first character)
                if (i > 0 && char.IsUpper(currentChar))
                {
                    // Append an underscore before the uppercase letter
                    result.Append('_');
                }

                // Convert the character to lowercase and append
                result.Append(char.ToLower(currentChar));
            }

            return result.ToString();
        }

        private static string ConvertToTitleCase(string input)
        {
            string[] words = input.Split('_');
            for (int i = 0; i < words.Length; i++)
            {
                words[i] = char.ToUpper(words[i][0]) + words[i].Substring(1).ToLower();
            }
            return string.Join("", words);
        }
    }
}

