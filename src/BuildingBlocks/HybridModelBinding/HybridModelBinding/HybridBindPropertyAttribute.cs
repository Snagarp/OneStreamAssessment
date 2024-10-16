﻿//this code was originally released under open source project but is no 
//longer being maintained. See: https://github.com/billbogaiv/hybrid-model-binding
using System;
using System.Runtime.CompilerServices;

namespace HybridModelBinding
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true, Inherited = false)]
    public class HybridBindPropertyAttribute : Attribute
    {
        /// <param name="name">Provide alternate name within `valueProvider` to bind to this property.</param>
        /// <param name="order">Provide explicit order of binding when matched with other usages of `HybridPropertyAttribute` on the same property.</param>
        public HybridBindPropertyAttribute(
            string valueProvider,
            [CallerMemberName]string name = default,
            [CallerLineNumber]int order = default)
            : this(new[] { valueProvider }, name, order)
        { }

        /// <param name="name">Provide alternate name within all `valueProviders` to bind to this property.</param>
        /// <param name="order">Provide explicit order of binding when matched with other usages of `HybridPropertyAttribute` on the same property.</param>
        public HybridBindPropertyAttribute(
            string[] valueProviders,
            [CallerMemberName]string name = default,
            [CallerLineNumber]int order = default)
        {
            ValueProviders = valueProviders;
            Name = name;
            Order = order;
        }

        /// <param name="name">Provide alternate name within all `valueProviders` to bind to this property.</param>
        /// <param name="order">Provide explicit order of binding when matched with other usages of `HybridPropertyAttribute` on the same property.</param>
        protected HybridBindPropertyAttribute(
            HybridModelBinder.BindStrategy strategy,
            string[] valueProviders,
            [CallerMemberName]string name = default,
            [CallerLineNumber]int order = default)
            : this(valueProviders, name, order)
        {
            Strategy = strategy ?? throw new ArgumentNullException(nameof(strategy));
        }

        public string Name { get; private set; }
        public int? Order { get; private set; }
        public HybridModelBinder.BindStrategy Strategy { get; private set; }
        public string[] ValueProviders { get; private set; }
    }
}
