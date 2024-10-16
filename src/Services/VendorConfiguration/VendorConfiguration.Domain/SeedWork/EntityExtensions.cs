using VendorConfiguration.Domain.AggregatesModel;

namespace VendorConfiguration.Domain.SeedWork
{
    /// <summary>
    /// Provides extension methods for entities in the domain.
    /// </summary>
    public static class EntityExtensions
    {
        /// <summary>
        /// Retrieves the modification information (creation and update details) for an entity that contains modification data.
        /// </summary>
        /// <param name="entity">The entity from which to extract modification information.</param>
        /// <returns>A <see cref="ModificationInfo"/> object containing the created and updated details of the entity.</returns>
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="entity"/> is null.</exception>
        public static ModificationInfo GetModificationInfo(this EntityWithModificationInfo entity)
        {
            ArgumentGuard.NotNull(entity, nameof(entity));

            return new ModificationInfo(entity?.CreatedBy, entity?.UpdatedBy, entity?.CreatedOn, entity?.UpdatedOn);
        }
    }
}
