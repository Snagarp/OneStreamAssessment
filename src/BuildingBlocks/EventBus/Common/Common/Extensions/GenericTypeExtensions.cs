//2023 (c) TD Synnex - All Rights Reserved.
#pragma warning disable CA1062

namespace Common.Extensions
{
    public static class GenericTypeExtensions
    {
        public static string GetGenericTypeName(this Type type)
        {
            string typeName;

            if (type.IsGenericType)
            {
                var genericTypes = string.Join(",", type.GetGenericArguments().Select(t => t.Name).ToArray());
                typeName = $"{type.Name.Remove(type.Name.IndexOf('`'))}<{genericTypes}>";
            }
            else
            {
                typeName = type.Name;
            }

            return typeName;
        }

        public static string GetGenericTypeName(this object obj)
        {
            return obj.GetType().GetGenericTypeName();
        }
    }

}
