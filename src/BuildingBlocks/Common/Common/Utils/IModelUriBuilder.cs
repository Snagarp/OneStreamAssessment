namespace Common.Utils
{
    /// <summary>
    /// Interface for building a URI for HTTP requests based on the client, path, and model.
    /// </summary>
    public interface IModelUriBuilder
    {
        /// <summary>
        /// Builds a URI for the specified HTTP client and path based on the provided model.
        /// </summary>
        /// <param name="client">The HTTP client used to generate the base URI.</param>
        /// <param name="path">The relative path or endpoint for the API call.</param>
        /// <param name="model">The model object whose properties may be included as query parameters in the URI.</param>
        /// <param name="suppressQueryParameters">If set to <c>true</c>, query parameters will be suppressed in the URI.</param>
        /// <returns>A <see cref="Uri"/> object representing the full request URI.</returns>
        Uri Build(HttpClient client, string path, object model, bool suppressQueryParameters = false);
    }
}
