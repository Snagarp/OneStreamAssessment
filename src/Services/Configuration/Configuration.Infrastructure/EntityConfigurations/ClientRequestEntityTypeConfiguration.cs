//2023 (c) TD Synnex - All Rights Reserved.


namespace Configuration.Infrastructure.EntityConfigurations;

class ClientRequestEntityTypeConfiguration
    : IEntityTypeConfiguration<ClientRequest>
{
    public void Configure(EntityTypeBuilder<ClientRequest> requestConfiguration)
    {
        requestConfiguration.ToTable("requests", ConfigurationContext.DEFAULT_SCHEMA);
        requestConfiguration.HasKey(cr => cr.Id);
        requestConfiguration.Property(cr => cr.Name).IsRequired();
        requestConfiguration.Property(cr => cr.Time).IsRequired();
    }
}
