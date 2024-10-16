namespace VendorConfiguration.Domain.AggregatesModel
{
    /// <summary>
    /// Represents metadata about the creation and modification of an entity.
    /// </summary>
    public record ModificationInfo
    {
        /// <summary>
        /// Gets the name of the user who created the entity.
        /// </summary>
        public string? CreatedBy { get; private set; }

        /// <summary>
        /// Gets the name of the user who last updated the entity.
        /// </summary>
        public string? UpdatedBy { get; private set; }

        /// <summary>
        /// Gets the date and time when the entity was created.
        /// </summary>
        public DateTime? CreatedOn { get; private set; }

        /// <summary>
        /// Gets the date and time when the entity was last updated.
        /// </summary>
        public DateTime? UpdatedOn { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ModificationInfo"/> record.
        /// </summary>
        /// <param name="createdBy">The name of the user who created the entity.</param>
        /// <param name="updatedBy">The name of the user who last updated the entity.</param>
        /// <param name="createdOn">The date and time when the entity was created.</param>
        /// <param name="updatedOn">The date and time when the entity was last updated.</param>
        public ModificationInfo(string? createdBy, string? updatedBy, DateTime? createdOn, DateTime? updatedOn)
        {
            CreatedBy = createdBy;
            UpdatedBy = updatedBy;
            CreatedOn = createdOn;
            UpdatedOn = updatedOn;
        }
    }
}
