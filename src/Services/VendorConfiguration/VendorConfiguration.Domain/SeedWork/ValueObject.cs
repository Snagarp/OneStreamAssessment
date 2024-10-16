namespace VendorConfiguration.Domain.SeedWork
{
    /// <summary>
    /// Represents the base class for value objects, providing equality comparison based on their components.
    /// </summary>
    public abstract class ValueObject
    {
        /// <summary>
        /// Determines whether two value objects are equal using the equality operator.
        /// </summary>
        /// <param name="left">The left value object.</param>
        /// <param name="right">The right value object.</param>
        /// <returns>true if both value objects are equal; otherwise, false.</returns>
        protected static bool EqualOperator(ValueObject left, ValueObject right) => (left is null ^ right is null) switch
        {
            true => false,
            _ => left is null || left.Equals(right),
        };

        /// <summary>
        /// Determines whether two value objects are not equal using the inequality operator.
        /// </summary>
        /// <param name="left">The left value object.</param>
        /// <param name="right">The right value object.</param>
        /// <returns>true if both value objects are not equal; otherwise, false.</returns>
        protected static bool NotEqualOperator(ValueObject left, ValueObject right) => !(EqualOperator(left, right));

        /// <summary>
        /// Provides the components of the value object used for equality comparison.
        /// Derived classes must implement this method to specify the components.
        /// </summary>
        /// <returns>An enumerable of objects representing the components of the value object.</returns>
        protected abstract IEnumerable<object> GetEqualityComponents();

        /// <summary>
        /// Determines whether the specified object is equal to the current value object.
        /// </summary>
        /// <param name="obj">The object to compare with the current value object.</param>
        /// <returns>true if the specified object is equal to the current value object; otherwise, false.</returns>
        public override bool Equals(object? obj)
        {
            if (obj == null || obj.GetType() != GetType())
            {
                return false;
            }

            var other = (ValueObject)obj;

            return this.GetEqualityComponents().SequenceEqual(other.GetEqualityComponents());
        }

        /// <summary>
        /// Returns the hash code for the current value object based on its equality components.
        /// </summary>
        /// <returns>An integer representing the hash code of the value object.</returns>
        public override int GetHashCode() => GetEqualityComponents()
            .Select(x => x != null ? x.GetHashCode() : 0)
            .Aggregate((x, y) => x ^ y);

        /// <summary>
        /// Creates a shallow copy of the current value object.
        /// </summary>
        /// <returns>A copy of the current value object.</returns>
        public ValueObject? GetCopy() => MemberwiseClone() as ValueObject;
    }
}
