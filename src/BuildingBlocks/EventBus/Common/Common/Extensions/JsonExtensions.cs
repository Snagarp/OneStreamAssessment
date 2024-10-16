//2023 (c) TD Synnex - All Rights Reserved.

using Common.Validation;

using System.Text.Json;

namespace Common.Extensions
{
    public static partial class JsonExtensions
    {
        public static JsonElement? Get(this JsonElement element, string name) =>
            ArgumentGuard.NotNull(element).ValueKind != JsonValueKind.Null && element.ValueKind != JsonValueKind.Undefined && element.TryGetProperty(name, out var value)
            ? value : (JsonElement?)null;

        public static JsonElement? Get(this JsonElement element, int index)
        {
            if (ArgumentGuard.NotNull(element).ValueKind != JsonValueKind.Null && element.ValueKind != JsonValueKind.Undefined)
                return null;
            return index < element.GetArrayLength() ? element[index] : null;
        }
    }
}
