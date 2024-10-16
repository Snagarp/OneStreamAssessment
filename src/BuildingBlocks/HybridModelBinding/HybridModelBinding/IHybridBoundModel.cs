//this code was originally released under open source project but is no 
//longer being maintained. See: https://github.com/billbogaiv/hybrid-model-binding
using System.Collections.Generic;

namespace HybridModelBinding
{
    public interface IHybridBoundModel
    {
        IDictionary<string, string> HybridBoundProperties { get; }

        internal (bool propertyIsBound, string source) IsBound(string name) => HybridBoundProperties.TryGetValue(name, out string value) ? (true, value)
            : (false, string.Empty);
    }
}
