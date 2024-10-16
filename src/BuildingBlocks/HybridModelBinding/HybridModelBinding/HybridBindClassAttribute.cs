//this code was originally released under open source project but is no 
//longer being maintained. See: https://github.com/billbogaiv/hybrid-model-binding
using System;

namespace HybridModelBinding
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class HybridBindClassAttribute : Attribute
    {
        public HybridBindClassAttribute(string[] defaultBindingOrder)
        {
            DefaultBindingOrder = defaultBindingOrder ?? throw new ArgumentNullException(nameof(defaultBindingOrder));
        }

        public string[] DefaultBindingOrder { get; private set; }
    }
}
