//2023 (c) TD Synnex - All Rights Reserved.

using Common.Binding;

using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Common.Attributes
{
    public class FromMultiSourceAttribute
        : Attribute, IBindingSourceMetadata
    {
        public BindingSource BindingSource => new FromRequestModelBindingSource();
    }
}
