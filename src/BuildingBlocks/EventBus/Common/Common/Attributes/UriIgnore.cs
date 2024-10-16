//2023 (c) TD Synnex - All Rights Reserved.

namespace Common.Attributes
{
    [AttributeUsage(AttributeTargets.Property |
                       AttributeTargets.Struct,
                       AllowMultiple = false)]
    public class UriIgnoreAttribute
        : Attribute
    {
    }
}
