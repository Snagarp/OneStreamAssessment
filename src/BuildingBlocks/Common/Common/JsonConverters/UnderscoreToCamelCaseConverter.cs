#pragma warning disable CA1304, CS8602
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Common.JsonConverters
{
    /// <summary>
    /// A JSON converter that converts enum values to underscore-separated lowercase strings.
    /// </summary>
    /// <typeparam name="TEnum">The enum type that is being converted.</typeparam>
    public class UnderscoreToCamelCaseConverter<TEnum> : JsonConverter<TEnum> where TEnum : Enum
    {
        /// <summary>
        /// This method is not implemented as the converter is intended for serialization only.
        /// </summary>
        /// <param name="reader">The JSON reader.</param>
        /// <param name="typeToConvert">The type to convert.</param>
        /// <param name="options">Serialization options.</param>
        /// <returns>Not implemented.</returns>
        /// <exception cref="NotImplementedException">Always throws this exception, as deserialization is not supported.</exception>
        public override TEnum Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) =>
            throw new NotImplementedException();

        /// <summary>
        /// Writes the enum value as an underscore-separated lowercase string to JSON.
        /// </summary>
        /// <param name="writer">The JSON writer.</param>
        /// <param name="value">The enum value to convert and write.</param>
        /// <param name="options">Serialization options.</param>
        public override void Write(Utf8JsonWriter writer, TEnum value, JsonSerializerOptions options)
        {
            // Convert enum value to underscore-separated string
            string underscoreString = ConvertToUnderscoreString(value);
            writer.WriteStringValue(underscoreString);
        }

        /// <summary>
        /// Converts an enum value to an underscore-separated lowercase string.
        /// </summary>
        /// <param name="value">The enum value to convert.</param>
        /// <returns>A string representation of the enum value, with spaces replaced by underscores and converted to lowercase.</returns>
        private static string ConvertToUnderscoreString(TEnum value) =>
            Enum.GetName(typeof(TEnum), value)
                .Replace(" ", "_") // Replace spaces with underscores
                .ToLower(); // Convert to lowercase
    }
}
