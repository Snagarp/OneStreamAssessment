//this code was originally released under open source project but is no 
//longer being maintained. See: https://github.com/billbogaiv/hybrid-model-binding
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using System.Collections.Generic;

namespace HybridModelBinding
{
    public class DefaultHybridModelBinderProvider : HybridModelBinderProvider
    {
        public DefaultHybridModelBinderProvider(
            IList<IInputFormatter> formatters,
            IHttpRequestStreamReaderFactory readerFactory,
            IEnumerable<string> fallbackBindingOrder)
            : base(
                 new HybridBindingSource(),
                 new DefaultHybridModelBinder(formatters, readerFactory, fallbackBindingOrder))
        { }
    }
}
