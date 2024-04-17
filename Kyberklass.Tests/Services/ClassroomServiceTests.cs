namespace Kyberklass.Tests.Services;

using System.Globalization;
using KyberKlass.Data;
using KyberKlass.Data.Models;
using KyberKlass.Services.Data;
using KyberKlass.Services.Data.Interfaces;
using KyberKlass.Web.ViewModels.Admin;
using KyberKlass.Web.ViewModels.Admin.Classroom;
using Microsoft.EntityFrameworkCore;

public class ClassroomServiceTests : IDisposable
{
	private readonly DbContextOptions<KyberKlassDbContext> _options;
	private readonly KyberKlassDbContext _dbContextMock;
	private readonly IClassroomService _sut;

	public ClassroomServiceTests()
	{
		this._options = new DbContextOptionsBuilder<KyberKlassDbContext>()
			.UseInMemoryDatabase(Guid.NewGuid().ToString())
			.Options;

		this._dbContextMock = new KyberKlassDbContext(this._options);

		this._sut = new ClassroomService(this._dbContextMock);
	}

	[Fact]
	public async Task HasStudentsAssignedAsync_ReturnsTrueWhenStudentsAssigned()
	{
		// Arrange
		var classroomId = Guid.NewGuid();
		var classroom = new Classroom { Id = classroomId, Name = "11E" };

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

		var student = new Student
		{
			Id = studentId,
			ApplicationUser = studentUser,
			GuardianId = Guid.NewGuid(),
			SchoolId = Guid.NewGuid(),
			ClassroomId = classroomId,
			Classroom = classroom
		};

		await this._dbContextMock.Classrooms.AddAsync(classroom);
		await this._dbContextMock.Students.AddAsync(student);
		await this._dbContextMock.SaveChangesAsync();

		// Act
		bool result = await this._sut.HasStudentsAssignedAsync(classroomId.ToString());

		// Assert
		Assert.True(result);
	}

	[Fact]
	public async Task HasStudentsAssignedAsync_ReturnsFalseWhenNoStudentsAssigned()
	{
		// Arrange
		var classroomId = Guid.NewGuid();
		var classroom = new Classroom { Id = classroomId, Name = "11E" };

		await this._dbContextMock.Classrooms.AddAsync(classroom);
		await this._dbContextMock.SaveChangesAsync();

		// Act
		bool result = await this._sut.HasStudentsAssignedAsync(classroomId.ToString());

		// Assert
		Assert.False(result);
	}


	[Fact]
	public async Task DeleteAsync_ReturnsTrueAndDeletesClassroomWhenNoStudentsAssigned()
	{
		// Arrange
		var classroomId = Guid.NewGuid();
		var classroom = new Classroom { Id = classroomId, Name = "11E" };

		await this._dbContextMock.Classrooms.AddAsync(classroom);
		await this._dbContextMock.SaveChangesAsync();

		// Act
		bool result = await this._sut.DeleteAsync(classroomId.ToString());

		// Assert
		Assert.True(result);
		Assert.Null(await this._dbContextMock.Classrooms.FindAsync(classroomId));
	}

	[Fact]
	public async Task DeleteAsync_ReturnsFalseAndDoesNotDeleteClassroom_WhenStudentsAssigned()
	{
		// Arrange
		var classroomId = Guid.NewGuid();
		var classroom = new Classroom { Id = classroomId, Name = "11E" };

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

		var student = new Student
		{
			Id = studentId,
			ApplicationUser = studentUser,
			GuardianId = Guid.NewGuid(),
			SchoolId = Guid.NewGuid(),
			ClassroomId = classroomId,
			Classroom = classroom
		};

		await this._dbContextMock.Classrooms.AddAsync(classroom);
		await this._dbContextMock.Students.AddAsync(student);
		await this._dbContextMock.SaveChangesAsync();

		// Act
		bool result = await this._sut.DeleteAsync(classroomId.ToString());

		// Assert
		Assert.False(result);
		Assert.NotNull(await this._dbContextMock.Classrooms.FindAsync(classroomId));
	}

	[Fact]
	public async Task GetForDeleteAsync_ReturnsNullWhenClassroomDoesNotExist()
	{
		// Act
		var result = await this._sut.GetForDeleteAsync(Guid.NewGuid().ToString());

		// Assert
		Assert.Null(result);
	}

	[Fact]
	public async Task GetForDeleteAsync_ReturnsClassroomDetailsViewModelWhenClassroomExists()
	{
		// Arrange
		var teacherId = Guid.NewGuid();
		var teacherUser = new ApplicationUser
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

		var teacher = new Teacher
		{
			Id = teacherId,
			ApplicationUser = teacherUser
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
			BirthDate = DateTime.ParseExact("2001-01-01", "yyyy-MM-dd", CultureInfo.InvariantCulture),
			Address = "Random Address 2",
			IsActive = true
		};

		var guardianId = Guid.NewGuid();
		var guardianUser = new ApplicationUser
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

		var guardian = new Guardian
		{
			Id = guardianId,
			ApplicationUser = guardianUser
		};

		var schoolId = Guid.NewGuid();
		var school = new School
		{
			Id = schoolId,
			Name = "Test School",
			Address = "Test Address",
			Email = "testemail@test.com",
			PhoneNumber = "08888888888",
			IsActive = true
		};

		var student = new Student
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

		var classroomId = Guid.NewGuid();
		var classroom = new Classroom
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

		await this._dbContextMock.Classrooms.AddAsync(classroom);
		await this._dbContextMock.SaveChangesAsync();

		// Act
		var result = await this._sut.GetForDeleteAsync(classroomId.ToString());

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
		var invalidModel = new AddClassroomViewModel { IsActive = true };

		// Act
		bool result = await this._sut.EditAsync(invalidId, invalidModel);

		// Assert
		Assert.False(result);
	}

	[Fact]
	public async Task EditAsync_ReturnsTrueWhenClassroomExistsAndIsSuccessfullyEdited()
	{
		// Arrange
		var classroomId = Guid.NewGuid();
		var classroom = new Classroom { Id = classroomId, Name = "11E", IsActive = true };

		await this._dbContextMock.Classrooms.AddAsync(classroom);
		await this._dbContextMock.SaveChangesAsync();

		var model = new AddClassroomViewModel { IsActive = false };

		// Act
		bool result = await this._sut.EditAsync(classroomId.ToString(), model);

		// Assert
		Assert.True(result);
	}

	[Fact]
	public async Task GetAllClassroomsBySchoolIdAsJsonAsync_ReturnsAllClassroomsForGivenSchoolId()
	{
		// Arrange
		string schoolId = Guid.NewGuid().ToString();
		var expectedClassrooms = new List<Classroom>
		{
			new()
				{ Id = Guid.NewGuid(), Name = "11E", SchoolId = Guid.Parse(schoolId) },
			new()
				{ Id = Guid.NewGuid(), Name = "12A", SchoolId = Guid.Parse(schoolId) },
			new()
				{ Id = Guid.NewGuid(), Name = "10A", SchoolId = Guid.Parse(schoolId) }
		};

		await this._dbContextMock.Classrooms.AddRangeAsync(expectedClassrooms);
		await this._dbContextMock.SaveChangesAsync();

		// Act
		IEnumerable<BasicViewModel> result = await this._sut.GetAllClassroomsBySchoolIdAsJsonAsync(schoolId);

		// Assert
		Assert.NotNull(result);
		List<BasicViewModel> resultViewModels = result.ToList();
		Assert.Equal(expectedClassrooms.Count, resultViewModels.Count);

		foreach (var expectedClassroom in expectedClassrooms)
		{
			var actual = resultViewModels.FirstOrDefault(c => c.Id == expectedClassroom.Id.ToString());
			Assert.NotNull(actual);
			Assert.Equal(expectedClassroom.Name, actual!.Name);
		}
	}

	[Fact]
	public async Task AllAsync_ReturnsAllClassroomsForGivenSchoolId()
	{
		// Arrange
		string schoolId = Guid.NewGuid().ToString();
		var expectedClassrooms = new List<Classroom>
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

		await this._dbContextMock.Classrooms.AddRangeAsync(expectedClassrooms);
		await this._dbContextMock.SaveChangesAsync();

		// Act
		IEnumerable<ClassroomDetailsViewModel> result = await this._sut.AllAsync(schoolId);

		// Assert
		Assert.NotNull(result);
		List<ClassroomDetailsViewModel> resultViewModels = result.ToList();
		Assert.Equal(expectedClassrooms.Count, resultViewModels.Count);

		foreach (var expectedClassroom in expectedClassrooms)
		{
			var actual = resultViewModels.FirstOrDefault(c => c.Id == expectedClassroom.Id.ToString());
			Assert.NotNull(actual);
			Assert.Equal(expectedClassroom.Name, actual!.Name);
			Assert.Equal(expectedClassroom.Teacher.ApplicationUser.GetFullName(), actual.TeacherName);
			Assert.Equal(expectedClassroom.IsActive, actual.IsActive);
			Assert.Equal(expectedClassroom.Students.Count, actual.Students.Count);

			foreach (var expectedStudent in expectedClassroom.Students)
			{
				var actualStudent = actual.Students.FirstOrDefault(s => s.Id == expectedStudent.Id.ToString());
				Assert.NotNull(actualStudent);
				Assert.Equal(expectedStudent.ApplicationUser.GetFullName(), actualStudent!.Name);
			}
		}
	}

	[Fact]
	public async Task ClassroomExistsInSchoolAsync_ReturnsTrueWhenClassroomExistsInSchool()
	{
		// Arrange
		var schoolId = Guid.NewGuid().ToString();
		var classroomName = "11E";
		var classroom = new Classroom { Id = Guid.NewGuid(), Name = classroomName, SchoolId = Guid.Parse(schoolId) };

		await this._dbContextMock.Classrooms.AddAsync(classroom);
		await this._dbContextMock.SaveChangesAsync();

		// Act
		var result = await this._sut.ClassroomExistsInSchoolAsync(classroomName, schoolId);

		// Assert
		Assert.True(result);
	}

	[Fact]
	public async Task ClassroomExistsInSchoolAsync_ReturnsFalseWhenClassroomDoesNotExistInSchool()
	{
		// Arrange
		var schoolId = Guid.NewGuid().ToString();
		var classroomName = "0A";

		// Act
		var result = await this._sut.ClassroomExistsInSchoolAsync(classroomName, schoolId);

		// Assert
		Assert.False(result);
	}

	[Fact]
	public async Task AddAsync_ReturnsFalseWhenSchoolNotFound()
	{
		// Arrange
		var model = new AddClassroomViewModel
		{
			Name = "11E",
			SchoolId = Guid.NewGuid().ToString(),
			TeacherId = Guid.NewGuid().ToString()
		};

		// Act
		var result = await this._sut.AddAsync(model);

		// Assert
		Assert.False(result);
	}

	[Fact]
	public async Task AddAsync_ReturnsTrueWhenClassroomSuccessfullyAdded()
	{
		// Arrange
		var existingSchool = new School
		{
			Id = Guid.NewGuid(),
			Name = "Test School",
			Address = "Test Address",
			Email = "test@test.com",
			PhoneNumber = "0888888888",
			IsActive = true
		};

		await this._dbContextMock.Schools.AddAsync(existingSchool);
		await this._dbContextMock.SaveChangesAsync();

		var model = new AddClassroomViewModel
		{
			Name = "11E",
			SchoolId = existingSchool.Id.ToString(),
			TeacherId = Guid.NewGuid().ToString() // Ensure a valid teacher ID
		};

		// Act
		var result = await this._sut.AddAsync(model);

		// Assert
		Assert.True(result);
	}

	[Fact]
	public async Task GetForEditAsync_ReturnsNullWhenClassroomDoesNotExist()
	{
		// Act
		var result = await this._sut.GetForEditAsync(Guid.NewGuid().ToString());

		// Assert
		Assert.Null(result);
	}

	[Fact]
	public async Task GetForEditAsync_ReturnsViewModelWhenClassroomExists()
	{
		// Arrange
		var classroomId = Guid.NewGuid();
		var classroomName = "11E";
		var teacherId = Guid.NewGuid();
		var schoolId = Guid.NewGuid();

		var classroom = new Classroom
		{
			Id = classroomId,
			Name = classroomName,
			TeacherId = teacherId,
			SchoolId = schoolId,
			IsActive = true
		};

		await this._dbContextMock.Classrooms.AddAsync(classroom);
		await this._dbContextMock.SaveChangesAsync();

		// Act
		var result = await this._sut.GetForEditAsync(classroomId.ToString());

		// Assert
		Assert.NotNull(result);
		Assert.Equal(classroomId, result!.Id);
		Assert.Equal(classroomName, result.Name);
		Assert.Equal(teacherId.ToString(), result.TeacherId);
		Assert.Equal(schoolId.ToString(), result.SchoolId);
	}

	public void Dispose()
	{
		this._dbContextMock.Dispose();
	}
}