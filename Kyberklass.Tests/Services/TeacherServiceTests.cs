namespace Kyberklass.Tests.Services;

using System.Globalization;
using KyberKlass.Data;
using KyberKlass.Data.Models;
using KyberKlass.Services.Data;
using KyberKlass.Services.Data.Interfaces;
using KyberKlass.Web.ViewModels.Admin;
using KyberKlass.Web.ViewModels.Admin.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Moq;

public class TeacherServiceTests : IDisposable
{
	private readonly DbContextOptions<KyberKlassDbContext> _options;
	private readonly KyberKlassDbContext _dbContextMock;
	private readonly Mock<UserManager<ApplicationUser>> _userManagerMock;
	private readonly ITeacherService _sut;

	public TeacherServiceTests()
	{
		this._options = new DbContextOptionsBuilder<KyberKlassDbContext>()
			.UseInMemoryDatabase(Guid.NewGuid().ToString())
			.Options;

		this._dbContextMock = new KyberKlassDbContext(this._options);

		this._userManagerMock = new Mock<UserManager<ApplicationUser>>(Mock.Of<IUserStore<ApplicationUser>>(), null, null, null, null, null, null, null, null);

		this._sut = new TeacherService(this._dbContextMock, this._userManagerMock.Object);
	}

	[Fact]
	public async Task IsTeacherAssignedToClassroomAsync_ReturnsTrueWhenTeacherIsAssigned()
	{
		// Arrange
		var teacherId = Guid.NewGuid();
		var classroomId = Guid.NewGuid();
		var teacherUser = new ApplicationUser
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

		var teacher = new Teacher
		{
			Id = teacherId,
			ApplicationUser = teacherUser
		};

		var classroom = new Classroom { Id = classroomId, Name = "11E", TeacherId = teacherId, Teacher = teacher };

		await this._dbContextMock.Users.AddAsync(teacherUser);
		await this._dbContextMock.Classrooms.AddAsync(classroom);
		await this._dbContextMock.SaveChangesAsync();

		// Act
		bool result = await this._sut.IsTeacherAssignedToClassroomAsync(teacherId.ToString());

		// Assert
		Assert.True(result);
	}

	[Fact]
	public async Task IsTeacherAssignedToClassroomAsync_ReturnsFalseWhenTeacherIsNotAssigned()
	{
		// Arrange
		var teacher1Id = Guid.NewGuid();
		var teacherUser1 = new ApplicationUser
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

		var teacher2Id = Guid.NewGuid();
		var teacherUser2 = new ApplicationUser
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

		var teacher2 = new Teacher
		{
			Id = teacher2Id,
			ApplicationUser = teacherUser2
		};

		var classroomId = Guid.NewGuid();
		var classroom = new Classroom { Id = classroomId, Name = "11E", TeacherId = teacher2Id, Teacher = teacher2 };

		await this._dbContextMock.Users.AddRangeAsync(teacherUser1, teacherUser2);
		await this._dbContextMock.Classrooms.AddAsync(classroom);
		await this._dbContextMock.SaveChangesAsync();

		// Act
		bool result = await this._sut.IsTeacherAssignedToClassroomAsync(teacher1Id.ToString());

		// Assert
		Assert.False(result);
	}

	[Fact]
	public async Task GetUnassignedTeachersAsync_ReturnsEmptyListWhenNoTeachersExist()
	{
		// Arrange
		var userManagerMock = new Mock<UserManager<ApplicationUser>>(Mock.Of<IUserStore<ApplicationUser>>(), null, null, null, null, null, null, null, null);
		userManagerMock.Setup(m => m.GetUsersInRoleAsync("Teacher"))
			.ReturnsAsync(new List<ApplicationUser>());

		var sut = new TeacherService(this._dbContextMock, userManagerMock.Object);

		// Act
		IEnumerable<BasicViewModel> result = await sut.GetUnassignedTeachersAsync();

		// Assert
		Assert.Empty(result);
	}

	[Fact]
	public async Task GetUnassignedTeachersAsync_ReturnsUnassignedTeachers()
	{
		// Arrange
		var teacher1Id = Guid.NewGuid();
		var teacherUser1 = new ApplicationUser
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

		var teacher2Id = Guid.NewGuid();
		var teacherUser2 = new ApplicationUser
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

		var teacher3Id = Guid.NewGuid();
		var teacherUser3 = new ApplicationUser
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

		var teacher1 = new Teacher
		{
			Id = teacher1Id,
			ApplicationUser = teacherUser1
		};

		var teacher2 = new Teacher
		{
			Id = teacher2Id,
			ApplicationUser = teacherUser2
		};

		var teacher3 = new Teacher
		{
			Id = teacher3Id,
			ApplicationUser = teacherUser3
		};

		var classroom1 = new Classroom { Id = Guid.NewGuid(), Name = "11E", TeacherId = teacher1Id, Teacher = teacher1 };
		var classroom2 = new Classroom { Id = Guid.NewGuid(), Name = "12A", TeacherId = teacher2Id, Teacher = teacher2 };

		await this._dbContextMock.Users.AddRangeAsync(new List<ApplicationUser> { teacherUser1, teacherUser2, teacherUser3 });
		await this._dbContextMock.Teachers.AddRangeAsync(new List<Teacher> { teacher1, teacher2, teacher3 });
		await this._dbContextMock.Classrooms.AddRangeAsync(new List<Classroom> { classroom1, classroom2 });
		await this._dbContextMock.SaveChangesAsync();

		this._userManagerMock.Setup(m => m.GetUsersInRoleAsync("Teacher"))
			.ReturnsAsync(new List<ApplicationUser> { teacherUser1, teacherUser2, teacherUser3 });

		// Act
		IEnumerable<BasicViewModel> result = await this._sut.GetUnassignedTeachersAsync();

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
		var teacher1Id = Guid.NewGuid();
		var teacherUser1 = new ApplicationUser
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

		var teacher2Id = Guid.NewGuid();
		var teacherUser2 = new ApplicationUser
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

		var teacher1 = new Teacher
		{
			Id = teacher1Id,
			ApplicationUser = teacherUser1
		};

		var teacher2 = new Teacher
		{
			Id = teacher2Id,
			ApplicationUser = teacherUser2
		};

		var classroom1 = new Classroom { Id = Guid.NewGuid(), Name = "11E", TeacherId = teacher1Id, Teacher = teacher1 };
		var classroom2 = new Classroom { Id = Guid.NewGuid(), Name = "12A", TeacherId = teacher2Id, Teacher = teacher2 };

		await this._dbContextMock.Users.AddRangeAsync(new List<ApplicationUser> { teacherUser1, teacherUser2 });
		await this._dbContextMock.Teachers.AddRangeAsync(new List<Teacher> { teacher1, teacher2 });
		await this._dbContextMock.Classrooms.AddRangeAsync(new List<Classroom> { classroom1, classroom2 });
		await this._dbContextMock.SaveChangesAsync();

		this._userManagerMock.Setup(m => m.GetUsersInRoleAsync("Teacher"))
			.ReturnsAsync(new List<ApplicationUser> { teacherUser1, teacherUser2 });

		// Act
		IEnumerable<BasicViewModel> result = await this._sut.GetUnassignedTeachersAsync();

		// Assert
		Assert.Empty(result);
	}

	[Fact]
	public async Task AllAsync_ReturnsEmptyList_WhenNoTeachersExist()
	{
		// Arrange
		await this._dbContextMock.Roles.AddAsync(new IdentityRole<Guid> { Id = Guid.NewGuid(), Name = "Teacher" });
		await this._dbContextMock.SaveChangesAsync();

		// Act
		List<UserViewModel>? result = await this._sut.AllAsync();

		// Assert
		Assert.Empty(result!);
	}

	[Fact]
	public async Task AllAsync_ReturnsListOfTeachersWhenTeachersExist()
	{
		// Arrange
		var teacherRoleId = Guid.NewGuid();
		await this._dbContextMock.Roles.AddAsync(new IdentityRole<Guid> { Id = teacherRoleId, Name = "Teacher" });
		await this._dbContextMock.SaveChangesAsync();

		var teacher1Id = Guid.NewGuid();
		var teacher2Id = Guid.NewGuid();
		var teachers = new List<ApplicationUser>
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
			},
		};

		await this._dbContextMock.Users.AddRangeAsync(teachers);
		await this._dbContextMock.SaveChangesAsync();

		foreach (var teacher in teachers)
		{
			await this._dbContextMock.UserRoles.AddAsync(new IdentityUserRole<Guid> { UserId = teacher.Id, RoleId = teacherRoleId });
		}

		await this._dbContextMock.SaveChangesAsync();

		// Act
		List<UserViewModel>? result = await this._sut.AllAsync();

		// Assert
		Assert.Equal(teachers.Count, result!.Count);

		foreach (var teacher in teachers)
		{
			var teacherViewModel = result.FirstOrDefault(t => t.Id == teacher.Id.ToString());
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
		var result = await this._sut.AllAsync();

		// Assert
		Assert.Null(result);
	}

	public async void Dispose()
	{
		await this._dbContextMock.DisposeAsync();
	}
}