namespace KyberKlass.Data;

using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;
using System.Reflection;
using KyberKlass.Data.Configurations;
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

	public DbSet<Absence> Absences { get; set; } = null!;

	protected override void OnModelCreating(ModelBuilder builder)
	{
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

		foreach (var entityType in builder.Model.GetEntityTypes())
		{
			var isDeletedProperty = entityType.FindProperty("IsDeleted");

			if (isDeletedProperty != null && isDeletedProperty.ClrType == typeof(bool))
			{
				var parameter = Expression.Parameter(entityType.ClrType, "e");
				var body = Expression.Equal(
					Expression.Property(parameter, "IsDeleted"),
					Expression.Constant(false));

				builder.Entity(entityType.ClrType)
					.HasQueryFilter(Expression.Lambda(body, parameter));
			}
		}

		base.OnModelCreating(builder);
	}
}