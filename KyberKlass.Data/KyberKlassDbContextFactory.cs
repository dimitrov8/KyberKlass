using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace KyberKlass.Data;

public class KyberKlassDbContextFactory : IDesignTimeDbContextFactory<KyberKlassDbContext>
{
    public KyberKlassDbContext CreateDbContext(string[] args)
    {
        DbContextOptionsBuilder<KyberKlassDbContext> optionsBuilder = new();
        optionsBuilder.UseSqlServer("Server=localhost;Database=KyberKlass;User Id=sa;Password=KyberKlassS!;TrustServerCertificate=True")
            .ConfigureWarnings(w => w.Ignore(RelationalEventId.PendingModelChangesWarning));

        return new KyberKlassDbContext(optionsBuilder.Options);
    }
}