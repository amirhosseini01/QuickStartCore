using Microsoft.EntityFrameworkCore;
using Server.Core.Data;

namespace Server.Core.Commons;

public static class Middlewares
{
    public static async Task MigrateDatabase(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<BaseDbContext>();
        await db.Database.MigrateAsync();
        await db.Database.EnsureCreatedAsync();
    }
}