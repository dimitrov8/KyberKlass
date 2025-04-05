using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace KyberKlass.Data;
public class KyberKlassDbContextFactory : IDesignTimeDbContextFactory<KyberKlassDbContext>
{
    public KyberKlassDbContext CreateDbContext(string[] args)
    {
        DbContextOptionsBuilder<KyberKlassDbContext> optionsBuilder = new();
        optionsBuilder.UseSqlServer("Server=.\\SQLEXPRESS;Database=KyberKlass;Trusted_Connection=True;MultipleActiveResultSets=true");

        return new KyberKlassDbContext(optionsBuilder.Options);
    }
}