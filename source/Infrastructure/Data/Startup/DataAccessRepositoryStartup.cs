using Data.Repositories;
using Domain.Entities.Events;
using Domain.Entities.Members;
using Domain.Entities.Posts;
using Domain.Entities.Roles;
using Domain.Entities.Tags;
using Microsoft.Extensions.DependencyInjection;

namespace Data.Startup;

/// <summary>
/// Initializes the Data Access Layer using Entity Framework
/// </summary>
public static class DataAccessRepositoryStartup
{

    public static void Register(IServiceCollection services, CachingInitializationStrategy strategy)
    {
        switch(strategy)
        {
            case CachingInitializationStrategy.Concrete: 
                RegisterRepositoriesWithCache(services); 
                break;
            case CachingInitializationStrategy.Decorator:
                RegisterRepositoriesWithCacheUsingScrutor(services);
                break;
            case CachingInitializationStrategy.None:
            default:
                RegisterRepositories(services);
                break;
        }
    }

    private static void RegisterRepositories(IServiceCollection services)
    {
        services.AddScoped<IPostRepository, PostRepository>();
        services.AddScoped<ITagRepository, TagRepository>();
        services.AddScoped<IMemberRepository, MemberRepository>();
        services.AddScoped<IRoleRepository, RoleRepository>();
        services.AddScoped<IEventRepository, EventRepository>();
    }
    private static void RegisterRepositoriesWithCache(IServiceCollection services)
    {
        services.AddScoped<IPostRepository, PostRepository>();
        services.AddScoped<ITagRepository, TagRepository>();
        services.AddScoped<MemberRepository>();
        services.AddScoped<IMemberRepository, MemberCacheRepository>();
        services.AddScoped<IRoleRepository, RoleRepository>();
        services.AddScoped<IEventRepository, EventRepository>();
    }
    private static void RegisterRepositoriesWithCacheUsingScrutor(IServiceCollection services)
    {
        //services.AddScoped<IPostRepository, PostRepository>();
        //services.AddScoped<ITagRepository, TagRepository>();
        //services.AddScoped<IMemberRepository, MemberRepository>();
        //services.Decorate<IMemberRepository, MemberCacheRepository>();
        //services.AddScoped<IRoleRepository, RoleRepository>();
        //services.AddScoped<IEventRepository, EventRepository>();
        throw new NotImplementedException("Sorry, the Scrutor package has not been included in the data layer and is therefore unavailable.");
    }
}
