using Microsoft.Extensions.DependencyInjection;
using MidAssignment.Infrastructure.Services;
using MidAssignment.Infrastructure.Services.Impl;

namespace MidAssignment.Infrastructure
{
    public static class Extensions
    {
        public static void AddInfrastructureServices(this IServiceCollection services)
        {
            services.AddScoped<IEmailService, EmailService>();
        }
    }
}
