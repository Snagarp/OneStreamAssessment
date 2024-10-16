//2023 (c) TD Synnex - All Rights Reserved.

using Common.Attributes;
using Common.Validation;

using System.Text;
using System.Linq;

namespace Common.Utils
{
    /// <summary>
    /// Util class that processes a query model object and adds populated members to the 
    /// APIPath and returns a Uri object for use with an HttpClient. No escaping is performed.
    /// </summary>
    public class ModelUriBuilder
        : IModelUriBuilder
    {
        public Uri Build(HttpClient client, string path, object model, bool suppressQueryParameters = false)
        {
            ArgumentGuard.NotNull(client, nameof(client));
            ArgumentGuard.NotNullOrEmpty(path, nameof(path));
            ArgumentGuard.NotNull(model, nameof(model));

            var baseAndPath = client.BaseAddress + path;
            var properties = model.GetType().GetProperties();
            var paramCount = 0;
            StringBuilder sb = new StringBuilder();
            foreach (var property in properties)
            {
                var key = "{" + property.Name + "}";

                object propertyValue = property.GetValue(model, null)!;

                if (propertyValue is null) continue;


                if (baseAndPath.Contains(key))
                {
                    baseAndPath = baseAndPath.Replace(key, propertyValue?.ToString());
                    continue;
                }

                var attributes = property.GetCustomAttributes(true);
                bool ignore = false;
                foreach (var _ in from Attribute attr in attributes.Cast<Attribute>()
                                  where attr is UriIgnoreAttribute
                                  select new { })
                {
                    // skip if the property has this attribute
                    ignore = true;
                    continue;
                }

                if (ignore) continue;

                var prefix = paramCount > 0 ? "&" : "?";
                sb.Append(prefix).Append(property.Name).Append('=').Append(propertyValue.ToString());
                paramCount++;

            }

            if (!suppressQueryParameters)
            {
                baseAndPath += sb.ToString();
            }

            return new Uri(baseAndPath);
        }

    }

}
