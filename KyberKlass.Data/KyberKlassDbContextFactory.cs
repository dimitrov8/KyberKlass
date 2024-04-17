namespace KyberKlass.Data;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

public class KyberKlassDbContextFactory : IDesignTimeDbContextFactory<KyberKlassDbContext>
{
    public KyberKlassDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<KyberKlassDbContext>();
        optionsBuilder.UseSqlServer("Server=.\\SQLEXPRESS;Database=KyberKlass;Trusted_Connection=True;MultipleActiveResultSets=true");

        return new KyberKlassDbContext(optionsBuilder.Options);
    }
}