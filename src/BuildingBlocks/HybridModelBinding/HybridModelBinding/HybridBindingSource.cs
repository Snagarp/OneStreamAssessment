//this code was originally released under open source project but is no 
//longer being maintained. See: https://github.com/billbogaiv/hybrid-model-binding
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace HybridModelBinding
{
    public sealed class HybridBindingSource : BindingSource
    {
        public HybridBindingSource()
            : base("Hybrid", "Hybrid", true, true)
        { }

        public override bool CanAcceptDataFrom(BindingSource bindingSource) => bindingSource.Id == "Hybrid";
    }
}
