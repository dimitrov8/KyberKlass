using KyberKlass.Data;
using KyberKlass.Data.Models;
using KyberKlass.Services.Data;
using KyberKlass.Services.Data.Interfaces.Guardians;
using KyberKlass.Web.ViewModels.Admin;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Moq;
using System.Globalization;

namespace KyberKlass.Tests.Services;
public class GuardianServiceTests : IDisposable
{
    private readonly DbContextOptions<KyberKlassDbContext> _options;
    private readonly KyberKlassDbContext _dbContextMock;
    private readonly Mock<UserManager<ApplicationUser>> _userManagerMock;
    private readonly IGuardianService _sut;

    public GuardianServiceTests()
    {
        _options = new DbContextOptionsBuilder<KyberKlassDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        _dbContextMock = new KyberKlassDbContext(_options);

        // Mock UserManager<ApplicationUser>
        Mock<IUserStore<ApplicationUser>> store = new();
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
        _userManagerMock = new Mock<UserManager<ApplicationUser>>(store.Object, null, null, null, null, null, null, null, null);


        _sut = new GuardianService(_dbContextMock, _userManagerMock.Object);
    }

    [Fact]
    public async Task GetByIdAsync_ReturnsGuardianWhenValidIdProvided()
    {
        // Arrange
        Guid guardianId = Guid.NewGuid();

        ApplicationUser guardianUser = new()
        {
            Id = guardianId,
            UserName = "test@test.com",
            NormalizedUserName = "TEST@TEST.COM",
            Email = "test@test.com",
            NormalizedEmail = "TEST@TEST.COM",
            FirstName = "Brendan",
            LastName = "Osborne",
            BirthDate = DateTime.ParseExact("1990-01-01", "yyyy-MM-dd", CultureInfo.InvariantCulture),
            Address = "Test Address",
            IsActive = true
        };

        Guardian expectedGuardian = new() { Id = guardianId, ApplicationUser = guardianUser };

        await _dbContextMock.Users.AddAsync(guardianUser);
        await _dbContextMock.Guardians.AddAsync(expectedGuardian);
        await _dbContextMock.SaveChangesAsync();

        // Act
        Guardian? result = await _sut.GetByIdAsync(guardianId.ToString());

        // Assert
        Assert.NotNull(result);
        Assert.Equal(expectedGuardian.Id, result!.Id);
    }

    [Fact]
    public async Task GetByIdAsync_ReturnsNullWhenInvalidIdProvided()
    {
        // Arrange
        string invalidId = Guid.NewGuid().ToString();

        // Act
        Guardian? result = await _sut.GetByIdAsync(invalidId);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task IsGuardianAssignedToStudentAsyncReturnsTrueWhenGuardianIsAssigned()
    {
        // Arrange
        Guid guardianId = Guid.NewGuid();
        ApplicationUser guardianUser = new()
        {
            Id = guardianId,
            UserName = "test@test.com",
            NormalizedUserName = "TEST@TEST.COM",
            Email = "test@test.com",
            NormalizedEmail = "TEST@TEST.COM",
            FirstName = "Brendan",
            LastName = "Osborne",
            BirthDate = DateTime.ParseExact("1990-01-01", "yyyy-MM-dd", CultureInfo.InvariantCulture),
            Address = "Test Address",
            IsActive = true
        };

        Guid studentId = Guid.NewGuid();
        ApplicationUser studentUser = new()
        {
            Id = studentId,
            UserName = "test_student@test.com",
            NormalizedUserName = "TEST_STUDENT@TEST.COM",
            Email = "test_student@test.com",
            NormalizedEmail = "TEST_STUDENT@TEST.COM",
            FirstName = "Random",
            LastName = "Student",
            BirthDate = DateTime.ParseExact("2000-01-01", "yyyy-MM-dd", CultureInfo.InvariantCulture),
            Address = "Random Address",
            IsActive = true
        };


        Guardian guardian = new() { Id = guardianId, ApplicationUser = guardianUser };
        Student student = new() { Id = studentId, ApplicationUser = studentUser, GuardianId = guardianId, Guardian = guardian };

        await _dbContextMock.Guardians.AddAsync(guardian);
        await _dbContextMock.Students.AddAsync(student);
        await _dbContextMock.SaveChangesAsync();

        // Act
        bool result = await _sut.IsGuardianAssignedToStudentAsync(guardianUser.Id.ToString());

        // Assert
        Assert.True(result);
    }

    [Fact]
    public async Task IsGuardianAssignedToStudentAsync_ReturnsFalseWhenGuardianIsNotAssigned()
    {
        // Arrange
        Guid guardianId = Guid.NewGuid();
        ApplicationUser guardianUser = new()
        {
            Id = guardianId,
            UserName = "test@test.com",
            NormalizedUserName = "TEST@TEST.COM",
            Email = "test@test.com",
            NormalizedEmail = "TEST@TEST.COM",
            FirstName = "Brendan",
            LastName = "Osborne",
            BirthDate = DateTime.ParseExact("1990-01-01", "yyyy-MM-dd", CultureInfo.InvariantCulture),
            Address = "Test Address",
            IsActive = true
        };

        Guardian guardian = new() { Id = guardianId, ApplicationUser = guardianUser };
        await _dbContextMock.Guardians.AddAsync(guardian);
        await _dbContextMock.SaveChangesAsync();

        // Act
        bool result = await _sut.IsGuardianAssignedToStudentAsync(guardianId.ToString());

        // Assert
        Assert.False(result);
    }

    [Fact]
    public async Task GetGuardianByUserIdAsync_ReturnsGuardianWhenGuardianIsFound()
    {
        // Arrange
        Guid guardianId = Guid.NewGuid();
        ApplicationUser guardianUser = new()
        {
            Id = guardianId,
            UserName = "test@test.com",
            NormalizedUserName = "TEST@TEST.COM",
            Email = "test@test.com",
            NormalizedEmail = "TEST@TEST.COM",
            FirstName = "Brendan",
            LastName = "Osborne",
            BirthDate = DateTime.ParseExact("1990-01-01", "yyyy-MM-dd", CultureInfo.InvariantCulture),
            Address = "Test Address",
            IsActive = true
        };

        Guid studentId = Guid.NewGuid();
        ApplicationUser studentUser = new()
        {
            Id = studentId,
            UserName = "test_student@test.com",
            NormalizedUserName = "TEST_STUDENT@TEST.COM",
            Email = "test_student@test.com",
            NormalizedEmail = "TEST_STUDENT@TEST.COM",
            FirstName = "Random",
            LastName = "Student",
            BirthDate = DateTime.ParseExact("2000-01-01", "yyyy-MM-dd", CultureInfo.InvariantCulture),
            Address = "Random Address",
            IsActive = true
        };

        Guardian guardian = new()
        {
            Id = guardianId,
            ApplicationUser = guardianUser
        };

        Student student = new()
        {
            Id = Guid.NewGuid(),
            ApplicationUser = studentUser,
            GuardianId = guardianId,
            Guardian = guardian
        };

        guardian.Students = new List<Student> { student };

        await _dbContextMock.Guardians.AddAsync(guardian);
        await _dbContextMock.Students.AddAsync(student);
        await _dbContextMock.SaveChangesAsync();

        // Act
        Guardian? result = await _sut.GetGuardianAssignedByUserIdAsync(studentId.ToString());

        // Assert
        Assert.NotNull(result);
        Assert.Equal(guardian.Id, result!.Id);
    }

    [Fact]
    public async Task GetGuardianByUserIdAsync_ReturnsNull_WhenNoGuardianIsFound()
    {
        // Arrange
        string userId = Guid.NewGuid().ToString();

        // Act
        Guardian? result = await _sut.GetGuardianAssignedByUserIdAsync(userId);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task GetAllGuardiansAsync_ReturnsGuardiansWhenGuardiansExist()
    {
        // Arrange
        List<ApplicationUser> guardians = new()
        {
            new()
                { Id = Guid.NewGuid(), FirstName = "TestFirstName", LastName = "TestLastName" },
            new()
                { Id = Guid.NewGuid(), FirstName = "AnotherTestFirstName", LastName = "AnotherTestLastName" }
        };

        _userManagerMock.Setup(m => m.GetUsersInRoleAsync("Guardian"))
            .ReturnsAsync(guardians);

        // Act
        IEnumerable<BasicViewModel> result = await _sut.GetAllGuardiansAsync();

        // Assert
        Assert.NotNull(result);
        List<BasicViewModel> basicViewModels = result.ToList();
        Assert.Equal(guardians.Count, basicViewModels.Count);

        foreach (ApplicationUser guardian in guardians)
        {
            Assert.Contains(basicViewModels, g => g.Id == guardian.Id.ToString() && g.Name == guardian.GetFullName());
        }
    }

    [Fact]
    public async Task GetAllGuardiansAsync_ReturnsEmptyListWhenNoGuardiansExist()
    {
        // Arrange
        _userManagerMock.Setup(m => m.GetUsersInRoleAsync("Guardian"))
            .ReturnsAsync(new List<ApplicationUser>());

        // Act
        IEnumerable<BasicViewModel> result = await _sut.GetAllGuardiansAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Empty(result);
    }

    public async void Dispose()
    {
        await _dbContextMock.DisposeAsync();
        GC.SuppressFinalize(this);
    }
}