using VendorConfiguration.Domain.AggregatesModel.CountryAggregate;

namespace VendorConfiguration.Infrastructure.EntityConfigurations
{
    /// <summary>
    /// Configures the entity mapping for the <see cref="Country"/> entity in the database.
    /// </summary>
    public class CountryConfiguration : IEntityTypeConfiguration<Country>
    {
        /// <summary>
        /// Configures the properties and relationships for the <see cref="Country"/> entity.
        /// </summary>
        /// <param name="builder">The builder used to configure the <see cref="Country"/> entity.</param>
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="builder"/> is null.</exception>
        public void Configure(EntityTypeBuilder<Country> builder)
        {
            switch (builder)
            {
                case null:
                    throw new ArgumentNullException(nameof(builder));
            }

            // Map the Country entity to the "Country" table in the default schema
            builder.ToTable("Country", VendorConfigurationContext.DEFAULTSCHEMA)
                .HasKey(builder => builder.CountryId);

            // Configure the CountryId property
            builder.Property(builder => builder.CountryId)
                .UsePropertyAccessMode(PropertyAccessMode.Property)
                .IsRequired()
                .HasColumnName("CountryId");

            // Configure the CountryName property
            builder.Property(builder => builder.CountryName)
                .UsePropertyAccessMode(PropertyAccessMode.Field)
                .IsRequired()
                .HasColumnName("CountryName");

            // Configure the Iso3166CountryCode2 property
            builder.Property(builder => builder.Iso3166CountryCode2)
                .UsePropertyAccessMode(PropertyAccessMode.Field)
                .IsRequired()
                .HasColumnName("Iso3166_countryCode_2");

            // Configure the Iso3166CountryCode3 property
            builder.Property(builder => builder.Iso3166CountryCode3)
                .UsePropertyAccessMode(PropertyAccessMode.Field)
                .IsRequired()
                .HasColumnName("Iso3166_countryCode_3");
        }
    }
}
