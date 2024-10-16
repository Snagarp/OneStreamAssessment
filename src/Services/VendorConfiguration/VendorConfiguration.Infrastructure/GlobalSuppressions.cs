// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

using System.Diagnostics.CodeAnalysis;

[assembly: SuppressMessage("Design", "CA1040:Avoid empty interfaces", Justification = "Empty Iterfaces are used to tag objects", Scope = "type", Target = "~T:VendorConfiguration.Domain.SeedWork.IAggregateRoot")]
[assembly: SuppressMessage("Reliability", "CA2007:Consider calling ConfigureAwait on the awaited task", Justification = "<Pending>", Scope = "member", Target = "~M:VendorConfiguration.Infrastructure.Idempotency.RequestManager.ExistAsync(System.Guid)~System.Threading.Tasks.Task{System.Boolean}")]
[assembly: SuppressMessage("Reliability", "CA2007:Consider calling ConfigureAwait on the awaited task", Justification = "<Pending>", Scope = "member", Target = "~M:VendorConfiguration.Infrastructure.Idempotency.RequestManager.CreateRequestForCommandAsync``1(System.Guid)~System.Threading.Tasks.Task")]
