namespace Common.Extensions
{
    /// <summary>
    /// Provides extension methods for working with generic types.
    /// </summary>
    public static class GenericTypeExtensions
    {
        /// <summary>
        /// Gets the full name of a generic type, including the type arguments.
        /// </summary>
        /// <param name="type">The <see cref="Type"/> to retrieve the name from.</param>
        /// <returns>A string representing the type name, including generic type arguments if applicable.</returns>
        public static string GetGenericTypeName(this Type type)
        {
            string typeName;

            switch (type.IsGenericType)
            {
                case true:
                    var genericTypes = string.Join(",", type.GetGenericArguments().Select(t => t.Name).ToArray());
                    typeName = $"{type.Name.Remove(type.Name.IndexOf('`'))}<{genericTypes}>";
                    break;
                default:
                    typeName = type.Name;
                    break;
            }

            return typeName;
        }

        /// <summary>
        /// Gets the full name of the object's type, including generic type arguments if applicable.
        /// </summary>
        /// <param name="obj">The object whose type name is being retrieved.</param>
        /// <returns>A string representing the object's type name, including generic type arguments if applicable.</returns>
        public static string GetGenericTypeName(this object obj) => obj.GetType().GetGenericTypeName();
    }
}
