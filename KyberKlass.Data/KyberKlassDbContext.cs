namespace KyberKlass.Data;

using System.Linq.Expressions;
using System.Reflection;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Models;

public class KyberKlassDbContext : IdentityDbContext<ApplicationUser, IdentityRole<Guid>, Guid>
{
	public KyberKlassDbContext(DbContextOptions<KyberKlassDbContext> options)
		: base(options)
	{
	}

	public DbSet<School> Schools { get; set; } = null!;

	public DbSet<Classroom> Classrooms { get; set; } = null!;

	public DbSet<Teacher> Teachers { get; set; } = null!;

	public DbSet<Subject> Subjects { get; set; } = null!;

	public DbSet<Student> Students { get; set; } = null!;

	public DbSet<Guardian> Guardians { get; set; } = null!;

	protected override void OnModelCreating(ModelBuilder builder)
	{
		builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

		foreach (var entityType in builder.Model.GetEntityTypes())
		{
			var isActiveProperty = entityType.FindProperty("IsActive");

			if (isActiveProperty != null && isActiveProperty.ClrType == typeof(bool))
			{
				var parameter = Expression.Parameter(entityType.ClrType, "e");
				var body = Expression.Equal(
					Expression.Property(parameter, "IsActive"),
					Expression.Constant(true));

				builder.Entity(entityType.ClrType)
					.HasQueryFilter(Expression.Lambda(body, parameter));
			}
		}

		base.OnModelCreating(builder);
	}
}