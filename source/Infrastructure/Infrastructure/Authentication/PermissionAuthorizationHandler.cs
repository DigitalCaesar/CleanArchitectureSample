using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.JsonWebTokens;
using System.Security.Claims;

namespace Infrastructure.Authentication;
public class PermissionAuthorizationHandler : AuthorizationHandler<PermissionRequirement>
{
    private readonly IServiceScopeFactory mServiceScopeFactory;

    public PermissionAuthorizationHandler(IServiceScopeFactory serviceScopeFactory)
    {
        this.mServiceScopeFactory = serviceScopeFactory;
    }

    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirement)
    {
        string? memberId = context.User.Claims.FirstOrDefault(
            x => x.Type == ClaimTypes.NameIdentifier)?.Value;

        if (!Guid.TryParse(memberId, out Guid parsedMemberId))
            return;

        using IServiceScope scope = mServiceScopeFactory.CreateScope();

        IPermissionService permissionService = scope.ServiceProvider.GetRequiredService<IPermissionService>();

        // This is the slow way that calls the database
        HashSet<string> permissions = await permissionService.GetPermissionsAsync(parsedMemberId);

        if (permissions.Contains(requirement.Permission))
            context.Succeed(requirement);
    }
}
