using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace KyberKlass.Data;

public class KyberKlassDbContextFactory : IDesignTimeDbContextFactory<KyberKlassDbContext>
{
    public KyberKlassDbContext CreateDbContext(string[] args)
    {
        DbContextOptionsBuilder<KyberKlassDbContext> optionsBuilder = new();
        optionsBuilder.UseSqlServer("Server=localhost;Database=KyberKlass;User Id=sa;Password=KyberKlassS!;TrustServerCertificate=True");

        return new KyberKlassDbContext(optionsBuilder.Options);
    }
}