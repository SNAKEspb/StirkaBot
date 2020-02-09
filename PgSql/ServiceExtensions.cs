using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace PgSql
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddPostgreSql(this IServiceCollection services, IConfiguration configuration) {
            services.Configure<ConnectionOptions>(configuration.GetSection("ConnectionOptions"));
            services.AddSingleton<PgSqlAdapter>();
            return services;
        }
        public static IServiceCollection AddPostgreSql(this IServiceCollection services, string connectionString)
        {
            services.AddSingleton((p) => new PgSqlAdapter(connectionString));
            return services;
        }
    }
}
