namespace KyberKlass.Web.Extensions;

using KyberKlass.Data;
using KyberKlass.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

public static class ServiceCollectionExtension
{
	public static IServiceCollection AddApplicationServices(this IServiceCollection services)
	{
		return services;
	}

	public static IServiceCollection AddApplicationDbContext(this IServiceCollection services, IConfiguration config)
	{
		string? connectionString = config.GetConnectionString("DefaultConnection") ??
		                           throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

		services.AddDbContext<KyberKlassDbContext>(options =>
			options.UseSqlServer(connectionString));

		services.AddDatabaseDeveloperPageExceptionFilter();

		return services;
	}

	public static IServiceCollection AddApplicationIdentity(this IServiceCollection services, IConfiguration config)
	{
		services.AddDefaultIdentity<ApplicationUser>(options => { options.SignIn.RequireConfirmedAccount = false; })
			.AddEntityFrameworkStores<KyberKlassDbContext>();

		return services;
	}
}