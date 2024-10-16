//this code was originally released under open source project but is no 
//longer being maintained. See: https://github.com/billbogaiv/hybrid-model-binding
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using System.Collections.Generic;

namespace HybridModelBinding
{
    public class DefaultPassthroughHybridModelBinderProvider : HybridModelBinderProvider
    {
        public DefaultPassthroughHybridModelBinderProvider(
            IList<IInputFormatter> formatters,
            IHttpRequestStreamReaderFactory readerFactory,
            IEnumerable<string> fallbackBindingOrder)
            : base(
                 new HybridBindingSource(),
                 new DefaultPassthroughHybridModelBinder(formatters, readerFactory, fallbackBindingOrder))
        { }
    }
}
