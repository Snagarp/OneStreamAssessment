//2023 (c) TD Synnex - All Rights Reserved.


using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Common.Binding
{
    public class FromRequestModelBindingSource
        : BindingSource
    {
        public FromRequestModelBindingSource()
            : base("FromModelBinding", "FromModelBinding", true, true) { }

        public override bool CanAcceptDataFrom(BindingSource bindingSource) =>
            bindingSource == ModelBinding;

    }
}
