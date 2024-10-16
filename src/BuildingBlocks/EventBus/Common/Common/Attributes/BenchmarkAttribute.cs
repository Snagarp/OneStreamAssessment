//2023 (c) TD Synnex - All Rights Reserved.



using Microsoft.AspNetCore.Mvc.Filters;

using System.Diagnostics;

namespace Common.Attributes
{
    /// <summary>
    /// This attribute adds a response header 'x-time-elapsed' reporting the endpoint time in milliseconds.
    /// </summary>
    public class BenchmarkAttribute
      : ActionFilterAttribute
    {
        private const string TimingHeader = "x-time-elapsed";
        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (context is null) throw new ArgumentNullException(nameof(context));
            if (next is null) throw new ArgumentNullException(nameof(next));

            var timer = Stopwatch.StartNew();
            await next().ConfigureAwait(false);
            timer.Stop();
            context.HttpContext.Response.Headers.Add(TimingHeader, timer.Elapsed.ToString());
        }
    }
}
