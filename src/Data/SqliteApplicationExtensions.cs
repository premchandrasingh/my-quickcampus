using Microsoft.EntityFrameworkCore;

namespace My.QuickCampus.Data
{
    public static class SqliteApplicationExtensions
    {
        public static IServiceCollection AddSqliteDatabase(this IServiceCollection services, IConfiguration configuration)
        {
            string connString = configuration.GetConnectionString("db_conn");
            services.AddDbContext<AppDbContext>(options =>
            {
                options.UseSqlite(connString);
                options.EnableSensitiveDataLogging(); // Enable this only for development purposes
            });
            

            return services;
        }
    }
}
