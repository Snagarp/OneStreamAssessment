//this code was originally released under open source project but is no 
//longer being maintained. See: https://github.com/billbogaiv/hybrid-model-binding
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;

namespace HybridModelBinding
{
    [AttributeUsage(AttributeTargets.Parameter, AllowMultiple = false)]
    public class FromHybridAttribute : Attribute, IBindingSourceMetadata
    {
        public FromHybridAttribute()
        { }

        public FromHybridAttribute(string[] defaultBindingOrder)
        {
            DefaultBindingOrder = defaultBindingOrder ?? throw new ArgumentNullException(nameof(defaultBindingOrder));
        }

        public BindingSource BindingSource => new HybridBindingSource();
        public string[] DefaultBindingOrder { get; private set; }
    }
}
