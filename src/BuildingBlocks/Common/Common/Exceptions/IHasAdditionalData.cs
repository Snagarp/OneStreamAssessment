namespace Common.Exceptions
{
    /// <summary>
    /// Defines a contract for exceptions or other classes that can provide additional data related to the exception or event.
    /// </summary>
    public interface IHasAdditionalData
    {
        /// <summary>
        /// Retrieves a list of additional data related to the exception or event.
        /// </summary>
        /// <returns>A <see cref="List{T}"/> of strings containing additional data.</returns>
        List<string> GetAdditionalData();
    }
}
