using KyberKlass.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Reflection;

namespace KyberKlass.Data;
public class KyberKlassDbContext : IdentityDbContext<ApplicationUser, IdentityRole<Guid>, Guid>
{
    public KyberKlassDbContext()
    {
    }

    public KyberKlassDbContext(DbContextOptions<KyberKlassDbContext> options)
        : base(options)
    {
    }

    public DbSet<School> Schools { get; set; } = null!;

    public DbSet<Classroom> Classrooms { get; set; } = null!;

    public DbSet<Teacher> Teachers { get; set; } = null!;

    public DbSet<Student> Students { get; set; } = null!;

    public DbSet<Guardian> Guardians { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        foreach (Microsoft.EntityFrameworkCore.Metadata.IMutableEntityType entityType in builder.Model.GetEntityTypes())
        {
            Microsoft.EntityFrameworkCore.Metadata.IMutableProperty? isActiveProperty = entityType.FindProperty("IsActive");

            if (isActiveProperty != null && isActiveProperty.ClrType == typeof(bool))
            {
                ParameterExpression parameter = Expression.Parameter(entityType.ClrType, "e");
                BinaryExpression body = Expression.Equal(
                    Expression.Property(parameter, "IsActive"),
                    Expression.Constant(true));

                builder.Entity(entityType.ClrType)
                    .HasQueryFilter(Expression.Lambda(body, parameter));
            }
        }

        base.OnModelCreating(builder);
    }
}