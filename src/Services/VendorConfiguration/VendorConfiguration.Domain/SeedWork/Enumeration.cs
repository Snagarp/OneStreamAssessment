#pragma warning disable CA1062, CA1036, CA1724

namespace VendorConfiguration.Domain.SeedWork
{
    /// <summary>
    /// Provides a base class for creating strongly-typed enumerations.
    /// </summary>
    public abstract class Enumeration : IComparable
    {
        /// <summary>
        /// Gets the name of the enumeration.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Gets the unique identifier of the enumeration.
        /// </summary>
        public int Id { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Enumeration"/> class with the specified id and name.
        /// </summary>
        /// <param name="id">The unique identifier of the enumeration.</param>
        /// <param name="name">The name of the enumeration.</param>
        protected Enumeration(int id, string name) => (Id, Name) = (id, name);

        /// <summary>
        /// Returns the name of the enumeration as a string.
        /// </summary>
        /// <returns>A string representing the name of the enumeration.</returns>
        public override string ToString() => Name;

        /// <summary>
        /// Retrieves all instances of a specific enumeration type.
        /// </summary>
        /// <typeparam name="T">The type of the enumeration.</typeparam>
        /// <returns>A collection of all instances of the specified enumeration type.</returns>
        public static IEnumerable<T> GetAll<T>() where T : Enumeration =>
            typeof(T).GetFields(BindingFlags.Public |
                                BindingFlags.Static |
                                BindingFlags.DeclaredOnly)
                        .Select(f => f.GetValue(null))
                        .Cast<T>();

        /// <summary>
        /// Determines whether the specified object is equal to the current enumeration.
        /// </summary>
        /// <param name="obj">The object to compare with the current enumeration.</param>
        /// <returns>true if the specified object is equal to the current enumeration; otherwise, false.</returns>
        public override bool Equals(object? obj)
        {
            ArgumentGuard.NotNull(obj, nameof(obj));

            if (obj is not Enumeration otherValue)
            {
                return false;
            }

            var typeMatches = GetType().Equals(obj?.GetType());
            var valueMatches = Id.Equals(otherValue.Id);

            return typeMatches && valueMatches;
        }

        /// <summary>
        /// Returns a hash code for the current enumeration.
        /// </summary>
        /// <returns>A hash code for the current enumeration.</returns>
        public override int GetHashCode() => Id.GetHashCode();

        /// <summary>
        /// Calculates the absolute difference between the identifiers of two enumerations.
        /// </summary>
        /// <param name="firstValue">The first enumeration.</param>
        /// <param name="secondValue">The second enumeration.</param>
        /// <returns>The absolute difference between the two enumeration ids.</returns>
        public static int AbsoluteDifference(Enumeration firstValue, Enumeration secondValue)
        {
            ArgumentGuard.NotNull(firstValue, nameof(firstValue));
            ArgumentGuard.NotNull(secondValue, nameof(secondValue));
            var absoluteDifference = Math.Abs(firstValue.Id - secondValue.Id);
            return absoluteDifference;
        }

        /// <summary>
        /// Retrieves an enumeration instance by its id.
        /// </summary>
        /// <typeparam name="T">The type of the enumeration.</typeparam>
        /// <param name="value">The id value to search for.</param>
        /// <returns>The enumeration instance that matches the specified id.</returns>
        /// <exception cref="InvalidOperationException">Thrown if no matching enumeration is found.</exception>
        public static T FromValue<T>(int value) where T : Enumeration
        {
            var matchingItem = Parse<T, int>(value, "value", item => item.Id == value);
            return matchingItem;
        }

        /// <summary>
        /// Retrieves an enumeration instance by its display name.
        /// </summary>
        /// <typeparam name="T">The type of the enumeration.</typeparam>
        /// <param name="displayName">The display name to search for.</param>
        /// <returns>The enumeration instance that matches the specified display name.</returns>
        /// <exception cref="InvalidOperationException">Thrown if no matching enumeration is found.</exception>
        public static T FromDisplayName<T>(string displayName) where T : Enumeration
        {
            var matchingItem = Parse<T, string>(displayName, "display name", item => item.Name == displayName);
            return matchingItem;
        }

        /// <summary>
        /// Helper method that parses an enumeration based on a provided value and predicate.
        /// </summary>
        /// <typeparam name="T">The type of the enumeration.</typeparam>
        /// <typeparam name="K">The type of the value to search for.</typeparam>
        /// <param name="value">The value to search for.</param>
        /// <param name="description">A description of the value being searched (used in the exception message).</param>
        /// <param name="predicate">A predicate that defines the matching logic for the enumeration.</param>
        /// <returns>The matching enumeration instance.</returns>
        /// <exception cref="InvalidOperationException">Thrown if no matching enumeration is found.</exception>
        private static T Parse<T, K>(K value, string description, Func<T, bool> predicate) where T : Enumeration
        {
            var matchingItem = GetAll<T>().FirstOrDefault(predicate);

            return matchingItem ?? throw new InvalidOperationException($"'{value}' is not a valid {description} in {typeof(T)}");
        }

        /// <summary>
        /// Compares the current enumeration instance with another object based on their identifiers.
        /// </summary>
        /// <param name="obj">The object to compare with.</param>
        /// <returns>An integer that indicates the relative order of the objects being compared.</returns>
        public int CompareTo(object? obj) => Id.CompareTo((obj as Enumeration)?.Id);
    }
}
