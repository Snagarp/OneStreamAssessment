//2023 (c) TD Synnex - All Rights Reserved.

#pragma warning disable CA1304, CS8602

using System.Text.Json;
using System.Text.Json.Serialization;

namespace Common.JsonConverters
{
    public class UnderscoreToCamelCaseConverter<TEnum>
        : JsonConverter<TEnum> where TEnum : Enum
    {

        public override TEnum Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            // Not needed for deserialization
            throw new NotImplementedException();
        }

        public override void Write(Utf8JsonWriter writer, TEnum value, JsonSerializerOptions options)
        {
            // Convert enum value to underscore-separated string
            string underscoreString = UnderscoreToCamelCaseConverter<TEnum>.ConvertToUnderscoreString(value);
            writer.WriteStringValue(underscoreString);
        }

        private static string ConvertToUnderscoreString(TEnum value)
        {
            return Enum.GetName(typeof(TEnum), value)
                .Replace(" ", "_") // Replace spaces with underscores
                .ToLower(); // Convert to lowercase
        }
    }
}

