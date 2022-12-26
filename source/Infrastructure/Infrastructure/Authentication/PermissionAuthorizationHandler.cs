using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.JsonWebTokens;
using Polly;
using System.Security.Claims;

namespace Infrastructure.Authentication;
public class PermissionAuthorizationHandler : AuthorizationHandler<PermissionRequirement>
{
    private readonly IServiceScopeFactory mServiceScopeFactory;

    public PermissionAuthorizationHandler(IServiceScopeFactory serviceScopeFactory)
    {
        this.mServiceScopeFactory = serviceScopeFactory;
    }

    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirement)
    {
        //var permissions = await GetPermissionsFromService(context);
        var permissions = GetPermissionsFromClaims(context);

        if (permissions.Contains(requirement.Permission))
            context.Succeed(requirement);
        return Task.CompletedTask;
    }
    private async Task<HashSet<string>> GetPermissionsFromService(AuthorizationHandlerContext context)
    {
        string? memberId = context.User.Claims.FirstOrDefault(
            x => x.Type == ClaimTypes.NameIdentifier)?.Value;

        if (!Guid.TryParse(memberId, out Guid parsedMemberId))
            return default!;

        // this is the slow way of pulling through the service and hitting the database
        using IServiceScope scope = mServiceScopeFactory.CreateScope();

        IPermissionService permissionService = scope.ServiceProvider.GetRequiredService<IPermissionService>();

        // This is the slow way that calls the database
        return await permissionService.GetPermissionsAsync(parsedMemberId);
    }
    private HashSet<string> GetPermissionsFromClaims(AuthorizationHandlerContext context)
    {
        return context.User.Claims.Where(x => x.Type == CustomClaims.Permissions)
            .Select(x => x.Value)
            .ToHashSet();
    }
}
