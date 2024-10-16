namespace VendorConfiguration.Domain.SeedWork
{
    public abstract class EntityWithModificationInfo
        : Entity
    {
        public string? CreatedBy { get; protected set; }

        public string? UpdatedBy { get; protected set; }

        public DateTime? CreatedOn { get; protected set; }

        public DateTime? UpdatedOn { get; protected set; }
    }
}
