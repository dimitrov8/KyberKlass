using KyberKlass.Data;
using KyberKlass.Data.Models;
using KyberKlass.Services.Data;
using KyberKlass.Services.Data.Interfaces;
using KyberKlass.Services.Data.Interfaces.Guardians;
using KyberKlass.Web.ViewModels.Admin;
using KyberKlass.Web.ViewModels.Admin.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Moq;
using System.Globalization;

namespace KyberKlass.Tests.Services;
public class StudentServiceTests : IDisposable
{
    private readonly DbContextOptions<KyberKlassDbContext> _options;
    private readonly KyberKlassDbContext _dbContextMock;
    private readonly Mock<IUserService> _userServiceMock;
    private readonly Mock<IGuardianService> _guardianServiceMock;
    private readonly IStudentService _sut;

    public StudentServiceTests()
    {
        _options = new DbContextOptionsBuilder<KyberKlassDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        _dbContextMock = new KyberKlassDbContext(_options);

        _userServiceMock = new Mock<IUserService>();
        _guardianServiceMock = new Mock<IGuardianService>();

        _sut = new StudentService(_dbContextMock, _userServiceMock.Object, _guardianServiceMock.Object);
    }

    [Fact]
    public async Task GetByIdAsync_ReturnsStudentWithCorrectId()
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
        Student student = new()
        {
            Id = studentId,
            GuardianId = guardianId,
            SchoolId = Guid.NewGuid(),
            ClassroomId = Guid.NewGuid(),
            ApplicationUser = new()
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
            }
        };

        Guardian guardian = new()
        {
            Id = guardianId,
            ApplicationUser = guardianUser,
            Students = new List<Student> { student }
        };

        student.Guardian = guardian;

        await _dbContextMock.Students.AddAsync(student);
        await _dbContextMock.SaveChangesAsync();

        // Act
        Student? result = await _sut.GetByIdASync(studentId.ToString());

        // Assert
        Assert.NotNull(result);
        Assert.Equal(studentId, result!.Id);
        Assert.Equal(student.Guardian.Id, result.GuardianId);
        Assert.Equal(student.Guardian, result.Guardian);
        Assert.Equal(student.SchoolId, result.SchoolId);
        Assert.Equal(student.ClassroomId, result.ClassroomId);
    }

    [Fact]
    public async Task GetByIdAsync_InvalidIdReturnsNull()
    {
        // Arrange
        Guid invalidId = Guid.NewGuid();

        // Act
        Student? result = await _sut.GetByIdASync(invalidId.ToString());

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task StudentChangeGuardianAsync_BothExistChangesGuardian()
    {
        // Arrange
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

        Guid previousGuardianId = Guid.NewGuid();
        ApplicationUser previousGuardianUser = new()
        {
            Id = previousGuardianId,
            UserName = "pr@test.com",
            NormalizedUserName = "PR@TEST.COM",
            Email = "PR@test.com",
            NormalizedEmail = "PR@TEST.COM",
            FirstName = "Test",
            LastName = "Guardian",
            BirthDate = DateTime.ParseExact("1980-01-01", "yyyy-MM-dd", CultureInfo.InvariantCulture),
            Address = "Test Address",
            IsActive = true
        };

        Guid newGuardianId = Guid.NewGuid();
        ApplicationUser newGuardianUser = new()
        {
            Id = newGuardianId,
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

        Student student = new()
        {
            Id = studentId,
            ApplicationUser = studentUser,
            GuardianId = previousGuardianId,
            SchoolId = Guid.NewGuid(),
            ClassroomId = Guid.NewGuid()
        };

        Guardian previousGuardian = new()
        {
            Id = previousGuardianId,
            ApplicationUser = previousGuardianUser
        };

        Guardian newGuardian = new()
        {
            Id = newGuardianId,
            ApplicationUser = newGuardianUser
        };

        student.Guardian = previousGuardian;

        _guardianServiceMock.Setup(m => m.GetByIdAsync(newGuardianId.ToString()))
            .ReturnsAsync(newGuardian);

        await _dbContextMock.Students.AddAsync(student);
        await _dbContextMock.Guardians.AddAsync(previousGuardian);
        await _dbContextMock.Guardians.AddAsync(newGuardian);
        await _dbContextMock.SaveChangesAsync();

        // Act
        bool result = await _sut.StudentChangeGuardianAsync(studentId.ToString(), newGuardianId.ToString());

        // Assert
        Assert.True(result);
        Assert.Equal(newGuardian.Id, student.GuardianId);
    }

    [Fact]
    public async Task StudentChangeGuardianAsync_StudentDoesNotExistReturnsFalse()
    {
        // Arrange
        string invalidStudentId = Guid.NewGuid().ToString();
        string guardianId = Guid.NewGuid().ToString();

        // Act
        bool result = await _sut.StudentChangeGuardianAsync(invalidStudentId, guardianId);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public async Task StudentChangeGuardianAsync_NewGuardianDoesNotExistReturnsFalse()
    {
        // Arrange
        string studentId = Guid.NewGuid().ToString();
        string invalidGuardian = Guid.NewGuid().ToString();

        // Act
        bool result = await _sut.StudentChangeGuardianAsync(studentId, invalidGuardian);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public async Task StudentChangeGuardianAsync_StudentHasSameGuardianReturnsFalse()
    {
        // Arrange
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
            SchoolId = Guid.NewGuid(),
            ClassroomId = Guid.NewGuid(),
            Guardian = guardian
        };

        guardian.Students = new List<Student> { student };


        _guardianServiceMock.Setup(m => m.GetByIdAsync(guardianId.ToString()))
            .ReturnsAsync(guardian);

        await _dbContextMock.Students.AddAsync(student);
        await _dbContextMock.Guardians.AddAsync(guardian);
        await _dbContextMock.SaveChangesAsync();

        // Act
        bool result = await _sut.StudentChangeGuardianAsync(studentId.ToString(), guardianId.ToString());

        // Assert
        Assert.False(result);
        Assert.Equal(student.Guardian.Id, guardianId);
    }

    [Fact]
    public async Task GetStudentChangeGuardianAsync_ReturnsViewModelWithUserDetailsAndAvailableGuardians()
    {
        // Arrange
        Guid userId = Guid.NewGuid();
        UserDetailsViewModel userDetails = new()
        {
            Id = userId.ToString(),
            FullName = "Barry Thompson",
            BirthDate = "1999-01-01",
            Address = "Random Address",
            PhoneNumber = "0888888888",
            Email = "test@test.com",
            Role = "No Role Assigned",
            IsActive = "true"
        };

        List<BasicViewModel> availableGuardians = new()
        {
            new()
                { Id = "1", Name = "Guardian 1" },
            new()
                { Id = "2", Name = "Guardian 2" }
        };

        _userServiceMock.Setup(m => m.GetDetailsAsync(userId.ToString()))
            .ReturnsAsync(userDetails);

        _guardianServiceMock.Setup(m => m.GetAllGuardiansAsync())
            .ReturnsAsync(availableGuardians);

        // Act
        Web.ViewModels.Admin.Student.StudentChangeGuardianViewModel result = await _sut.GetStudentChangeGuardianAsync(userId.ToString());

        // Assert
        Assert.NotNull(result);
        Assert.NotNull(result.UserDetails);
        Assert.Equal(userDetails.Id, result.UserDetails!.Id);
        Assert.Equal(userDetails.FullName, result.UserDetails.FullName);
        Assert.Equal(userDetails.BirthDate, result.UserDetails.BirthDate);
        Assert.Equal(userDetails.Address, result.UserDetails.Address);
        Assert.Equal(userDetails.PhoneNumber, result.UserDetails.PhoneNumber);
        Assert.Equal(userDetails.Email, result.UserDetails.Email);
        Assert.Equal(userDetails.Role, result.UserDetails.Role);
        Assert.Equal(userDetails.IsActive, result.UserDetails.IsActive);

        Assert.NotNull(result.AvailableGuardians);
        Assert.Equal(availableGuardians.Count, result.AvailableGuardians.Count());

        foreach (BasicViewModel expectedGuardian in availableGuardians)
        {
            Assert.Contains(result.AvailableGuardians, g => g.Id == expectedGuardian.Id && g.Name == expectedGuardian.Name);
        }
    }

    [Fact]
    public async Task AllAsync_ReturnsAllUsersWithStudentRole()
    {
        // Arrange
        Guid studentRoleId = Guid.NewGuid();
        Guid student1Id = Guid.NewGuid();
        Guid student2Id = Guid.NewGuid();
        List<ApplicationUser> students = new()
        {
            new()
            {
                Id = student1Id,
                UserName = "test_student1@test.com",
                NormalizedUserName = "TEST_STUDENT1@TEST.COM",
                Email = "test_student1@test.com",
                NormalizedEmail = "TEST_STUDENT1@TEST.COM",
                FirstName = "Random",
                LastName = "Student",
                BirthDate = DateTime.ParseExact("2000-01-01", "yyyy-MM-dd", CultureInfo.InvariantCulture),
                Address = "Random Address",
                IsActive = true
            },
            new()
            {
                Id = student2Id,
                UserName = "test_student2@test.com",
                NormalizedUserName = "TEST_STUDENT2@TEST.COM",
                Email = "test_student2@test.com",
                NormalizedEmail = "TEST_STUDENT2@TEST.COM",
                FirstName = "AnotherRandom",
                LastName = "Student",
                BirthDate = DateTime.ParseExact("2001-01-01", "yyyy-MM-dd", CultureInfo.InvariantCulture),
                Address = "Random Address",
                IsActive = true
            }
        };

        List<IdentityRole<Guid>> roles = new()
        {
            new()
            {
                Id = studentRoleId,
                Name = "Student",
                NormalizedName = "STUDENT",
                ConcurrencyStamp = Guid.NewGuid().ToString()
            }
        };

        List<IdentityUserRole<Guid>> userRoles = new()
        {
            new()
            {
                UserId = student1Id,
                RoleId = studentRoleId
            },
            new()
            {
                UserId = student2Id,
                RoleId = studentRoleId
            }
        };

        await _dbContextMock.Roles.AddRangeAsync(roles);
        await _dbContextMock.Users.AddRangeAsync(students);
        await _dbContextMock.UserRoles.AddRangeAsync(userRoles);
        await _dbContextMock.SaveChangesAsync();

        // Act
        IEnumerable<UserViewModel>? result = await _sut.AllAsync();

        // Assert
        Assert.NotNull(result);
        List<UserViewModel> resultViewModels = result!.ToList();
        Assert.NotEmpty(resultViewModels);
        Assert.Equal(students.Count, resultViewModels.Count);

        foreach (ApplicationUser expectedUser in students)
        {
            UserViewModel? userViewModel = resultViewModels.FirstOrDefault(u => u.Id == expectedUser.Id.ToString());
            Assert.NotNull(userViewModel);
            Assert.Equal(expectedUser.Email, userViewModel!.Email);
            Assert.Equal(expectedUser.GetFullName(), userViewModel.FullName);
            Assert.Equal("Student", userViewModel.Role);
            Assert.Equal(expectedUser.IsActive, userViewModel.IsActive);
        }
    }

    [Fact]
    public async Task AllAsync_ReturnsNullWhenStudentRoleIdIsInvalid()
    {
        // Arrange
        List<IdentityRole<Guid>> roles = new()
        {
            new()
            {
                Id = Guid.NewGuid(),
                Name = "Teacher",
                NormalizedName = "TEACHER",
                ConcurrencyStamp = Guid.NewGuid().ToString()
            }
        };

        await _dbContextMock.Roles.AddRangeAsync(roles);
        await _dbContextMock.SaveChangesAsync();

        // Act
        IEnumerable<UserViewModel>? result = await _sut.AllAsync();

        // Assert
        Assert.Null(result);
    }


    public async void Dispose()
    {
        await _dbContextMock.DisposeAsync();
        GC.SuppressFinalize(this);
    }
}