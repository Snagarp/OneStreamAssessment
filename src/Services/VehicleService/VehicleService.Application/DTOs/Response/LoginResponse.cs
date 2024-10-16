namespace Vehicle.Application.DTOs.Response
{
    /// <summary>
    /// Represents the response after a login attempt.
    /// </summary>
    /// <param name="Flag">Indicates whether the login was successful.</param>
    /// <param name="Message">A message describing the result of the login attempt.</param>
    /// <param name="Token">The JWT token if the login was successful; otherwise, null.</param>
    /// <param name="RefreshToken">The refresh token if the login was successful; otherwise, null.</param>
    public record LoginResponse
        (bool Flag = false, string Message = null!, string Token = null!, string RefreshToken = null!);
}