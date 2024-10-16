//this code was originally released under open source project but is no 
//longer being maintained. See: https://github.com/billbogaiv/hybrid-model-binding
using System.Collections.Generic;

namespace HybridModelBinding
{
    public class HybridModelBinderOptions
    {
        public IEnumerable<string> FallbackBindingOrder { get; set; }
        public bool Passthrough { get; set; }
    }
}