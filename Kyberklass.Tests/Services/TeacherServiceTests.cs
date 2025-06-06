﻿using KyberKlass.Data;
using KyberKlass.Data.Models;
using KyberKlass.Services.Data;
using KyberKlass.Services.Data.Interfaces;
using KyberKlass.Web.ViewModels.Admin;
using KyberKlass.Web.ViewModels.Admin.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Moq;
using System.Globalization;

namespace KyberKlass.Tests.Services;
public class TeacherServiceTests : IDisposable
{
    private readonly DbContextOptions<KyberKlassDbContext> _options;
    private readonly KyberKlassDbContext _dbContextMock;
    private readonly Mock<UserManager<ApplicationUser>> _userManagerMock;
    private readonly ITeacherService _sut;

    public TeacherServiceTests()
    {
        _options = new DbContextOptionsBuilder<KyberKlassDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        _dbContextMock = new KyberKlassDbContext(_options);

#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
        _userManagerMock = new Mock<UserManager<ApplicationUser>>(Mock.Of<IUserStore<ApplicationUser>>(), null, null, null, null, null, null, null, null);

        _sut = new TeacherService(_dbContextMock, _userManagerMock.Object);
    }

    [Fact]
    public async Task IsTeacherAssignedToClassroomAsync_ReturnsTrueWhenTeacherIsAssigned()
    {
        // Arrange
        Guid teacherId = Guid.NewGuid();
        Guid classroomId = Guid.NewGuid();
        ApplicationUser teacherUser = new()
        {
            Id = teacherId,
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

        Teacher teacher = new()
        {
            Id = teacherId,
            ApplicationUser = teacherUser
        };

        Classroom classroom = new() { Id = classroomId, Name = "11E", TeacherId = teacherId, Teacher = teacher };

        await _dbContextMock.Users.AddAsync(teacherUser);
        await _dbContextMock.Classrooms.AddAsync(classroom);
        await _dbContextMock.SaveChangesAsync();

        // Act
        bool result = await _sut.IsTeacherAssignedToClassroomAsync(teacherId.ToString());

        // Assert
        Assert.True(result);
    }

    [Fact]
    public async Task IsTeacherAssignedToClassroomAsync_ReturnsFalseWhenTeacherIsNotAssigned()
    {
        // Arrange
        Guid teacher1Id = Guid.NewGuid();
        ApplicationUser teacherUser1 = new()
        {
            Id = teacher1Id,
            UserName = "test_teacher@test.com",
            NormalizedUserName = "TEST_TEACHER@TEST.COM",
            Email = "test_teacher@test.com",
            NormalizedEmail = "TEST_TEACHER@TEST.COM",
            FirstName = "Random",
            LastName = "Teacher",
            BirthDate = DateTime.ParseExact("2000-01-01", "yyyy-MM-dd", CultureInfo.InvariantCulture),
            Address = "Random Address",
            IsActive = true
        };

        Guid teacher2Id = Guid.NewGuid();
        ApplicationUser teacherUser2 = new()
        {
            Id = teacher2Id,
            UserName = "test_diff@test.com",
            NormalizedUserName = "TEST_DIFF@TEST.COM",
            Email = "test_diff@test.com",
            NormalizedEmail = "TEST_DIFF@TEST.COM",
            FirstName = "RandomFirstName",
            LastName = "RandomLastName",
            BirthDate = DateTime.ParseExact("2005-01-01", "yyyy-MM-dd", CultureInfo.InvariantCulture),
            Address = "Random Address",
            IsActive = true
        };

        Teacher teacher2 = new()
        {
            Id = teacher2Id,
            ApplicationUser = teacherUser2
        };

        Guid classroomId = Guid.NewGuid();
        Classroom classroom = new() { Id = classroomId, Name = "11E", TeacherId = teacher2Id, Teacher = teacher2 };

        await _dbContextMock.Users.AddRangeAsync(teacherUser1, teacherUser2);
        await _dbContextMock.Classrooms.AddAsync(classroom);
        await _dbContextMock.SaveChangesAsync();

        // Act
        bool result = await _sut.IsTeacherAssignedToClassroomAsync(teacher1Id.ToString());

        // Assert
        Assert.False(result);
    }

    [Fact]
    public async Task GetUnassignedTeachersAsync_ReturnsEmptyListWhenNoTeachersExist()
    {
        // Arrange
        Mock<UserManager<ApplicationUser>> userManagerMock = new(Mock.Of<IUserStore<ApplicationUser>>(), null, null, null, null, null, null, null, null);
        userManagerMock.Setup(m => m.GetUsersInRoleAsync("Teacher"))
            .ReturnsAsync(new List<ApplicationUser>());

        TeacherService sut = new(_dbContextMock, userManagerMock.Object);

        // Act
        IEnumerable<BasicViewModel> result = await sut.GetUnassignedTeachersAsync();

        // Assert
        Assert.Empty(result);
    }

    [Fact]
    public async Task GetUnassignedTeachersAsync_ReturnsUnassignedTeachers()
    {
        // Arrange
        Guid teacher1Id = Guid.NewGuid();
        ApplicationUser teacherUser1 = new()
        {
            Id = teacher1Id,
            UserName = "test1@test.com",
            NormalizedUserName = "TEST1@TEST.COM",
            Email = "test1@test.com",
            NormalizedEmail = "TEST1@TEST.COM",
            FirstName = "Kyle",
            LastName = "Wilson",
            Address = "Test Address 1",
            BirthDate = DateTime.ParseExact("1991-01-01", "yyyy-MM-dd", CultureInfo.InvariantCulture),
            IsActive = true
        };

        Guid teacher2Id = Guid.NewGuid();
        ApplicationUser teacherUser2 = new()
        {
            Id = teacher2Id,
            UserName = "test2@test.com",
            NormalizedUserName = "TEST2@TEST.COM",
            Email = "test2@test.com",
            NormalizedEmail = "TEST2@TEST.COM",
            FirstName = "Wilson",
            LastName = "Smith",
            Address = "Test Address 2",
            BirthDate = DateTime.ParseExact("1992-01-01", "yyyy-MM-dd", CultureInfo.InvariantCulture),
            IsActive = true
        };

        Guid teacher3Id = Guid.NewGuid();
        ApplicationUser teacherUser3 = new()
        {
            Id = teacher3Id,
            UserName = "test3@test.com",
            NormalizedUserName = "TEST3@TEST.COM",
            Email = "test3@test.com",
            NormalizedEmail = "TEST3@TEST.COM",
            FirstName = "Smith",
            LastName = "Johnson",
            Address = "Test Address 2",
            BirthDate = DateTime.ParseExact("1995-01-01", "yyyy-MM-dd", CultureInfo.InvariantCulture),
            IsActive = true
        };

        Teacher teacher1 = new()
        {
            Id = teacher1Id,
            ApplicationUser = teacherUser1
        };

        Teacher teacher2 = new()
        {
            Id = teacher2Id,
            ApplicationUser = teacherUser2
        };

        Teacher teacher3 = new()
        {
            Id = teacher3Id,
            ApplicationUser = teacherUser3
        };

        Classroom classroom1 = new() { Id = Guid.NewGuid(), Name = "11E", TeacherId = teacher1Id, Teacher = teacher1 };
        Classroom classroom2 = new() { Id = Guid.NewGuid(), Name = "12A", TeacherId = teacher2Id, Teacher = teacher2 };

        await _dbContextMock.Users.AddRangeAsync(new List<ApplicationUser> { teacherUser1, teacherUser2, teacherUser3 });
        await _dbContextMock.Teachers.AddRangeAsync(new List<Teacher> { teacher1, teacher2, teacher3 });
        await _dbContextMock.Classrooms.AddRangeAsync(new List<Classroom> { classroom1, classroom2 });
        await _dbContextMock.SaveChangesAsync();

        _userManagerMock.Setup(m => m.GetUsersInRoleAsync("Teacher"))
            .ReturnsAsync(new List<ApplicationUser> { teacherUser1, teacherUser2, teacherUser3 });

        // Act
        IEnumerable<BasicViewModel> result = await _sut.GetUnassignedTeachersAsync();

        // Assert
        Assert.Collection(result, item =>
        {
            Assert.Equal(teacher3Id.ToString(), item.Id);
            Assert.Equal(teacherUser3.GetFullName(), item.Name);
        });
    }

    [Fact]
    public async Task GetUnassignedTeachersAsync_ReturnsEmptyCollectionWhenAllTeachersAreAssigned()
    {
        // Arrange
        Guid teacher1Id = Guid.NewGuid();
        ApplicationUser teacherUser1 = new()
        {
            Id = teacher1Id,
            UserName = "test1@test.com",
            NormalizedUserName = "TEST1@TEST.COM",
            Email = "test1@test.com",
            NormalizedEmail = "TEST1@TEST.COM",
            FirstName = "Kyle",
            LastName = "Wilson",
            Address = "Test Address 1",
            BirthDate = DateTime.ParseExact("1991-01-01", "yyyy-MM-dd", CultureInfo.InvariantCulture),
            IsActive = true
        };

        Guid teacher2Id = Guid.NewGuid();
        ApplicationUser teacherUser2 = new()
        {
            Id = teacher2Id,
            UserName = "test2@test.com",
            NormalizedUserName = "TEST2@TEST.COM",
            Email = "test2@test.com",
            NormalizedEmail = "TEST2@TEST.COM",
            FirstName = "Wilson",
            LastName = "Smith",
            Address = "Test Address 2",
            BirthDate = DateTime.ParseExact("1992-01-01", "yyyy-MM-dd", CultureInfo.InvariantCulture),
            IsActive = true
        };

        Teacher teacher1 = new()
        {
            Id = teacher1Id,
            ApplicationUser = teacherUser1
        };

        Teacher teacher2 = new()
        {
            Id = teacher2Id,
            ApplicationUser = teacherUser2
        };

        Classroom classroom1 = new() { Id = Guid.NewGuid(), Name = "11E", TeacherId = teacher1Id, Teacher = teacher1 };
        Classroom classroom2 = new() { Id = Guid.NewGuid(), Name = "12A", TeacherId = teacher2Id, Teacher = teacher2 };

        await _dbContextMock.Users.AddRangeAsync(new List<ApplicationUser> { teacherUser1, teacherUser2 });
        await _dbContextMock.Teachers.AddRangeAsync(new List<Teacher> { teacher1, teacher2 });
        await _dbContextMock.Classrooms.AddRangeAsync(new List<Classroom> { classroom1, classroom2 });
        await _dbContextMock.SaveChangesAsync();

        _userManagerMock.Setup(m => m.GetUsersInRoleAsync("Teacher"))
            .ReturnsAsync(new List<ApplicationUser> { teacherUser1, teacherUser2 });

        // Act
        IEnumerable<BasicViewModel> result = await _sut.GetUnassignedTeachersAsync();

        // Assert
        Assert.Empty(result);
    }

    [Fact]
    public async Task AllAsync_ReturnsEmptyList_WhenNoTeachersExist()
    {
        // Arrange
        await _dbContextMock.Roles.AddAsync(new IdentityRole<Guid> { Id = Guid.NewGuid(), Name = "Teacher" });
        await _dbContextMock.SaveChangesAsync();

        // Act
        List<UserViewModel>? result = await _sut.AllAsync();

        // Assert
        Assert.Empty(result!);
    }

    [Fact]
    public async Task AllAsync_ReturnsListOfTeachersWhenTeachersExist()
    {
        // Arrange
        Guid teacherRoleId = Guid.NewGuid();
        await _dbContextMock.Roles.AddAsync(new IdentityRole<Guid> { Id = teacherRoleId, Name = "Teacher" });
        await _dbContextMock.SaveChangesAsync();

        Guid teacher1Id = Guid.NewGuid();
        Guid teacher2Id = Guid.NewGuid();
        List<ApplicationUser> teachers = new()
        {
            new()
            {
                Id = teacher1Id,
                UserName = "test@test.com",
                NormalizedUserName = "TEST@TEST.COM",
                Email = "test@test.com",
                NormalizedEmail = "TEST@TEST.COM",
                FirstName = "Brendan",
                LastName = "Osborne",
                BirthDate = DateTime.ParseExact("1990-01-01", "yyyy-MM-dd", CultureInfo.InvariantCulture),
                Address = "Test Address",
                IsActive = true
            },
            new()
            {
                Id = teacher2Id,
                UserName = "test2@test.com",
                NormalizedUserName = "TEST2@TEST.COM",
                Email = "test2@test.com",
                NormalizedEmail = "TEST2@TEST.COM",
                FirstName = "Test",
                LastName = "Osborne",
                BirthDate = DateTime.ParseExact("2000-01-01", "yyyy-MM-dd", CultureInfo.InvariantCulture),
                Address = "Test Address 2",
                IsActive = true
            }
        };

        await _dbContextMock.Users.AddRangeAsync(teachers);
        await _dbContextMock.SaveChangesAsync();

        foreach (ApplicationUser teacher in teachers)
        {
            await _dbContextMock.UserRoles.AddAsync(new IdentityUserRole<Guid> { UserId = teacher.Id, RoleId = teacherRoleId });
        }

        await _dbContextMock.SaveChangesAsync();

        // Act
        List<UserViewModel>? result = await _sut.AllAsync();

        // Assert
        Assert.Equal(teachers.Count, result!.Count);

        foreach (ApplicationUser teacher in teachers)
        {
            UserViewModel? teacherViewModel = result.FirstOrDefault(t => t.Id == teacher.Id.ToString());
            Assert.NotNull(teacherViewModel);
            Assert.Equal(teacher.Email, teacherViewModel!.Email);
            Assert.Equal(teacher.GetFullName(), teacherViewModel.FullName);
            Assert.Equal("Teacher", teacherViewModel.Role);
            Assert.True(teacherViewModel.IsActive);
        }
    }

    [Fact]
    public async Task AllAsync_ReturnsNullWhenTeacherRoleIdNotFound()
    {
        // Act
        List<UserViewModel>? result = await _sut.AllAsync();

        // Assert
        Assert.Null(result);
    }

    public async void Dispose()
    {
        await _dbContextMock.DisposeAsync();
        GC.SuppressFinalize(this);
    }
}