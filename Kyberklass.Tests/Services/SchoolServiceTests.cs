using KyberKlass.Data;
using KyberKlass.Data.Models;
using KyberKlass.Services.Data;
using KyberKlass.Services.Data.Interfaces;
using KyberKlass.Web.ViewModels.Admin;
using KyberKlass.Web.ViewModels.Admin.School;
using Microsoft.EntityFrameworkCore;

namespace KyberKlass.Tests.Services;
public class SchoolServiceTests : IDisposable
{
    private readonly DbContextOptions<KyberKlassDbContext> _options;
    private readonly KyberKlassDbContext _dbContextMock;
    private readonly ISchoolService _sut;

    public SchoolServiceTests()
    {
        _options = new DbContextOptionsBuilder<KyberKlassDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        _dbContextMock = new KyberKlassDbContext(_options);

        _sut = new SchoolService(_dbContextMock);
    }

    [Fact]
    public async Task AddAsync_ShouldSetProperties_Correctly()
    {
        // Arrange
        AddSchoolFormModel model = new()
        {
            Name = "Test School",
            Address = "Test Address",
            Email = "test@test.com",
            PhoneNumber = "0888888888"
        };

        // Expected School Count => Should not change because we check only if properties are set correctly and not the addition
        int expectedSchoolsCount = await _dbContextMock.Schools.CountAsync();

        // Act
        bool result = await _sut.AddAsync(model);

        // Actual School Count => Should not change because we check only if properties are set correctly and not the addition
        int actualSchoolsCount = await _dbContextMock.Schools.CountAsync();

        // Assert
        Assert.True(result);

        // Verify that the school is added to the database
        School? addedSchool = await _dbContextMock.Schools.FirstOrDefaultAsync(s => s.Name == model.Name);
        Assert.NotNull(addedSchool);

        // Verify that the properties are set correctly
        Assert.Equal(model.Name, addedSchool!.Name);
        Assert.Equal(model.Address, addedSchool.Address);
        Assert.Equal(model.Email, addedSchool.Email);
        Assert.Equal(model.PhoneNumber, addedSchool.PhoneNumber);
        Assert.Equal(model.IsActive, addedSchool.IsActive);
        Assert.Equal(expectedSchoolsCount + 1, actualSchoolsCount);
    }

    [Fact]
    public async Task AddAsync_ShouldReturnFalseIfSchoolAlreadyExists()
    {
        // Arrange
        AddSchoolFormModel model = new()
        {
            Name = "Existing School",
            Address = "Existing Address",
            Email = "existing@test.com",
            PhoneNumber = "0888888888"
        };

        await _sut.AddAsync(model); // Adding the school once

        int expectedSchoolsCount = await _dbContextMock.Schools.CountAsync();

        // Act
        // Adding the same school again
        bool result = await _sut.AddAsync(model);

        int actualSchoolsCount = await _dbContextMock.Schools.CountAsync();

        // Assert
        Assert.False(result);
        Assert.Equal(expectedSchoolsCount, actualSchoolsCount);
    }

    [Fact]
    public async Task BasicAllAsync_ReturnsCorrectViewModel()
    {
        Guid schoolId1 = Guid.NewGuid();
        Guid schoolId2 = Guid.NewGuid();
        Guid schoolId3 = Guid.NewGuid();
        Guid schoolId4 = Guid.NewGuid();
        Guid schoolId5 = Guid.NewGuid();

        await _dbContextMock.Schools.AddRangeAsync(
            new School
            {
                Id = schoolId1,
                Name = "11E",
                Address = "Random Address",
                Email = "school1@school.com",
                PhoneNumber = "0888888888",
                IsActive = true
            },
            new School
            {
                Id = schoolId2,
                Name = "12A",
                Address = "Random Address2",
                Email = "school2@school.com",
                PhoneNumber = "0888888888",
                IsActive = true
            },
            new School
            {
                Id = schoolId3,
                Name = "10A",
                Address = "Random Address3",
                Email = "school3@school.com",
                PhoneNumber = "0888888888",
                IsActive = true
            },
            new School
            {
                Id = schoolId4,
                Name = "9A",
                Address = "Random Address4",
                Email = "school4@school.com",
                PhoneNumber = "0888888888",
                IsActive = true
            }
            ,
            new School
            {
                Id = schoolId5,
                Name = "8E",
                Address = "Random Address5",
                Email = "school5@school.com",
                PhoneNumber = "0888888888",
                IsActive = true
            });

        await _dbContextMock.SaveChangesAsync();

        List<BasicViewModel> expectedResultViewModels = new()
        {
            new()
            {
                Id = schoolId1.ToString(),
                Name = "11E"
            },
            new()
            {
                Id = schoolId2.ToString(),
                Name = "12A"
            },
            new()
            {
                Id = schoolId3.ToString(),
                Name = "10A"
            },
            new()
            {
                Id = schoolId4.ToString(),
                Name = "9A"
            },
            new()
            {
                Id = schoolId5.ToString(),
                Name = "8E"
            }
        };

        // Act
        IEnumerable<BasicViewModel> result = await _sut.BasicAllAsync();
        List<BasicViewModel> resultViewModels = result.ToList();

        Assert.NotNull(result);
        Assert.Equal(expectedResultViewModels.Count, resultViewModels.Count);

        for (int i = 0; i < expectedResultViewModels.Count; i++)
        {
            Assert.Equal(expectedResultViewModels[i].Id, resultViewModels[i].Id);
            Assert.Equal(expectedResultViewModels[i].Name, resultViewModels[i].Name);
        }
    }

    [Fact]
    public async Task GetForEditAsync_ReturnsCorrectSchool()
    {
        // Arrange
        Guid schoolId = Guid.NewGuid();
        School expectedSchool = new()
        {
            Id = schoolId,
            Name = "Test School",
            Address = "Test Address",
            Email = "test@test.com",
            PhoneNumber = "1234567890",
            IsActive = true
        };

        await _dbContextMock.Schools.AddAsync(expectedSchool);
        await _dbContextMock.SaveChangesAsync();

        // Act
        AddSchoolFormModel? result = await _sut.GetForEditAsync(schoolId.ToString());

        // Assert
        Assert.NotNull(result);
        Assert.Equal(expectedSchool.Name, result!.Name);
        Assert.Equal(expectedSchool.Address, result.Address);
        Assert.Equal(expectedSchool.Email, result.Email);
        Assert.Equal(expectedSchool.PhoneNumber, result.PhoneNumber);
        Assert.Equal(expectedSchool.IsActive, result.IsActive);
    }

    [Fact]
    public async Task GetForEditAsync_ReturnsNullForNonexistentSchool()
    {
        // Arrange
        string invalidSchoolId = Guid.NewGuid().ToString();

        // Act
        AddSchoolFormModel? result = await _sut.GetForEditAsync(invalidSchoolId);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task EditAsync_ReturnsTrueAndUpdatesSchool()
    {
        // Arrange
        Guid schoolId = Guid.NewGuid();
        School initialSchool = new()
        {
            Id = schoolId,
            Name = "Initial School Name",
            Address = "Initial Address",
            Email = "initial@test.com",
            PhoneNumber = "0999999999",
            IsActive = true
        };

        await _dbContextMock.Schools.AddAsync(initialSchool);
        await _dbContextMock.SaveChangesAsync();

        AddSchoolFormModel editedModel = new()
        {
            Name = "Edited School Name",
            Address = "Edited Address",
            Email = "edited@test.com",
            PhoneNumber = "0888888888",
            IsActive = false
        };

        // Act
        bool result = await _sut.EditAsync(schoolId.ToString(), editedModel);
        School? updatedSchool = await _dbContextMock.Schools.FindAsync(schoolId);

        // Assert
        Assert.True(result);
        Assert.NotNull(updatedSchool);
        Assert.Equal(editedModel.Name, updatedSchool!.Name);
        Assert.Equal(editedModel.Address, updatedSchool.Address);
        Assert.Equal(editedModel.Email, updatedSchool.Email);
        Assert.Equal(editedModel.PhoneNumber, updatedSchool.PhoneNumber);
        Assert.Equal(editedModel.IsActive, updatedSchool.IsActive);
    }

    [Fact]
    public async Task EditAsync_ReturnsFalseForNonexistentSchool()
    {
        // Arrange
        string invalidSchoolId = Guid.NewGuid().ToString();
        AddSchoolFormModel editedModel = new()
        {
            Name = "Edited School Name",
            Address = "Edited Address",
            Email = "edited@test.com",
            PhoneNumber = "0888888888",
            IsActive = false
        };

        // Act
        bool result = await _sut.EditAsync(invalidSchoolId, editedModel);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public async Task GetForDeleteAsync_ReturnsSchoolDetailsForDeletion()
    {
        // Arrange
        Guid schoolId = Guid.NewGuid();
        SchoolDetailsViewModel expectedViewModel = new()
        {
            Id = schoolId.ToString(),
            Name = "Test School",
            Address = "Test Address",
            Email = "test@test.com",
            PhoneNumber = "0888888888",
            IsActive = true
        };

        await _dbContextMock.Schools.AddAsync(new School
        {
            Id = schoolId,
            Name = expectedViewModel.Name,
            Address = expectedViewModel.Address,
            Email = expectedViewModel.Email,
            PhoneNumber = expectedViewModel.PhoneNumber,
            IsActive = expectedViewModel.IsActive
        });

        await _dbContextMock.SaveChangesAsync();

        // Act
        SchoolDetailsViewModel? result = await _sut.GetForDeleteAsync(schoolId.ToString());

        // Assert
        Assert.NotNull(result);
        Assert.Equal(expectedViewModel.Id, result!.Id);
        Assert.Equal(expectedViewModel.Name, result.Name);
        Assert.Equal(expectedViewModel.Address, result.Address);
        Assert.Equal(expectedViewModel.Email, result.Email);
        Assert.Equal(expectedViewModel.PhoneNumber, result.PhoneNumber);
        Assert.Equal(expectedViewModel.IsActive, result.IsActive);
    }

    [Fact]
    public async Task GetForDeleteAsync_ReturnsNullForNonexistentSchool()
    {
        // Arrange
        string invalidSchoolId = Guid.NewGuid().ToString();

        // Act
        SchoolDetailsViewModel? result = await _sut.GetForDeleteAsync(invalidSchoolId);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task DeleteAsync_DeletesSchoolWhenNoStudentsOrClassrooms()
    {
        // Arrange
        Guid schoolId = Guid.NewGuid();
        School school = new()
        {
            Id = schoolId,
            Name = "Test School",
            Address = "Test Address",
            Email = "test@test.com",
            PhoneNumber = "0888888888",
            IsActive = true
        };

        await _dbContextMock.Schools.AddAsync(school);
        await _dbContextMock.SaveChangesAsync();

        // Act
        bool result = await _sut.DeleteAsync(schoolId.ToString());
        School? deletedSchool = await _dbContextMock.Schools.FindAsync(schoolId);

        // Assert
        Assert.True(result);
        Assert.Null(deletedSchool);
    }

    [Fact]
    public async Task DeleteAsync_DoesNotDeleteSchoolWithStudentsOrClassrooms()
    {
        // Arrange
        Guid schoolId = Guid.NewGuid();
        School school = new()
        {
            Id = schoolId,
            Name = "Test School",
            Address = "Test Address",
            Email = "test@test.com",
            PhoneNumber = "0888888888",
            IsActive = true
        };

        Guid classroomId = Guid.NewGuid();
        school.Classrooms.Add(new Classroom
        {
            Id = classroomId,
            Name = "11E",
            TeacherId = Guid.NewGuid(),
            IsActive = true,
            SchoolId = schoolId
        });

        school.Students.Add(new Student
        {
            Id = Guid.NewGuid(),
            GuardianId = Guid.NewGuid(),
            SchoolId = schoolId,
            ClassroomId = classroomId
        });

        await _dbContextMock.Schools.AddAsync(school);
        await _dbContextMock.SaveChangesAsync();

        // Act
        bool result = await _sut.DeleteAsync(schoolId.ToString());
        School? deletedSchool = await _dbContextMock.Schools.FindAsync(schoolId);

        // Assert
        Assert.False(result);
        Assert.NotNull(deletedSchool);
    }

    [Fact]
    public async Task HasStudentsAssignedAsync_ReturnsTrueWhenStudentsAssigned()
    {
        // Arrange
        Guid schoolId = Guid.NewGuid();
        School school = new()
        {
            Id = schoolId,
            Name = "Test School",
            Address = "Test Address",
            Email = "test@test.com",
            PhoneNumber = "0888888888",
            IsActive = true
        };

        Classroom classroom1 = new()
        {
            Id = Guid.NewGuid(),
            Name = "11E",
            TeacherId = Guid.NewGuid(),
            IsActive = true,
            SchoolId = schoolId,
            School = school
        };

        Classroom classroom2 = new()
        {
            Id = Guid.NewGuid(),
            Name = "12A",
            TeacherId = Guid.NewGuid(),
            IsActive = true,
            SchoolId = schoolId,
            School = school
        };

        Student student1 = new()
        {
            Id = Guid.NewGuid(),
            GuardianId = Guid.NewGuid(),
            SchoolId = schoolId,
            School = school,
            ClassroomId = classroom1.Id
        };

        Student student2 = new()
        {
            Id = Guid.NewGuid(),
            GuardianId = Guid.NewGuid(),
            SchoolId = schoolId,
            School = school,
            ClassroomId = classroom2.Id
        };

        // Add classrooms and students to the school
        school.Students = new List<Student> { student1, student2 };
        school.Classrooms = new List<Classroom> { classroom1, classroom2 };

        await _dbContextMock.Schools.AddAsync(school);
        await _dbContextMock.SaveChangesAsync();

        // Act
        bool result = await _sut.HasStudentsAssignedAsync(schoolId.ToString());

        // Assert
        Assert.True(result);
        Assert.NotNull(school.Students);
        Assert.NotEmpty(school.Students);
        Assert.Equal(2, school.Students.Count);
    }

    [Fact]
    public async Task HasStudentsAssignedAsync_ReturnsFalseWhenSchoolNotFound()
    {
        // Arrange
        string invalidSchoolId = Guid.NewGuid().ToString();

        // Act
        bool result = await _sut.HasStudentsAssignedAsync(invalidSchoolId);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public async Task HasStudentsAssignedAsync_ReturnsFalseWhenNoStudentsAssigned()
    {
        // Arrange
        Guid schoolId = Guid.NewGuid();
        School school = new()
        {
            Id = schoolId,
            Name = "Test School",
            Address = "Test Address",
            Email = "test@test.com",
            PhoneNumber = "0888888888",
            IsActive = true
        };

        // Add school to database
        await _dbContextMock.Schools.AddAsync(school);
        await _dbContextMock.SaveChangesAsync();

        // Act
        bool result = await _sut.HasStudentsAssignedAsync(schoolId.ToString());

        // Assert
        Assert.False(result);
    }

    [Fact]
    public async Task GetByIdAsync_ReturnsSchoolDetailsViewModelWhenSchoolExists()
    {
        // Arrange
        Guid schoolId = Guid.NewGuid();
        SchoolDetailsViewModel expectedViewModel = new()
        {
            Id = schoolId.ToString(),
            Name = "Test School",
            Address = "Test Address",
            Email = "test@test.com",
            PhoneNumber = "0888888888",
            IsActive = true
        };

        School school = new()
        {
            Id = schoolId,
            Name = "Test School",
            Address = "Test Address",
            Email = "test@test.com",
            PhoneNumber = "0888888888",
            IsActive = true
        };

        await _dbContextMock.Schools.AddAsync(school);
        await _dbContextMock.SaveChangesAsync();

        // Act
        SchoolDetailsViewModel? result = await _sut.GetByIdAsync(schoolId.ToString());

        // Assert
        Assert.NotNull(result);
        Assert.Equal(expectedViewModel.Id, result!.Id);
        Assert.Equal(expectedViewModel.Name, result.Name);
        Assert.Equal(expectedViewModel.Address, result.Address);
        Assert.Equal(expectedViewModel.Email, result.Email);
        Assert.Equal(expectedViewModel.PhoneNumber, result.PhoneNumber);
        Assert.Equal(expectedViewModel.IsActive, result.IsActive);
    }

    [Fact]
    public async Task GetByIdAsync_ReturnsNullWhenSchoolDoesNotExist()
    {
        // Arrange
        string invalidSchoolId = Guid.NewGuid().ToString();

        // Act
        SchoolDetailsViewModel? result = await _sut.GetByIdAsync(invalidSchoolId);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task ClassroomExistsInSchoolAsync_ReturnsTrueWhenClassroomExistsInSchool()
    {
        // Arrange
        string schoolId = Guid.NewGuid().ToString();
        string classroomId = Guid.NewGuid().ToString();

        School school = new()
        {
            Id = Guid.Parse(schoolId),
            Name = "Test School",
            Address = "Test Address",
            Email = "test@test.com",
            PhoneNumber = "0888888888",
            IsActive = true,
            Classrooms = new List<Classroom>
            {
                new()
                {
                    Id = Guid.Parse(classroomId),
                    Name = "11E",
                    TeacherId = Guid.NewGuid(),
                    IsActive = true,
                    SchoolId = Guid.Parse(schoolId)
                }
            }
        };

        await _dbContextMock.Schools.AddAsync(school);
        await _dbContextMock.SaveChangesAsync();

        // Act
        bool result = await _sut.ClassroomExistsInSchoolAsync(schoolId, classroomId);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public async Task ClassroomExistsInSchoolAsync_ReturnsFalseWhenClassroomDoesNotExistInSchool()
    {
        // Arrange
        string schoolId = Guid.NewGuid().ToString();
        string classroomId = Guid.NewGuid().ToString();

        School school = new()
        {
            Id = Guid.Parse(schoolId),
            Name = "Test School",
            Address = "Test Address",
            Email = "test@test.com",
            PhoneNumber = "0888888888",
            IsActive = true,
            Classrooms = new List<Classroom>()
        };

        await _dbContextMock.Schools.AddAsync(school);
        await _dbContextMock.SaveChangesAsync();

        // Act
        bool result = await _sut.ClassroomExistsInSchoolAsync(schoolId, classroomId);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public async Task AllAsync_ReturnsAllSchools()
    {
        // Arrange
        Guid school1Id = Guid.NewGuid();
        Guid school2Id = Guid.NewGuid();

        School school1 = new()
        {
            Id = school1Id,
            Name = "School 1",
            Address = "Address 1",
            Email = "school1@test.com",
            PhoneNumber = "0888888888",
            IsActive = true,
            Classrooms = new List<Classroom>
            {
                new()
                {
                    Id = Guid.NewGuid(),
                    Name = "11E",
                    TeacherId = Guid.NewGuid(),
                    IsActive = true,
                    Students = new List<Student>
                    {
                        new()
                        {
                            Id = Guid.NewGuid(),
                            GuardianId = Guid.NewGuid(),
                            SchoolId = school1Id,
                            ClassroomId = Guid.NewGuid(),
                            ApplicationUser = new ApplicationUser
                            {
                                Id = Guid.NewGuid(),
                                FirstName = "Alice",
                                LastName = "Barry",
                                Address = "Random Address 10",
                                BirthDate = DateTime.Parse("2000-01-01"),
                                IsActive = true
                            }
                        }
                    }
                }
            }
        };

        School school2 = new()
        {
            Id = school2Id,
            Name = "School 2",
            Address = "Address 2",
            Email = "school2@test.com",
            PhoneNumber = "0999999999",
            IsActive = true
        };

        await _dbContextMock.Schools.AddRangeAsync(school1, school2);
        await _dbContextMock.SaveChangesAsync();

        // Act
        IEnumerable<SchoolDetailsViewModel> result = await _sut.AllAsync();

        // Assert
        Assert.NotNull(result);
        List<SchoolDetailsViewModel> resultViewModels = result.ToList();
        Assert.Equal(2, resultViewModels.Count);

        foreach (SchoolDetailsViewModel school in resultViewModels)
        {
            School? actualSchool = await _dbContextMock.Schools.FindAsync(Guid.Parse(school.Id));

            Assert.Equal(actualSchool!.Name, school.Name);
            Assert.Equal(actualSchool.Address, school.Address);
            Assert.Equal(actualSchool.Email, school.Email);
            Assert.Equal(actualSchool.PhoneNumber, school.PhoneNumber);
            Assert.Equal(actualSchool.IsActive, school.IsActive);

            int expectedClassroomCount = await _dbContextMock.Classrooms.CountAsync(c => c.SchoolId == actualSchool.Id);
            Assert.Equal(expectedClassroomCount, actualSchool.Classrooms.Count);

            foreach (Web.ViewModels.Admin.Classroom.ClassroomDetailsViewModel classroom in school.Classrooms)
            {
                Classroom? actualClassroom = await _dbContextMock.Classrooms.FindAsync(Guid.Parse(classroom.Id));

                Assert.Equal(actualClassroom!.Name, classroom.Name);
                Assert.Equal($"{actualClassroom.Teacher.ApplicationUser.FirstName} {actualClassroom.Teacher.ApplicationUser.LastName}", classroom.TeacherName);

                int expectedStudentCount = await _dbContextMock.Students.CountAsync(s => s.ClassroomId == actualClassroom.Id);
                Assert.Equal(expectedStudentCount, classroom.Students.Count);

                foreach (BasicViewModel student in classroom.Students)
                {
                    Student? actualStudent = await _dbContextMock.Students.FindAsync(Guid.Parse(student.Id));

                    Assert.Equal(actualStudent!.ApplicationUser.GetFullName(), student.Name);
                }
            }
        }
    }

    public async void Dispose()
    {
        await _dbContextMock.DisposeAsync();
        GC.SuppressFinalize(this);
    }
}