using KyberKlass.Data;
using KyberKlass.Data.Models;
using KyberKlass.Services.Data;
using KyberKlass.Services.Data.Interfaces;
using KyberKlass.Web.ViewModels.Admin;
using KyberKlass.Web.ViewModels.Admin.Classroom;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace KyberKlass.Tests.Services;
public class ClassroomServiceTests : IDisposable
{
    private readonly DbContextOptions<KyberKlassDbContext> _options;
    private readonly KyberKlassDbContext _dbContextMock;
    private readonly IClassroomService _sut;

    public ClassroomServiceTests()
    {
        _options = new DbContextOptionsBuilder<KyberKlassDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        _dbContextMock = new KyberKlassDbContext(_options);

        _sut = new ClassroomService(_dbContextMock);
    }

    [Fact]
    public async Task HasStudentsAssignedAsync_ReturnsTrueWhenStudentsAssigned()
    {
        // Arrange
        Guid classroomId = Guid.NewGuid();
        Classroom classroom = new() { Id = classroomId, Name = "11E" };

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

        Student student = new()
        {
            Id = studentId,
            ApplicationUser = studentUser,
            GuardianId = Guid.NewGuid(),
            SchoolId = Guid.NewGuid(),
            ClassroomId = classroomId,
            Classroom = classroom
        };

        await _dbContextMock.Classrooms.AddAsync(classroom);
        await _dbContextMock.Students.AddAsync(student);
        await _dbContextMock.SaveChangesAsync();

        // Act
        bool result = await _sut.HasStudentsAssignedAsync(classroomId.ToString());

        // Assert
        Assert.True(result);
    }

    [Fact]
    public async Task HasStudentsAssignedAsync_ReturnsFalseWhenNoStudentsAssigned()
    {
        // Arrange
        Guid classroomId = Guid.NewGuid();
        Classroom classroom = new() { Id = classroomId, Name = "11E" };

        await _dbContextMock.Classrooms.AddAsync(classroom);
        await _dbContextMock.SaveChangesAsync();

        // Act
        bool result = await _sut.HasStudentsAssignedAsync(classroomId.ToString());

        // Assert
        Assert.False(result);
    }


    [Fact]
    public async Task DeleteAsync_ReturnsTrueAndDeletesClassroomWhenNoStudentsAssigned()
    {
        // Arrange
        Guid classroomId = Guid.NewGuid();
        Classroom classroom = new() { Id = classroomId, Name = "11E" };

        await _dbContextMock.Classrooms.AddAsync(classroom);
        await _dbContextMock.SaveChangesAsync();

        // Act
        bool result = await _sut.DeleteAsync(classroomId.ToString());

        // Assert
        Assert.True(result);
        Assert.Null(await _dbContextMock.Classrooms.FindAsync(classroomId));
    }

    [Fact]
    public async Task DeleteAsync_ReturnsFalseAndDoesNotDeleteClassroom_WhenStudentsAssigned()
    {
        // Arrange
        Guid classroomId = Guid.NewGuid();
        Classroom classroom = new() { Id = classroomId, Name = "11E" };

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

        Student student = new()
        {
            Id = studentId,
            ApplicationUser = studentUser,
            GuardianId = Guid.NewGuid(),
            SchoolId = Guid.NewGuid(),
            ClassroomId = classroomId,
            Classroom = classroom
        };

        await _dbContextMock.Classrooms.AddAsync(classroom);
        await _dbContextMock.Students.AddAsync(student);
        await _dbContextMock.SaveChangesAsync();

        // Act
        bool result = await _sut.DeleteAsync(classroomId.ToString());

        // Assert
        Assert.False(result);
        Assert.NotNull(await _dbContextMock.Classrooms.FindAsync(classroomId));
    }

    [Fact]
    public async Task GetForDeleteAsync_ReturnsNullWhenClassroomDoesNotExist()
    {
        // Act
        ClassroomDetailsViewModel? result = await _sut.GetForDeleteAsync(Guid.NewGuid().ToString());

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task GetForDeleteAsync_ReturnsClassroomDetailsViewModelWhenClassroomExists()
    {
        // Arrange
        Guid teacherId = Guid.NewGuid();
        ApplicationUser teacherUser = new()
        {
            Id = teacherId,
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

        Teacher teacher = new()
        {
            Id = teacherId,
            ApplicationUser = teacherUser
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
            BirthDate = DateTime.ParseExact("2001-01-01", "yyyy-MM-dd", CultureInfo.InvariantCulture),
            Address = "Random Address 2",
            IsActive = true
        };

        Guid guardianId = Guid.NewGuid();
        ApplicationUser guardianUser = new()
        {
            Id = guardianId,
            UserName = "test_guardian@test.com",
            NormalizedUserName = "TEST_GUARDIAN@TEST.COM",
            Email = "test_guardian@test.com",
            NormalizedEmail = "TEST_GUARDIAN@TEST.COM",
            FirstName = "Mike",
            LastName = "Wilson",
            BirthDate = DateTime.ParseExact("1999-01-01", "yyyy-MM-dd", CultureInfo.InvariantCulture),
            Address = "Test Address 123",
            IsActive = true
        };

        Guardian guardian = new()
        {
            Id = guardianId,
            ApplicationUser = guardianUser
        };

        Guid schoolId = Guid.NewGuid();
        School school = new()
        {
            Id = schoolId,
            Name = "Test School",
            Address = "Test Address",
            Email = "testemail@test.com",
            PhoneNumber = "08888888888",
            IsActive = true
        };

        Student student = new()
        {
            Id = studentId,
            ApplicationUser = studentUser,
            GuardianId = guardianId,
            Guardian = guardian,
            SchoolId = schoolId,
            School = school
        };

        guardian.Students = new List<Student> { student };
        school.Students = new List<Student> { student };

        Guid classroomId = Guid.NewGuid();
        Classroom classroom = new()
        {
            Id = classroomId,
            Name = "11E",
            TeacherId = teacherId,
            Teacher = teacher,
            SchoolId = schoolId,
            School = school,
            IsActive = true
        };

        school.Classrooms = new List<Classroom> { classroom };
        student.ClassroomId = classroomId;
        student.Classroom = classroom;

        await _dbContextMock.Classrooms.AddAsync(classroom);
        await _dbContextMock.SaveChangesAsync();

        // Act
        ClassroomDetailsViewModel? result = await _sut.GetForDeleteAsync(classroomId.ToString());

        // Assert
        Assert.NotNull(result);
        Assert.Equal(classroomId.ToString(), result!.Id);
        Assert.Equal("11E", result.Name);
        Assert.Equal(teacherUser.GetFullName(), result.TeacherName);
    }

    [Fact]
    public async Task EditAsync_ReturnsFalseWhenClassroomDoesNotExist()
    {
        // Arrange
        string invalidId = Guid.NewGuid().ToString();
        AddClassroomViewModel invalidModel = new() { IsActive = true };

        // Act
        bool result = await _sut.EditAsync(invalidId, invalidModel);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public async Task EditAsync_ReturnsTrueWhenClassroomExistsAndIsSuccessfullyEdited()
    {
        // Arrange
        Guid classroomId = Guid.NewGuid();
        Classroom classroom = new() { Id = classroomId, Name = "11E", IsActive = true };

        await _dbContextMock.Classrooms.AddAsync(classroom);
        await _dbContextMock.SaveChangesAsync();

        AddClassroomViewModel model = new() { IsActive = false };

        // Act
        bool result = await _sut.EditAsync(classroomId.ToString(), model);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public async Task GetAllClassroomsBySchoolIdAsJsonAsync_ReturnsAllClassroomsForGivenSchoolId()
    {
        // Arrange
        string schoolId = Guid.NewGuid().ToString();
        List<Classroom> expectedClassrooms = new()
        {
            new()
                { Id = Guid.NewGuid(), Name = "11E", SchoolId = Guid.Parse(schoolId) },
            new()
                { Id = Guid.NewGuid(), Name = "12A", SchoolId = Guid.Parse(schoolId) },
            new()
                { Id = Guid.NewGuid(), Name = "10A", SchoolId = Guid.Parse(schoolId) }
        };

        await _dbContextMock.Classrooms.AddRangeAsync(expectedClassrooms);
        await _dbContextMock.SaveChangesAsync();

        // Act
        IEnumerable<BasicViewModel> result = await _sut.GetAllClassroomsBySchoolIdAsJsonAsync(schoolId);

        // Assert
        Assert.NotNull(result);
        List<BasicViewModel> resultViewModels = result.ToList();
        Assert.Equal(expectedClassrooms.Count, resultViewModels.Count);

        foreach (Classroom expectedClassroom in expectedClassrooms)
        {
            BasicViewModel? actual = resultViewModels.FirstOrDefault(c => c.Id == expectedClassroom.Id.ToString());
            Assert.NotNull(actual);
            Assert.Equal(expectedClassroom.Name, actual!.Name);
        }
    }

    [Fact]
    public async Task AllAsync_ReturnsAllClassroomsForGivenSchoolId()
    {
        // Arrange
        string schoolId = Guid.NewGuid().ToString();
        List<Classroom> expectedClassrooms = new()
        {
            new()
            {
                Id = Guid.NewGuid(),
                Name = "11E",
                SchoolId = Guid.Parse(schoolId),
                Teacher = new Teacher
                {
                    ApplicationUser = new ApplicationUser
                    {
                        Id = Guid.NewGuid(),
                        UserName = "test_teacher@test.com",
                        NormalizedUserName = "TEST_TEACHER@TEST.COM",
                        Email = "test_teacher@test.com",
                        NormalizedEmail = "TEST_TEACHER@TEST.COM",
                        FirstName = "Brendan",
                        LastName = "Osborne",
                        BirthDate = DateTime.ParseExact("1990-01-01", "yyyy-MM-dd", CultureInfo.InvariantCulture),
                        Address = "Test Address",
                        IsActive = true
                    }
                },
                Students = new List<Student>
                {
                    new()
                    {
                        ApplicationUser = new ApplicationUser
                        {
                            Id = Guid.NewGuid(),
                            UserName = "test_student1@test.com",
                            NormalizedUserName = "TEST_STUDENT1@TEST.COM",
                            Email = "test_student1@test.com",
                            NormalizedEmail = "TEST_STUDENT1@TEST.COM",
                            FirstName = "Random",
                            LastName = "Student",
                            BirthDate = DateTime.ParseExact("2000-01-01", "yyyy-MM-dd", CultureInfo.InvariantCulture),
                            Address = "Random Address",
                            IsActive = true
                        }
                    },
                    new()
                    {
                        ApplicationUser = new ApplicationUser
                        {
                            Id = Guid.NewGuid(),
                            UserName = "test_student2@test.com",
                            NormalizedUserName = "TEST_STUDENT2@TEST.COM",
                            Email = "test_student2@test.com",
                            NormalizedEmail = "TEST_STUDENT2@TEST.COM",
                            FirstName = "Test",
                            LastName = "Student",
                            BirthDate = DateTime.ParseExact("1999-01-01", "yyyy-MM-dd", CultureInfo.InvariantCulture),
                            Address = "Random Address",
                            IsActive = true
                        }
                    }
                }
            },
            new()
            {
                Id = Guid.NewGuid(),
                Name = "12A",
                SchoolId = Guid.Parse(schoolId),
                Teacher = new Teacher
                {
                    ApplicationUser = new ApplicationUser
                    {
                        Id = Guid.NewGuid(),
                        UserName = "test_teacher2@test.com",
                        NormalizedUserName = "TEST_TEACHER2@TEST.COM",
                        Email = "test_teacher2@test.com",
                        NormalizedEmail = "TEST_TEACHER2@TEST.COM",
                        FirstName = "Billy",
                        LastName = "Osborne",
                        BirthDate = DateTime.ParseExact("1995-01-01", "yyyy-MM-dd", CultureInfo.InvariantCulture),
                        Address = "Test Address",
                        IsActive = true
                    }
                },
                Students = new List<Student>
                {
                    new()
                    {
                        ApplicationUser = new ApplicationUser
                        {
                            Id = Guid.NewGuid(),
                            UserName = "test_student3@test.com",
                            NormalizedUserName = "TEST_STUDENT3@TEST.COM",
                            Email = "test_student3@test.com",
                            NormalizedEmail = "TEST_STUDENT3@TEST.COM",
                            FirstName = "AnotherTest",
                            LastName = "Student",
                            BirthDate = DateTime.ParseExact("2002-01-01", "yyyy-MM-dd", CultureInfo.InvariantCulture),
                            Address = "Random Address",
                            IsActive = true
                        }
                    }
                }
            }
        };

        await _dbContextMock.Classrooms.AddRangeAsync(expectedClassrooms);
        await _dbContextMock.SaveChangesAsync();

        // Act
        IEnumerable<ClassroomDetailsViewModel> result = await _sut.AllAsync(schoolId);

        // Assert
        Assert.NotNull(result);
        List<ClassroomDetailsViewModel> resultViewModels = result.ToList();
        Assert.Equal(expectedClassrooms.Count, resultViewModels.Count);

        foreach (Classroom expectedClassroom in expectedClassrooms)
        {
            ClassroomDetailsViewModel? actual = resultViewModels.FirstOrDefault(c => c.Id == expectedClassroom.Id.ToString());
            Assert.NotNull(actual);
            Assert.Equal(expectedClassroom.Name, actual!.Name);
            Assert.Equal(expectedClassroom.Teacher.ApplicationUser.GetFullName(), actual.TeacherName);
            Assert.Equal(expectedClassroom.IsActive, actual.IsActive);
            Assert.Equal(expectedClassroom.Students.Count, actual.Students.Count);

            foreach (Student expectedStudent in expectedClassroom.Students)
            {
                BasicViewModel? actualStudent = actual.Students.FirstOrDefault(s => s.Id == expectedStudent.Id.ToString());
                Assert.NotNull(actualStudent);
                Assert.Equal(expectedStudent.ApplicationUser.GetFullName(), actualStudent!.Name);
            }
        }
    }

    [Fact]
    public async Task ClassroomExistsInSchoolAsync_ReturnsTrueWhenClassroomExistsInSchool()
    {
        // Arrange
        string schoolId = Guid.NewGuid().ToString();
        string classroomName = "11E";
        Classroom classroom = new() { Id = Guid.NewGuid(), Name = classroomName, SchoolId = Guid.Parse(schoolId) };

        await _dbContextMock.Classrooms.AddAsync(classroom);
        await _dbContextMock.SaveChangesAsync();

        // Act
        bool result = await _sut.ClassroomExistsInSchoolAsync(classroomName, schoolId);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public async Task ClassroomExistsInSchoolAsync_ReturnsFalseWhenClassroomDoesNotExistInSchool()
    {
        // Arrange
        string schoolId = Guid.NewGuid().ToString();
        string classroomName = "0A";

        // Act
        bool result = await _sut.ClassroomExistsInSchoolAsync(classroomName, schoolId);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public async Task AddAsync_ReturnsFalseWhenSchoolNotFound()
    {
        // Arrange
        AddClassroomViewModel model = new()
        {
            Name = "11E",
            SchoolId = Guid.NewGuid().ToString(),
            TeacherId = Guid.NewGuid().ToString()
        };

        // Act
        bool result = await _sut.AddAsync(model);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public async Task AddAsync_ReturnsTrueWhenClassroomSuccessfullyAdded()
    {
        // Arrange
        School existingSchool = new()
        {
            Id = Guid.NewGuid(),
            Name = "Test School",
            Address = "Test Address",
            Email = "test@test.com",
            PhoneNumber = "0888888888",
            IsActive = true
        };

        await _dbContextMock.Schools.AddAsync(existingSchool);
        await _dbContextMock.SaveChangesAsync();

        AddClassroomViewModel model = new()
        {
            Name = "11E",
            SchoolId = existingSchool.Id.ToString(),
            TeacherId = Guid.NewGuid().ToString() // Ensure a valid teacher ID
        };

        // Act
        bool result = await _sut.AddAsync(model);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public async Task GetForEditAsync_ReturnsNullWhenClassroomDoesNotExist()
    {
        // Act
        AddClassroomViewModel? result = await _sut.GetForEditAsync(Guid.NewGuid().ToString());

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task GetForEditAsync_ReturnsViewModelWhenClassroomExists()
    {
        // Arrange
        Guid classroomId = Guid.NewGuid();
        string classroomName = "11E";
        Guid teacherId = Guid.NewGuid();
        Guid schoolId = Guid.NewGuid();

        Classroom classroom = new()
        {
            Id = classroomId,
            Name = classroomName,
            TeacherId = teacherId,
            SchoolId = schoolId,
            IsActive = true
        };

        await _dbContextMock.Classrooms.AddAsync(classroom);
        await _dbContextMock.SaveChangesAsync();

        // Act
        AddClassroomViewModel? result = await _sut.GetForEditAsync(classroomId.ToString());

        // Assert
        Assert.NotNull(result);
        Assert.Equal(classroomId, result!.Id);
        Assert.Equal(classroomName, result.Name);
        Assert.Equal(teacherId.ToString(), result.TeacherId);
        Assert.Equal(schoolId.ToString(), result.SchoolId);
    }

    public void Dispose()
    {
        _dbContextMock.Dispose();
        GC.SuppressFinalize(this);
    }
}