using ClinicManagementSystem.backend.Common.Data.Seed;

namespace ClinicManagementSystem.backend.Common.Extensions
{
    public static class SeederExtensions
    {
        /// <summary>
        /// Extension methods for seeding the database.
        /// </summary> 
        public static async Task SeedDatabaseAsync(this WebApplication app)
        {
            /// <summary>
            /// Applies migrations and seeds the database.
            /// </summary>
            await SeedData.InitializeAsync(app);
        }
    }
}
