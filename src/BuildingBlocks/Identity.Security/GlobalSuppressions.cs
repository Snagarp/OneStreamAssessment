// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

using System.Diagnostics.CodeAnalysis;

[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Reliability", "CA2007:Consider calling ConfigureAwait on the awaited task", Justification = "suggestion is inappropriate for .net core")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Globalization", "CA1303:Do not pass literals as localized parameters", Justification = "too many to deal with right now - so many green lines obscures more important issues.")]
[assembly: SuppressMessage("Style", "IDE0060:Remove unused parameter", Justification = "<Pending>", Scope = "member", Target = "~M:Identity.Security.TokenValidators.PingIdentitySecurityTokenValidator.SetToCache(System.String,Identity.Security.TokenValidators.PingIdentitySecurityTokenValidator.ValidateResponse,System.String)")]
[assembly: SuppressMessage("Performance", "CA1859:Use concrete types when possible for improved performance", Justification = "<Pending>", Scope = "member", Target = "~F:Identity.Security.HttpClientService.Cache")]
[assembly: SuppressMessage("Usage", "CA2254:Template should be a static expression", Justification = "<Pending>", Scope = "member", Target = "~M:Identity.Security.Attributes.PingFederateAuthAttribute.OnActionExecutionAsync(Microsoft.AspNetCore.Mvc.Filters.ActionExecutingContext,Microsoft.AspNetCore.Mvc.Filters.ActionExecutionDelegate)~System.Threading.Tasks.Task")]
[assembly: SuppressMessage("Usage", "CA2254:Template should be a static expression", Justification = "<Pending>", Scope = "member", Target = "~M:Identity.Security.Caching.DistributedCacheProvider.Get``1(System.String)~``0")]
[assembly: SuppressMessage("Usage", "CA2254:Template should be a static expression", Justification = "<Pending>", Scope = "member", Target = "~M:Identity.Security.Caching.DistributedCacheProvider.Set``1(System.String,``0,System.Nullable{System.TimeSpan})")]
