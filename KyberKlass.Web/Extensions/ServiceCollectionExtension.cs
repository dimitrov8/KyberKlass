namespace KyberKlass.Web.Extensions;

using Data;
using Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Services.Data;
using Services.Data.Interfaces;

public static class ServiceCollectionExtension
{
	public static IServiceCollection AddApplicationServices(this IServiceCollection services)
	{
		services.AddAuthentication()
			.AddCookie();

		services.AddAuthorization();

		services.AddScoped<ISchoolService, SchoolService>();
		services.AddScoped<IUserService, UserService>();

		return services;
	}

	public static IServiceCollection AddApplicationDbContext(this IServiceCollection services, IConfiguration config)
	{
		string? connectionString = config.GetConnectionString("DefaultConnection") ??
		                           throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

		services.AddDbContext<KyberKlassDbContext>(options =>
			options.UseSqlServer(connectionString));

		//services.AddCoreAdmin("Admin");
		//	services.AddCoreAdmin(new CoreAdminOptions()
		//	{
		//		IgnoreEntityTypes = new[] {typeof(IdentityUserRole<Guid>), typeof(IdentityRoleClaim<Guid>), typeof(Classroom)}
		//	});

		services.AddDatabaseDeveloperPageExceptionFilter();

		return services;
	}

	public static IServiceCollection AddApplicationIdentity(this IServiceCollection services, IConfiguration config)
	{
		services.AddDefaultIdentity<ApplicationUser>(options => { options.SignIn.RequireConfirmedAccount = false; })
			.AddRoles<IdentityRole<Guid>>()
			.AddEntityFrameworkStores<KyberKlassDbContext>();

		return services;
	}
}