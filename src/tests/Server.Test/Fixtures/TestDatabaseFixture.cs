using Microsoft.EntityFrameworkCore;
using Server.Core.Data;

namespace Server.Test.Fixtures;

public class TestDatabaseFixture
{
    private static readonly object Lock = new();
    private static bool _databaseInitialized;

    public TestDatabaseFixture()
    {
        lock (Lock)
        {
            if (_databaseInitialized) return;
            
            using (var context = CreateContext())
            {
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();

                    
                context.SaveChanges();
            }

            _databaseInitialized = true;
        }
    }

    public static BaseDbContext CreateContext()
    {
        const string connectionString = @"Server=.;Database=QuickStart;Trusted_Connection=True;TrustServerCertificate=True;ConnectRetryCount=0";
        return new BaseDbContext(
            new DbContextOptionsBuilder<BaseDbContext>()
                .UseSqlServer(connectionString)
                .Options);
    }
}