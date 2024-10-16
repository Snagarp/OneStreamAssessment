//2023 (c) TD Synnex - All Rights Reserved.


namespace Configuration.Infrastructure.Idempotency;

public class ClientRequest
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public DateTime Time { get; set; }
}
