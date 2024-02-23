namespace KyberKlass.Data;

using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

public class KyberKlassDbContext : IdentityDbContext
{
	public KyberKlassDbContext(DbContextOptions<KyberKlassDbContext> options)
		: base(options)
	{
	}
}