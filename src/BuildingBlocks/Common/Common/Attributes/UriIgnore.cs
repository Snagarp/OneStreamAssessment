namespace Common.Attributes
{
    /// <summary>
    /// Specifies that a property or struct should be ignored when constructing a URI.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Struct, AllowMultiple = false)]
    public class UriIgnoreAttribute : Attribute
    {
    }
}
