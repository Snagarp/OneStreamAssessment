using Common.Attributes;
using Common.Validation;
using System.Text;

namespace Common.Utils
{
    /// <summary>
    /// Util class that processes a query model object and adds populated members to the 
    /// APIPath and returns a Uri object for use with an HttpClient. No escaping is performed.
    /// </summary>
    public class ModelUriBuilder
        : IModelUriBuilder
    {

        /// <summary>
        /// Builds a URI for the specified HTTP client and path based on the provided model.
        /// </summary>
        /// <param name="client">The HTTP client used to generate the base URI.</param>
        /// <param name="path">The relative path or endpoint for the API call.</param>
        /// <param name="model">The model object whose properties may be included as query parameters in the URI.</param>
        /// <param name="suppressQueryParameters">If set to <c>true</c>, query parameters will be suppressed in the URI.</param>
        /// <returns>A <see cref="Uri"/> object representing the full request URI.</returns>
        public Uri Build(HttpClient client, string path, object model, bool suppressQueryParameters = false)
        {
            ArgumentGuard.NotNull(client, nameof(client));
            ArgumentGuard.NotNullOrEmpty(path, nameof(path));
            ArgumentGuard.NotNull(model, nameof(model));

            var baseAndPath = client.BaseAddress + path;
            var properties = model.GetType().GetProperties();
            var paramCount = 0;
            StringBuilder sb = new();
            foreach (var property in properties)
            {
                var key = "{" + property.Name + "}";

                object propertyValue = property.GetValue(model, null)!;

                switch (propertyValue)
                {
                    case null:
                        continue;
                }
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

                switch (ignore)
                {
                    case true:
                        continue;
                }

                var prefix = paramCount > 0 ? "&" : "?";
                sb.Append(prefix).Append(property.Name).Append('=').Append(propertyValue.ToString());
                paramCount++;

            }

            switch (suppressQueryParameters)
            {
                case false:
                    baseAndPath += sb.ToString();
                    break;
            }
            return new Uri(baseAndPath);
        }
    }
}
