using Domain.Enums;
using Microsoft.AspNetCore.Authorization;

namespace Infrastructure.Authentication;

/// <summary>
/// Alternativbe to .RequireAuthorization() to allow for passing enum
/// </summary>
public sealed class HasPermissionAttribute : AuthorizeAttribute
{
    public HasPermissionAttribute(Permission permission)
        : base(policy: permission.ToString()) { }
}
