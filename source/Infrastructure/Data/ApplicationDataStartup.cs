using Data.Interceptors;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data;
public static class ApplicationDataStartup
{
    public static void SetupData(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<ConvertDomainEventsToOutboxMessagesInterceptor>();
        services.AddDbContext<ApplicationDbContext>((sp, options) =>
        {
            var interceptor = sp.GetService<ConvertDomainEventsToOutboxMessagesInterceptor>();
            options.UseInMemoryDatabase("CleanArchitectureDb")
                .AddInterceptors(interceptor);
        });        
    }

    //public static async Task PopulateDatabase(this IApplicationBuilder webapp)
    //{
    //    using (IServiceScope scope = webapp.ApplicationServices.CreateScope())
    //    {
    //        ApplicationDbContext context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            
    //    }

    //}
}
