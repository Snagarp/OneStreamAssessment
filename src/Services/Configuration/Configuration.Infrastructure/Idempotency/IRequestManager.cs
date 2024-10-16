//2023 (c) TD Synnex - All Rights Reserved.


namespace Configuration.Infrastructure.Idempotency;

public interface IRequestManager
{
    Task<bool> ExistAsync(Guid id);

    Task CreateRequestForCommandAsync<T>(Guid id);
}
