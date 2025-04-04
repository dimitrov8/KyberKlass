namespace KyberKlass.Data.Configurations;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Models;

public class SchoolEntityConfiguration : IEntityTypeConfiguration<School>
{
    public void Configure(EntityTypeBuilder<School> builder)
    {
        builder
            .HasQueryFilter(c => c.IsActive);

        this.SeedSchools(builder);
    }

    private void SeedSchools(EntityTypeBuilder<School> builder)
    {
        builder.HasData(
            new School
            {
                Id = Guid.NewGuid(),
                Name = "St. George International School",
                Address = "Promishlena zona Hladilnika, bul. \"Nikola Y. Vaptsarov\" 47, 1407 Sofia",
                Email = "st@example.com",
                PhoneNumber = "02 414 4414"
            },
            new School
            {
                Id = Guid.NewGuid(),
                Name = "91. Немска езикова гимназия „Проф. Константин Гълъбов“",
                Address = "Sofia Center, Pozitano St 26, 1000 Sofia",
                Email = "schoolb@ez.com",
                PhoneNumber = "02 987 5305"
            },
                new School
                {
                    Id = Guid.NewGuid(),
                    Name = "Test School",
                    Address = "Rnd. Address",
                    Email = "test_schoolc@ez.com",
                    PhoneNumber = "02 987 0000"
                }
        );
    }
}