namespace VendorConfiguration.Domain.SeedWork;

public abstract class Entity
{
    int? _requestedHashCode;
    int _id;


    protected virtual int Id
    {
        get
        {
            return _id;
        }
        set
        {
            _id = value;
        }
    }

    private List<INotification> _domainEvents;
    public IReadOnlyCollection<INotification> DomainEvents =>
        _domainEvents?.AsReadOnly() ?? new List<INotification>().AsReadOnly();

    public void AddDomainEvent(INotification eventItem)
    {
        ArgumentGuard.NotNull(eventItem, nameof(eventItem));

        _domainEvents ??= new List<INotification>();
        _domainEvents.Add(eventItem);
    }

    public void RemoveDomainEvent(INotification eventItem)
    {
        ArgumentGuard.NotNull(eventItem, nameof(eventItem));

        _domainEvents?.Remove(eventItem);
    }

    public void ClearDomainEvents() => _domainEvents?.Clear();

    public bool IsTransient() => this.Id == default;

    public override bool Equals(object? obj)
    {
        if (obj is null || obj is not Entity)
            return false;

        if (Object.ReferenceEquals(this, obj))
            return true;

        if (this.GetType() != obj.GetType())
            return false;

        if (obj is not Entity item) return false;

        if (item.IsTransient() || this.IsTransient())
            return false;
        else
            return item.Id == this.Id;
    }

    public override int GetHashCode()
    {
        if (!IsTransient())
        {
            switch (_requestedHashCode)
            {
                case null:
                    _requestedHashCode = this.Id.GetHashCode() ^ 31; // XOR for random distribution (http://blogs.msdn.com/b/ericlippert/archive/2011/02/28/guidelines-and-rules-for-gethashcode.aspx)
                    break;
            }

            return _requestedHashCode.Value;
        }
        else
            return base.GetHashCode();

    }
    public static bool operator ==(Entity left, Entity right)
    {
        ArgumentGuard.NotNull(left, nameof(left));
        ArgumentGuard.NotNull(right, nameof(right));

        return left switch
        {
            null => (Object.Equals(right, null)),
            _ => left.Equals(right),
        };
    }

    public static bool operator !=(Entity left, Entity right)
    {
        ArgumentGuard.NotNull(left, nameof(left));
        ArgumentGuard.NotNull(right, nameof(right));

        return !(left == right);
    }

    protected Entity()
    {
        _domainEvents = new List<INotification>();
    }
}
