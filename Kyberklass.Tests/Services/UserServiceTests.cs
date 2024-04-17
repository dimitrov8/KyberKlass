namespace KyberKlass.Tests.Services;

using System.Globalization;
using Data;
using Data.Models;
using KyberKlass.Services.Data;
using KyberKlass.Services.Data.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Moq;
using Web.ViewModels.Admin.User;

public class UserServiceTests : IDisposable
{
    private readonly DbContextOptions<KyberKlassDbContext> _options;
    private readonly KyberKlassDbContext _dbContextMock;
    private readonly Mock<UserManager<ApplicationUser>> _userManagerMock;
    private readonly Mock<RoleManager<IdentityRole<Guid>>> _roleManagerMock;
    private readonly Mock<IGuardianService> _guardianServiceMock;
    private readonly Mock<ISchoolService> _schoolServiceMock;
    private readonly UserService _sut;

    public UserServiceTests()
    {
        this._options = new DbContextOptionsBuilder<KyberKlassDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        this._dbContextMock = new KyberKlassDbContext(this._options);

        this._userManagerMock = new Mock<UserManager<ApplicationUser>>(
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

        this._roleManagerMock = new Mock<RoleManager<IdentityRole<Guid>>>(
            Mock.Of<IRoleStore<IdentityRole<Guid>>>(),
            null,
            null,
            null,
            null
        );

        this._guardianServiceMock = new Mock<IGuardianService>();
        this._schoolServiceMock = new Mock<ISchoolService>();

        this._sut = new UserService(this._dbContextMock, this._userManagerMock.Object, this._roleManagerMock.Object, this._guardianServiceMock.Object,
            this._schoolServiceMock.Object);
    }


    [Fact]
    public async Task GetUserById_ReturnsUser()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var user = new ApplicationUser
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

        await this._dbContextMock.Users.AddAsync(user);
        await this._dbContextMock.SaveChangesAsync();

        // Act
        var result = await this._sut.GetUserById(user.Id.ToString());

        Assert.NotNull(result);
        Assert.Equal(userId, result!.Id);
    }

    [Fact]
    public async Task GetUserById_ReturnsNullIfUserDoesNotExist()
    {
        // Arrange
        var invalidUserId = Guid.NewGuid();

        // Act
        var result = await this._sut.GetUserById(invalidUserId.ToString());

        Assert.Null(result);
    }

    [Fact]
    public async Task GetRoleNameByIdAsyncReturnsRoleName()
    {
        // Arrange
        string roleId = Guid.NewGuid().ToString();
        string roleName = "Admin";
        var role = new IdentityRole<Guid>(roleName);

        this._roleManagerMock.Setup(m => m.FindByIdAsync(roleId))
            .ReturnsAsync(role);

        // Act
        string? result = await this._sut.GetRoleNameByIdAsync(roleId);

        // Assert
        Assert.Equal(roleName, result);
    }

    [Fact]
    public async Task GetRoleNameByIdAsync_ReturnsNullForInvalidRoleId()
    {
        // Arrange
        string invalidRoleId = Guid.NewGuid().ToString();

        this._roleManagerMock.Setup(m => m.FindByIdAsync(invalidRoleId))
            .ReturnsAsync((IdentityRole<Guid>)null!);

        // Act
        string? result = await this._sut.GetRoleNameByIdAsync(invalidRoleId);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task GetDetailsAsync_ReturnsUserWithNoRoleDetailsViewModel()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var user = new ApplicationUser
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

        var viewModel = new UserDetailsViewModel
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

        await this._dbContextMock.AddAsync(user);
        await this._dbContextMock.SaveChangesAsync();

        this._userManagerMock.Setup(m => m.GetRolesAsync(user)).ReturnsAsync(new List<string>());

        // Act
        var result = await this._sut.GetDetailsAsync(userId.ToString());

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
        var result = await this._sut.GetDetailsAsync(invalidUserId);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task GetDetailsAsync_RoleIsSetRight()
    {
        // Arrange 
        var studentUser = new ApplicationUser
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

        var guardianUser = new ApplicationUser
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

        var guardian = new Guardian
        {
            Id = guardianUser.Id,
            ApplicationUser = guardianUser
        };

        var student = new Student
        {
            Id = studentUser.Id,
            ApplicationUser = studentUser,
            GuardianId = guardian.Id,
            Guardian = guardian
        };

        guardian.Students = new List<Student> { student };

        await this._dbContextMock.Users.AddAsync(studentUser);
        await this._dbContextMock.SaveChangesAsync();

        this._userManagerMock.Setup(m => m.GetRolesAsync(studentUser))
            .ReturnsAsync(new List<string> { "Student" });

        // Act
        var result = await this._sut.GetDetailsAsync(studentUser.Id.ToString());

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Student", result!.Role);
    }

    [Fact]
    public async Task GetForEditAsync_ReturnsUserEditFormModelWhenUserExists()
    {
        // Arrange
        var userId = Guid.NewGuid();
        string expectedBirthDate = "1990-05-15";
        var user = new ApplicationUser
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

        await this._dbContextMock.Users.AddAsync(user);
        await this._dbContextMock.SaveChangesAsync();

        // Act
        var result = await this._sut.GetForEditAsync(userId.ToString());

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
        var invalidUserId = Guid.NewGuid();

        // Act
        var result = await this._sut.GetForEditAsync(invalidUserId.ToString());

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task EditAsync_ReturnsUserEditFormModelWhenUserExists()
    {
        // Arrange
        var userId = Guid.NewGuid();
        string expectedBirthDate = "1990-05-15";
        var model = new UserEditFormModel
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

        var user = new ApplicationUser
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

        await this._dbContextMock.AddAsync(user);
        await this._dbContextMock.SaveChangesAsync();

        // Act
        var result = await this._sut.EditAsync(userId.ToString(), model);

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
        var invalidUserId = Guid.NewGuid();
        var model = new UserEditFormModel();

        // Act
        var result = await this._sut.EditAsync(invalidUserId.ToString(), model);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task EditAsync_UpdatesAllProperties()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var model = new UserEditFormModel
        {
            FirstName = "NewFirstName",
            LastName = "NewLastName",
            BirthDate = "2000-01-01",
            Address = "NewAddress",
            PhoneNumber = "NewPhoneNumber",
            Email = "newemail@example.com",
            IsActive = true
        };

        var user = new ApplicationUser
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

        await this._dbContextMock.AddAsync(user);
        await this._dbContextMock.SaveChangesAsync();

        // Act
        var result = await this._sut.EditAsync(userId.ToString(), model);

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
        IEnumerable<UserViewModel> result = await this._sut.AllAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Empty(result);
    }

    [Fact]
    public async Task AllAsync_ReturnsAllUsersWithRoles()
    {
        // Arrange
        var user1Id = Guid.NewGuid();
        var user2Id = Guid.NewGuid();
        var adminRoleId = Guid.NewGuid();

        var users = new List<ApplicationUser>
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

        var role = new IdentityRole<Guid>
        {
            Id = adminRoleId,
            Name = "Admin"
        };

        var userRole = new IdentityUserRole<Guid>
        {
            UserId = user1Id,
            RoleId = adminRoleId
        };

        await this._dbContextMock.Users.AddRangeAsync(users);
        await this._dbContextMock.Roles.AddAsync(role);
        await this._dbContextMock.UserRoles.AddAsync(userRole);
        await this._dbContextMock.SaveChangesAsync();

        // Act
        IEnumerable<UserViewModel> result = await this._sut.AllAsync();

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
        var guardianId = Guid.NewGuid();
        var guardianUser = new ApplicationUser
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
            Id = studentId,
            ApplicationUser = studentUser,
            GuardianId = guardianId,
            Guardian = guardian,
            SchoolId = Guid.NewGuid(),
            ClassroomId = Guid.NewGuid()
        };

        student.Guardian.Id = guardianId;
        student.Guardian = guardian;

        await this._dbContextMock.Users.AddAsync(studentUser);
        await this._dbContextMock.Students.AddAsync(student);
        await this._dbContextMock.SaveChangesAsync();

        this._userManagerMock.Setup(m => m.GetRolesAsync(studentUser))
            .ReturnsAsync(new List<string> { "Student" });

        this._guardianServiceMock.Setup(m => m.GetGuardianByUserIdAsync(studentId.ToString()))
            .ReturnsAsync(guardian);

        // Act
        var result = await this._sut.GetDetailsAsync(studentId.ToString());

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
        var guardianId = Guid.NewGuid();
        var guardianUser = new ApplicationUser
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
            Id = studentId,
            ApplicationUser = studentUser,
            GuardianId = guardianId,
            Guardian = guardian,
            SchoolId = Guid.NewGuid(),
            ClassroomId = Guid.NewGuid()
        };

        student.Guardian.Id = guardianId;
        student.Guardian = guardian;

        await this._dbContextMock.Users.AddAsync(studentUser);
        await this._dbContextMock.Students.AddAsync(student);
        await this._dbContextMock.SaveChangesAsync();

        this._userManagerMock.Setup(m => m.GetRolesAsync(guardianUser))
            .ReturnsAsync(new List<string> { "Guardian" });

        this._guardianServiceMock.Setup(m => m.GetGuardianByUserIdAsync(guardianId.ToString()))
            .ReturnsAsync(guardian);

        // Act
        var result = await this._sut.GetDetailsAsync(guardianId.ToString());

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

    [Fact]
    public async Task UpdateRoleAsync_UserNotFoundReturnsFalse()
    {
        // Arrange
        var invalidUserId = Guid.NewGuid();
        var roleId = Guid.NewGuid();

        this._userManagerMock.Setup(m => m.FindByIdAsync(invalidUserId.ToString()))!
            .ReturnsAsync((ApplicationUser?)null);

        // Act
        bool result = await this._sut.UpdateRoleAsync(invalidUserId.ToString(), roleId.ToString(), null, null, null);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public async Task UpdateRoleAsync_RoleNotFoundReturnsFalse()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var invalidRoleId = Guid.NewGuid();
        var user = new ApplicationUser
        {
            Id = userId,
            UserName = "test_user@test.com",
            NormalizedUserName = "TEST_USER1@TEST.COM",
            Email = "test_user1@test.com",
            NormalizedEmail = "TEST_USER1@TEST.COM",
            FirstName = "Random",
            LastName = "User",
            BirthDate = DateTime.ParseExact("2000-01-01", "yyyy-MM-dd", CultureInfo.InvariantCulture),
            Address = "Random Address",
            IsActive = true
        };

        this._userManagerMock.Setup(m => m.FindByIdAsync(userId.ToString()))
            .ReturnsAsync(user);

        this._roleManagerMock.Setup(m => m.FindByIdAsync(invalidRoleId.ToString()))!
            .ReturnsAsync((IdentityRole<Guid>?)null);

        // Act
        bool result = await this._sut.UpdateRoleAsync(userId.ToString(), invalidRoleId.ToString(), null, null, null);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public async Task GetForUpdateRoleAsync_UserNotFoundReturnsNull()
    {
        // Arrange
        string userId = Guid.NewGuid().ToString();

        // Act
        var result = await this._sut.GetForUpdateRoleAsync(userId);

        // Assert
        Assert.Null(result);
    }

    public async void Dispose()
    {
        await this._dbContextMock.DisposeAsync();
    }
}