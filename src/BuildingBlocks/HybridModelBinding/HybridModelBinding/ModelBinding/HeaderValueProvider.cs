//this code was originally released under open source project but is no 
//longer being maintained. See: https://github.com/billbogaiv/hybrid-model-binding
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Internal;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace HybridModelBinding.ModelBinding
{
    /// <summary>
    /// Modified from https://github.com/aspnet/Mvc/blob/8d66f104f7f2ca42ee8b21f75b0e2b3e1abe2e00/src/Microsoft.AspNetCore.Mvc.Core/ModelBinding/QueryStringValueProvider.cs.
    /// </summary>
    public class HeaderValueProvider : BindingSourceValueProvider, IEnumerableValueProvider
    {
        public HeaderValueProvider(
            BindingSource bindingSource,
            IHeaderDictionary values,
            CultureInfo culture)
            : base(bindingSource)
        {
            if (bindingSource != null)
            {
                this.values = values ?? throw new ArgumentNullException(nameof(values));
                Culture = culture;
            }
            else
            {
                throw new ArgumentNullException(nameof(bindingSource));
            }
        }

        public CultureInfo Culture { get; private set; }

        private readonly IHeaderDictionary values;
        private PrefixContainer prefixContainer;

        protected PrefixContainer PrefixContainer
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

        public override bool ContainsPrefix(string prefix) => PrefixContainer.ContainsPrefix(prefix);

        public virtual IDictionary<string, string> GetKeysFromPrefix(string prefix) => prefix switch
        {
            null => throw new ArgumentNullException(nameof(prefix)),
            _ => PrefixContainer.GetKeysFromPrefix(prefix),
        };

        public override ValueProviderResult GetValue(string key)
        {
            switch (key)
            {
                case null:
                    throw new ArgumentNullException(nameof(key));
            }

            var values = this.values[key];

            return values.Count switch
            {
                0 => ValueProviderResult.None,
                _ => new ValueProviderResult(values, Culture),
            };
        }
    }
}
