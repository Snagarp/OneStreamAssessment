namespace Identity.Security.Abstract
{
    /// <summary>
    ///     Creates security -related cookies on current HTTP response
    /// </summary>
    public interface ICookieBaker
    {
        /// <summary>Adds JWT cookies to current HTTP response.</summary>
        /// <param name="jwtToken">The JWT token.</param>
        void AddJwtCookie(string jwtToken);

        /// <summary>Removes JWT cookies from current HTTP response.</summary>
        void RemoveJwtCookie();

        /// <summary>Adds the anti-forgery cookie to current HTTP response.</summary>
        void AddAntiForgeryCookie();
    }
}
