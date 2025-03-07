using Microsoft.EntityFrameworkCore;
using PetFamily.Infrastructure;

namespace PetFamily.API;

public static class AppExtensions
{
    public static async Task ApplyMigration(this WebApplication app)
    {
        await using var scope = app.Services.CreateAsyncScope();
        var dbContex = scope.ServiceProvider.GetRequiredService<ApplicationDbContex>();

        await dbContex.Database.MigrateAsync();
    }
}