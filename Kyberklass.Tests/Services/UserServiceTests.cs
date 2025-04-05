using KyberKlass.Data;
using KyberKlass.Data.Models;
using KyberKlass.Services.Data.Interfaces.Guardians;
using KyberKlass.Services.Data.User;
using KyberKlass.Web.ViewModels.Admin.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Moq;
using System.Globalization;

namespace KyberKlass.Tests.Services;
public class UserServiceTests : IDisposable
{
    private readonly DbContextOptions<KyberKlassDbContext> _options;
    private readonly KyberKlassDbContext _dbContextMock;
    private readonly Mock<UserManager<ApplicationUser>> _userManagerMock;
    private readonly Mock<IGuardianService> _guardianServiceMock;
    private readonly UserService _sut;

    public UserServiceTests()
    {
        _options = new DbContextOptionsBuilder<KyberKlassDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        _dbContextMock = new KyberKlassDbContext(_options);

#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
        _userManagerMock = new Mock<UserManager<ApplicationUser>>(
            Mock.Of<IUserStore<ApplicationUser>>(),
            null,
            null,
            null,
            null,
            null,
            null,
            null,
            null
        );

        _guardianServiceMock = new Mock<IGuardianService>();

        _sut = new UserService(_dbContextMock, _userManagerMock.Object, _guardianServiceMock.Object);
    }

    [Fact]
    public async Task GetUserById_ReturnsUser()
    {
        // Arrange
        Guid userId = Guid.NewGuid();
        ApplicationUser user = new()
        {
            Id = userId,
            UserName = "test@test.com",
            NormalizedUserName = "TES@TEST.COM",
            Email = "test@test.com",
            NormalizedEmail = "test@TEST.COM",
            FirstName = "Test",
            LastName = "Test",
            BirthDate = DateTime.ParseExact("2000-01-01", "yyyy-MM-dd", CultureInfo.InvariantCulture),
            Address = "Test Address",
            IsActive = true
        };

        await _dbContextMock.Users.AddAsync(user);
        await _dbContextMock.SaveChangesAsync();

        // Act
        ApplicationUser? result = await _sut.GetUserById(user.Id.ToString());

        Assert.NotNull(result);
        Assert.Equal(userId, result!.Id);
    }

    [Fact]
    public async Task GetUserById_ReturnsNullIfUserDoesNotExist()
    {
        // Arrange
        Guid invalidUserId = Guid.NewGuid();

        // Act
        ApplicationUser? result = await _sut.GetUserById(invalidUserId.ToString());

        Assert.Null(result);
    }

    [Fact]
    public async Task GetDetailsAsync_ReturnsUserWithNoRoleDetailsViewModel()
    {
        // Arrange
        Guid userId = Guid.NewGuid();
        ApplicationUser user = new()
        {
            Id = userId,
            UserName = "test@test.com",
            NormalizedUserName = "TEST@TEST.COM",
            FirstName = "Brendan",
            LastName = "Osborne",
            BirthDate = new DateTime(1990,
                5,
                15),
            Address = "Random Address",
            IsActive = true,
            PhoneNumber = "0888888888",
            AccessFailedCount = 0,
            Email = "test@test.com",
            NormalizedEmail = "TEST@TEST.COM",
            ConcurrencyStamp = Guid.NewGuid().ToString()
        };

        string expectedRole = "No Role Assigned";
        string expectedFullName = "Brendan Osborne";
        string expectedBirthDate = "1990-05-15";

        UserDetailsViewModel viewModel = new()
        {
            Id = userId.ToString(),
            FullName = user.GetFullName(),
            BirthDate = user.GetBirthDate(),
            Address = user.Address,
            PhoneNumber = user.PhoneNumber,
            Email = user.Email,
            Role = "No Role Assigned",
            IsActive = "true",
            Guardian = null,
            Students = null,
            School = null
        };

        await _dbContextMock.AddAsync(user);
        await _dbContextMock.SaveChangesAsync();

        _userManagerMock.Setup(m => m.GetRolesAsync(user)).ReturnsAsync(new List<string>());

        // Act
        UserDetailsViewModel? result = await _sut.GetDetailsAsync(userId.ToString());

        // Assert
        Assert.NotNull(result);
        Assert.Equal(user.Id, Guid.Parse(viewModel.Id));
        Assert.Equal(expectedFullName, viewModel.FullName);
        Assert.Equal(expectedBirthDate, viewModel.BirthDate);
        Assert.Equal(user.Address, viewModel.Address);
        Assert.Equal(user.PhoneNumber, viewModel.PhoneNumber);
        Assert.Equal(user.Email, viewModel.Email);
        Assert.Equal(expectedRole, viewModel.Role);
        Assert.Equal("true", viewModel.IsActive);
    }

    [Fact]
    public async Task GetDetailsAsync_ReturnsNullViewModelIfUserNotFound()
    {
        // Arrange
        string invalidUserId = Guid.NewGuid().ToString();

        // Act
        UserDetailsViewModel? result = await _sut.GetDetailsAsync(invalidUserId);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task GetDetailsAsync_RoleIsSetRight()
    {
        // Arrange 
        ApplicationUser studentUser = new()
        {
            Id = Guid.NewGuid(),
            FirstName = "Brendan",
            LastName = "Osborne",
            BirthDate = new DateTime(1990,
                5,
                15),
            Address = "Random Address",
            IsActive = true,
            PhoneNumber = "0888888888",
            Email = "test@test.com",
            NormalizedEmail = "TEST@TEST.COM"
        };

        ApplicationUser guardianUser = new()
        {
            Id = Guid.NewGuid(),
            FirstName = "Brendan",
            LastName = "Osborne",
            BirthDate = new DateTime(1990,
                5,
                15),
            Address = "Random Address",
            IsActive = true,
            PhoneNumber = "0888888888",
            Email = "test@test.com",
            NormalizedEmail = "TEST@TEST.COM"
        };

        Guardian guardian = new()
        {
            Id = guardianUser.Id,
            ApplicationUser = guardianUser
        };

        Student student = new()
        {
            Id = studentUser.Id,
            ApplicationUser = studentUser,
            GuardianId = guardian.Id,
            Guardian = guardian
        };

        guardian.Students = new List<Student> { student };

        await _dbContextMock.Users.AddAsync(studentUser);
        await _dbContextMock.SaveChangesAsync();

        _userManagerMock.Setup(m => m.GetRolesAsync(studentUser))
            .ReturnsAsync(new List<string> { "Student" });

        // Act
        UserDetailsViewModel? result = await _sut.GetDetailsAsync(studentUser.Id.ToString());

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Student", result!.Role);
    }

    [Fact]
    public async Task GetForEditAsync_ReturnsUserEditFormModelWhenUserExists()
    {
        // Arrange
        Guid userId = Guid.NewGuid();
        string expectedBirthDate = "1990-05-15";
        ApplicationUser user = new()
        {
            Id = userId,
            FirstName = "Brendan",
            LastName = "Osborne",
            BirthDate = new DateTime(1990,
                5,
                15),
            Address = "Random Address",
            PhoneNumber = "0888888888",
            Email = "test@test.com",
            IsActive = true
        };

        await _dbContextMock.Users.AddAsync(user);
        await _dbContextMock.SaveChangesAsync();

        // Act
        UserEditFormModel? result = await _sut.GetForEditAsync(userId.ToString());

        // Assert
        Assert.NotNull(result);
        Assert.Equal(user.Id.ToString(), result!.Id);
        Assert.Equal(user.FirstName, result.FirstName);
        Assert.Equal(user.LastName, result.LastName);
        Assert.Equal(expectedBirthDate, result.BirthDate);
        Assert.Equal(user.Address, result.Address);
        Assert.Equal(user.PhoneNumber, result.PhoneNumber);
        Assert.Equal(user.Email, result.Email);
        Assert.Equal(user.IsActive, result.IsActive);
    }

    [Fact]
    public async Task GetForEditAsync_ReturnsNullWhenUserDoesNotExist()
    {
        // Arrange
        Guid invalidUserId = Guid.NewGuid();

        // Act
        UserEditFormModel? result = await _sut.GetForEditAsync(invalidUserId.ToString());

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task EditAsync_ReturnsUserEditFormModelWhenUserExists()
    {
        // Arrange
        Guid userId = Guid.NewGuid();
        string expectedBirthDate = "1990-05-15";
        UserEditFormModel model = new()
        {
            Id = userId.ToString(),
            FirstName = "Brendan",
            LastName = "Osborne",
            BirthDate = expectedBirthDate,
            Address = "Random Address",
            PhoneNumber = "0888888888",
            Email = "test@test.com",
            IsActive = true
        };

        ApplicationUser user = new()
        {
            Id = userId,
            FirstName = "Brendan",
            LastName = "Osborne",
            BirthDate = new DateTime(1990,
                5,
                15),
            Address = "Random Address",
            PhoneNumber = "0888888888",
            Email = "test@test.com",
            IsActive = true
        };

        await _dbContextMock.AddAsync(user);
        await _dbContextMock.SaveChangesAsync();

        // Act
        UserEditFormModel? result = await _sut.EditAsync(userId.ToString(), model);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(user.Id.ToString(), result!.Id);
        Assert.Equal(user.FirstName, result.FirstName);
        Assert.Equal(user.LastName, result.LastName);
        Assert.Equal(expectedBirthDate, result.BirthDate);
        Assert.Equal(user.Address, result.Address);
        Assert.Equal(user.PhoneNumber, result.PhoneNumber);
        Assert.Equal(user.Email, result.Email);
        Assert.Equal(user.IsActive, result.IsActive);
    }

    [Fact]
    public async Task EditAsync_ReturnsNullWhenUserDoesNotExist()
    {
        // Arrange
        Guid invalidUserId = Guid.NewGuid();
        UserEditFormModel model = new();

        // Act
        UserEditFormModel? result = await _sut.EditAsync(invalidUserId.ToString(), model);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task EditAsync_UpdatesAllProperties()
    {
        // Arrange
        Guid userId = Guid.NewGuid();
        UserEditFormModel model = new()
        {
            FirstName = "NewFirstName",
            LastName = "NewLastName",
            BirthDate = "2000-01-01",
            Address = "NewAddress",
            PhoneNumber = "NewPhoneNumber",
            Email = "newemail@example.com",
            IsActive = true
        };

        ApplicationUser user = new()
        {
            Id = userId,
            FirstName = "InitialFirstName",
            LastName = "InitialLastName",
            BirthDate = DateTime.ParseExact("1990-01-01", "yyyy-MM-dd", CultureInfo.InvariantCulture),
            Address = "InitialAddress",
            PhoneNumber = "InitialPhoneNumber",
            Email = "initial@example.com",
            IsActive = true
        };

        await _dbContextMock.AddAsync(user);
        await _dbContextMock.SaveChangesAsync();

        // Act
        UserEditFormModel? result = await _sut.EditAsync(userId.ToString(), model);

        // Assert
        Assert.NotNull(result);

        Assert.Equal(model.FirstName, user.FirstName);
        Assert.Equal(model.LastName, user.LastName);
        Assert.Equal(DateTime.ParseExact(model.BirthDate, "yyyy-MM-dd", CultureInfo.InvariantCulture), user.BirthDate);
        Assert.Equal(model.Address, user.Address);
        Assert.Equal(model.PhoneNumber, user.PhoneNumber);
        Assert.Equal(model.Email, user.Email);
        Assert.Equal(model.IsActive, user.IsActive);
    }

    [Fact]
    public async Task AllAsync_ReturnsEmptyListWhenNoUsersExist()
    {
        // Act
        IEnumerable<UserViewModel> result = await _sut.AllAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Empty(result);
    }

    [Fact]
    public async Task AllAsync_ReturnsAllUsersWithRoles()
    {
        // Arrange
        Guid user1Id = Guid.NewGuid();
        Guid user2Id = Guid.NewGuid();
        Guid adminRoleId = Guid.NewGuid();

        List<ApplicationUser> users = new()
        {
            new()
            {
                Id = user1Id,
                UserName = "test@test.com",
                NormalizedUserName = "TEST@TEST.COM",
                Email = "test@test.com",
                NormalizedEmail = "test@TEST.COM",
                FirstName = "Random",
                LastName = "User",
                BirthDate = DateTime.ParseExact("2000-01-01", "yyyy-MM-dd", CultureInfo.InvariantCulture),
                Address = "Random Address",
                IsActive = true
            },
            new()
            {
                Id = user2Id,
                UserName = "test2@test.com",
                NormalizedUserName = "TEST2@TEST.COM",
                Email = "test2@test.com",
                NormalizedEmail = "test2@TEST.COM",
                FirstName = "AnotherRandom",
                LastName = "User",
                BirthDate = DateTime.ParseExact("1999-01-01", "yyyy-MM-dd", CultureInfo.InvariantCulture),
                Address = "Random Address",
                IsActive = true
            }
        };

        IdentityRole<Guid> role = new()
        {
            Id = adminRoleId,
            Name = "Admin"
        };

        IdentityUserRole<Guid> userRole = new()
        {
            UserId = user1Id,
            RoleId = adminRoleId
        };

        await _dbContextMock.Users.AddRangeAsync(users);
        await _dbContextMock.Roles.AddAsync(role);
        await _dbContextMock.UserRoles.AddAsync(userRole);
        await _dbContextMock.SaveChangesAsync();

        // Act
        IEnumerable<UserViewModel> result = await _sut.AllAsync();

        // Assert
        Assert.Collection(result,
            user =>
            {
                Assert.Equal(user1Id.ToString(), user.Id);
                Assert.Equal("Random User", user.FullName);
                Assert.Equal("test@test.com", user.Email);
                Assert.Equal("Admin", user.Role);
                Assert.True(user.IsActive);
            },
            user =>
            {
                Assert.Equal(user2Id.ToString(), user.Id);
                Assert.Equal("AnotherRandom User", user.FullName);
                Assert.Equal("test2@test.com", user.Email);
                Assert.Equal("No Role Assigned", user.Role);
                Assert.True(user.IsActive);
            }
        );
    }

    [Fact]
    public async Task GetDetailsAsync_ReturnsGuardianViewModelIfUserHasStudentRole()
    {
        // Arrange
        Guid guardianId = Guid.NewGuid();
        ApplicationUser guardianUser = new()
        {
            Id = guardianId,
            UserName = "test_guardian@test.com",
            NormalizedUserName = "TEST_GUARDIAN@TEST.COM",
            Email = "test_guardian@test.com",
            NormalizedEmail = "TEST_GUARDIAN@TEST.COM",
            FirstName = "Random",
            LastName = "Guardian",
            BirthDate = DateTime.ParseExact("2000-01-01", "yyyy-MM-dd", CultureInfo.InvariantCulture),
            Address = "Random Address",
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
            Id = studentId,
            ApplicationUser = studentUser,
            GuardianId = guardianId,
            Guardian = guardian,
            SchoolId = Guid.NewGuid(),
            ClassroomId = Guid.NewGuid()
        };

        student.Guardian.Id = guardianId;
        student.Guardian = guardian;

        await _dbContextMock.Users.AddAsync(studentUser);
        await _dbContextMock.Students.AddAsync(student);
        await _dbContextMock.SaveChangesAsync();

        _userManagerMock.Setup(m => m.GetRolesAsync(studentUser))
            .ReturnsAsync(new List<string> { "Student" });

        _guardianServiceMock.Setup(m => m.GetGuardianAssignedByUserIdAsync(studentId.ToString()))
            .ReturnsAsync(guardian);

        // Act
        UserDetailsViewModel? result = await _sut.GetDetailsAsync(studentId.ToString());

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Student", result!.Role);
        Assert.NotNull(result.Guardian);
        Assert.Equal(guardianId.ToString(), result.Guardian!.Id);
        Assert.Equal(guardianUser.GetFullName(), result.Guardian.FullName);
        Assert.Equal(guardianUser.Address, result.Guardian.Address);
        Assert.Equal(guardianUser.Email, result.Guardian.Email);
        Assert.Equal(guardianUser.PhoneNumber, result.Guardian.PhoneNumber);
        Assert.NotNull(result.Students);
    }

    [Fact]
    public async Task GetDetailsAsync_ReturnsStudentsIfUserHasGuardianRole()
    {
        // Arrange
        Guid guardianId = Guid.NewGuid();
        ApplicationUser guardianUser = new()
        {
            Id = guardianId,
            UserName = "test_guardian@test.com",
            NormalizedUserName = "TEST_GUARDIAN@TEST.COM",
            Email = "test_guardian@test.com",
            NormalizedEmail = "TEST_GUARDIAN@TEST.COM",
            FirstName = "Random",
            LastName = "Guardian",
            BirthDate = DateTime.ParseExact("2000-01-01", "yyyy-MM-dd", CultureInfo.InvariantCulture),
            Address = "Random Address",
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
            Id = studentId,
            ApplicationUser = studentUser,
            GuardianId = guardianId,
            Guardian = guardian,
            SchoolId = Guid.NewGuid(),
            ClassroomId = Guid.NewGuid()
        };

        student.Guardian.Id = guardianId;
        student.Guardian = guardian;

        await _dbContextMock.Users.AddAsync(studentUser);
        await _dbContextMock.Students.AddAsync(student);
        await _dbContextMock.SaveChangesAsync();

        _userManagerMock.Setup(m => m.GetRolesAsync(guardianUser))
            .ReturnsAsync(new List<string> { "Guardian" });

        _guardianServiceMock.Setup(m => m.GetGuardianAssignedByUserIdAsync(guardianId.ToString()))
            .ReturnsAsync(guardian);

        // Act
        UserDetailsViewModel? result = await _sut.GetDetailsAsync(guardianId.ToString());

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Guardian", result!.Role);
        Assert.NotNull(result.Students);
        Assert.Equal(guardianId.ToString(), result.Id);
        Assert.Equal(guardianUser.GetFullName(), result.FullName);
        Assert.Equal(guardianUser.Address, result.Address);
        Assert.Equal(guardianUser.Email, result.Email);
        Assert.Equal(guardianUser.PhoneNumber, result.PhoneNumber);
    }

    public void Dispose()
    {
        _dbContextMock.Dispose();
        GC.SuppressFinalize(this);
    }
}