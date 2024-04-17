namespace KyberKlass.Tests.Services;

using System.Globalization;
using Data;
using Data.Models;
using KyberKlass.Services.Data;
using KyberKlass.Services.Data.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Moq;
using Web.ViewModels.Admin;

public class GuardianServiceTests : IDisposable
{
    private readonly DbContextOptions<KyberKlassDbContext> _options;
    private readonly KyberKlassDbContext _dbContextMock;
    private readonly Mock<UserManager<ApplicationUser>> _userManagerMock;
    private readonly IGuardianService _sut;

    public GuardianServiceTests()
    {
        this._options = new DbContextOptionsBuilder<KyberKlassDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        this._dbContextMock = new KyberKlassDbContext(this._options);

        // Mock UserManager<ApplicationUser>
        var store = new Mock<IUserStore<ApplicationUser>>();
        this._userManagerMock = new Mock<UserManager<ApplicationUser>>(store.Object, null, null, null, null, null, null, null, null);


        this._sut = new GuardianService(this._dbContextMock, this._userManagerMock.Object);
    }

    [Fact]
    public async Task GetByIdAsync_ReturnsGuardianWhenValidIdProvided()
    {
        // Arrange
        var guardianId = Guid.NewGuid();

        var guardianUser = new ApplicationUser
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

        var expectedGuardian = new Guardian { Id = guardianId, ApplicationUser = guardianUser };

        await this._dbContextMock.Users.AddAsync(guardianUser);
        await this._dbContextMock.Guardians.AddAsync(expectedGuardian);
        await this._dbContextMock.SaveChangesAsync();

        // Act
        var result = await this._sut.GetByIdAsync(guardianId.ToString());

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
        var result = await this._sut.GetByIdAsync(invalidId);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task IsGuardianAssignedToStudentAsyncReturnsTrueWhenGuardianIsAssigned()
    {
        // Arrange
        var guardianId = Guid.NewGuid();
        var guardianUser = new ApplicationUser
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

        var studentId = Guid.NewGuid();
        var studentUser = new ApplicationUser
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


        var guardian = new Guardian { Id = guardianId, ApplicationUser = guardianUser };
        var student = new Student { Id = studentId, ApplicationUser = studentUser, GuardianId = guardianId, Guardian = guardian };

        await this._dbContextMock.Guardians.AddAsync(guardian);
        await this._dbContextMock.Students.AddAsync(student);
        await this._dbContextMock.SaveChangesAsync();

        // Act
        bool result = await this._sut.IsGuardianAssignedToStudentAsync(guardianUser.Id.ToString());

        // Assert
        Assert.True(result);
    }

    [Fact]
    public async Task IsGuardianAssignedToStudentAsync_ReturnsFalseWhenGuardianIsNotAssigned()
    {
        // Arrange
        var guardianId = Guid.NewGuid();
        var guardianUser = new ApplicationUser
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

        var guardian = new Guardian { Id = guardianId, ApplicationUser = guardianUser };
        await this._dbContextMock.Guardians.AddAsync(guardian);
        await this._dbContextMock.SaveChangesAsync();

        // Act
        bool result = await this._sut.IsGuardianAssignedToStudentAsync(guardianId.ToString());

        // Assert
        Assert.False(result);
    }

    [Fact]
    public async Task GetGuardianByUserIdAsync_ReturnsGuardianWhenGuardianIsFound()
    {
        // Arrange
        var guardianId = Guid.NewGuid();
        var guardianUser = new ApplicationUser
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

        var studentId = Guid.NewGuid();
        var studentUser = new ApplicationUser
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

        var guardian = new Guardian
        {
            Id = guardianId,
            ApplicationUser = guardianUser
        };

        var student = new Student
        {
            Id = Guid.NewGuid(),
            ApplicationUser = studentUser,
            GuardianId = guardianId,
            Guardian = guardian
        };

        guardian.Students = new List<Student> { student };

        await this._dbContextMock.Guardians.AddAsync(guardian);
        await this._dbContextMock.Students.AddAsync(student);
        await this._dbContextMock.SaveChangesAsync();

        // Act
        var result = await this._sut.GetGuardianAssignedByUserIdAsync(studentId.ToString());

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
        var result = await this._sut.GetGuardianAssignedByUserIdAsync(userId);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task GetAllGuardiansAsync_ReturnsGuardiansWhenGuardiansExist()
    {
        // Arrange
        var guardians = new List<ApplicationUser>
        {
            new()
                { Id = Guid.NewGuid(), FirstName = "TestFirstName", LastName = "TestLastName" },
            new()
                { Id = Guid.NewGuid(), FirstName = "AnotherTestFirstName", LastName = "AnotherTestLastName" }
        };

        this._userManagerMock.Setup(m => m.GetUsersInRoleAsync("Guardian"))
            .ReturnsAsync(guardians);

        // Act
        IEnumerable<BasicViewModel> result = await this._sut.GetAllGuardiansAsync();

        // Assert
        Assert.NotNull(result);
        List<BasicViewModel> basicViewModels = result.ToList();
        Assert.Equal(guardians.Count, basicViewModels.Count);

        foreach (var guardian in guardians)
        {
            Assert.Contains(basicViewModels, g => g.Id == guardian.Id.ToString() && g.Name == guardian.GetFullName());
        }
    }

    [Fact]
    public async Task GetAllGuardiansAsync_ReturnsEmptyListWhenNoGuardiansExist()
    {
        // Arrange
        this._userManagerMock.Setup(m => m.GetUsersInRoleAsync("Guardian"))
            .ReturnsAsync(new List<ApplicationUser>());

        // Act
        IEnumerable<BasicViewModel> result = await this._sut.GetAllGuardiansAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Empty(result);
    }

    public async void Dispose()
    {
        await this._dbContextMock.DisposeAsync();
    }
}