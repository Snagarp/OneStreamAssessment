namespace Vehicle.Application.DTOs.Request.Account
{
    /// <summary>
    /// Represents a request to change the role of a user.
    /// </summary>
    public record ChangeUserRoleRequestDTO(string UserEmail, string RoleName);
}