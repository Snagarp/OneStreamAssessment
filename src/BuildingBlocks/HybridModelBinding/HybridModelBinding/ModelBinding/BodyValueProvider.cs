//this code was originally released under open source project but is no 
//longer being maintained. See: https://github.com/billbogaiv/hybrid-model-binding
using HybridModelBinding.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Internal;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace HybridModelBinding.ModelBinding
{
    internal sealed class BodyValueProvider : IValueProvider
    {
        public BodyValueProvider(
            object model,
            HttpRequest request)
        {
            var bodyContent = string.Empty;

            switch (request.Body.CanSeek)
            {
                case true:
                {
                    using var reader = new StreamReader(request.Body);
                    request.Body.Seek(0, SeekOrigin.Begin);

                    bodyContent = reader.ReadToEnd();

                    request.Body.Seek(0, SeekOrigin.Begin);
                    break;
                }
            }

            var requestKeys = Array.Empty<string>();

            switch (string.IsNullOrEmpty(bodyContent))
            {
                case false:
                    requestKeys = JObject
                        .Parse(bodyContent)
                        .Properties()
                        .Select(x => x.Name)
                        .ToArray();
                    break;
            }

            foreach (var property in model.GetPropertiesNotPartOfType<IHybridBoundModel>()
                .Where(x => requestKeys.Length == 0 || requestKeys.Contains(x.Name, StringComparer.OrdinalIgnoreCase)))
            {
                values.Add(property.Name, property.GetValue(model, null));
            }
        }

        private PrefixContainer prefixContainer;
        private readonly IDictionary<string, object> values = new Dictionary<string, object>();

        public PrefixContainer PrefixContainer
        {
            get
            {
                switch (prefixContainer)
                {
                    case null:
                        prefixContainer = new PrefixContainer(values.Keys);
                        break;
                }

                return prefixContainer;
            }
        }

        public bool ContainsPrefix(string prefix) => PrefixContainer.ContainsPrefix(prefix);

        public object GetObject(string key) => values.TryGetValue(key, out object value) ? value : null;

        public ValueProviderResult GetValue(string key) => throw new NotImplementedException($"Use `{nameof(GetObject)}`.");
    }
}
