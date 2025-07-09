using Microsoft.EntityFrameworkCore;
using My.QuickCampus.Data;
using My.QuickCampus.Entities;

namespace My.QuickCampus
{
    public static class ProgramMigration
    {
        public static void Migrate(this WebApplication app)
        {
            using (var scope = app.Services.CreateScope())
            {
                var _logger = scope.ServiceProvider.GetService<ILogger<Program>>();
                _logger?.LogInformation("Starting database migration...");
                try
                {
                    var context = scope.ServiceProvider.GetService<SqliteDbContext>();
                    context.Database.Migrate();

                    var anyStudent = context.Students.AnyAsync().GetAwaiter().GetResult();
                    if (!anyStudent)
                    {
                        _logger?.LogInformation("Creating migration students...");

                        var lia = new Student() { StudentId = 1, ScholarNumber = 13610, Name = "Lia", DisplayName = "Lia Leishangthem", QuickCampusId = "31090e22-1408-4cac-9faf-6fc6df06de26" };
                        context.Students.Add(lia);

                        var leo = new Student() { StudentId = 2, ScholarNumber = 14366, Name = "Leo", DisplayName = "Leo Leishangthem", QuickCampusId = "f321032d-0af6-4379-ad5a-09a880518855" };
                        context.Students.Add(leo);

                        var lenin = new Student() { StudentId = 3, ScholarNumber = 14365, Name = "Lenin", DisplayName = "Lenin Leishangthem", QuickCampusId = "c224de9e-459c-4890-a438-41e8415082ea" };
                        context.Students.Add(lenin);

                        context.SaveChanges();
                    }
                    else
                    {
                        _logger?.LogInformation("Migration students already exist...");
                    }

                    _logger?.LogInformation("Database migration successful...");
                }
                catch (Exception ex)
                {
                    _logger?.LogError(ex, "Database migration failed...");
                }
            }
        }
    }
}
