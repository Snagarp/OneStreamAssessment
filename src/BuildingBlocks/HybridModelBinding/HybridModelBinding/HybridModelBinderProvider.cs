//this code was originally released under open source project but is no 
//longer being maintained. See: https://github.com/billbogaiv/hybrid-model-binding
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
#pragma warning disable CA1510

namespace HybridModelBinding
{
    public abstract class HybridModelBinderProvider : IModelBinderProvider
    {
        public HybridModelBinderProvider(
            BindingSource bindingSource,
            IModelBinder modelBinder)
        {
            if (bindingSource == null)
            {
                throw new ArgumentNullException(nameof(bindingSource));
            }

            switch (modelBinder)
            {
                case null:
                    throw new ArgumentNullException(nameof(modelBinder));
                default:
                    this.BindingSource = bindingSource;
                    this.ModelBinder = modelBinder;
                    break;
            }
        }

        private BindingSource BindingSource { get; set; }
        private IModelBinder ModelBinder { get; set; }

        public virtual IModelBinder GetBinder(ModelBinderProviderContext context)
        {
            if (context.BindingInfo?.BindingSource != null &&
                context.BindingInfo.BindingSource.CanAcceptDataFrom(BindingSource))
            {
                return ModelBinder;
            }
            else
            {
                return null;
            }
        }
    }
}
