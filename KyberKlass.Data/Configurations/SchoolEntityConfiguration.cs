#region

using KyberKlass.Data.Models;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using static KyberKlass.Data.Configurations.Constants.SeedDataConstants.School;

#endregion

namespace KyberKlass.Data.Configurations;

public class SchoolEntityConfiguration : IEntityTypeConfiguration<School>
{
    public void Configure(EntityTypeBuilder<School> builder)
    {
        builder
            .HasQueryFilter(filter: static c => c.IsActive);

        SeedSchools(builder);
    }

    private void SeedSchools(EntityTypeBuilder<School> builder)
    {
        builder.HasData(
        new School
        {
            Id = School1Id,
            Name = "St. George International School",
            Address = "Promishlena zona Hladilnika, bul. \"Nikola Y. Vaptsarov\" 47, 1407 Sofia",
            Email = "st@example.com",
            PhoneNumber = "02 414 4414",
            IsActive = true
        },
        new School
        {
            Id = School2Id,
            Name = "91. Немска езикова гимназия „Проф. Константин Гълъбов“",
            Address = "Sofia Center, Pozitano St 26, 1000 Sofia",
            Email = "schoolb@ez.com",
            PhoneNumber = "02 987 5305",
            IsActive = true
        },
        new School
        {
            Id = School3Id,
            Name = "Test School",
            Address = "Rnd. Address",
            Email = "test_schoolc@ez.com",
            PhoneNumber = "02 987 0000",
            IsActive = true
        }
        );
    }
}