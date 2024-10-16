//2023 (c) TD Synnex - All Rights Reserved.


namespace Configuration.Domain.SeedWork;

public interface IRepository<T> where T : IAggregateRoot
{
    IUnitOfWork UnitOfWork { get; }
}
