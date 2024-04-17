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
using Web.ViewModels.Admin.User;

public class StudentServiceTests : IDisposable
{
	private readonly DbContextOptions<KyberKlassDbContext> _options;
	private readonly KyberKlassDbContext _dbContextMock;
	private readonly Mock<IUserService> _userServiceMock;
	private readonly Mock<IGuardianService> _guardianServiceMock;
	private readonly IStudentService _sut;

	public StudentServiceTests()
	{
		this._options = new DbContextOptionsBuilder<KyberKlassDbContext>()
			.UseInMemoryDatabase(Guid.NewGuid().ToString())
			.Options;

		this._dbContextMock = new KyberKlassDbContext(this._options);

		this._userServiceMock = new Mock<IUserService>();
		this._guardianServiceMock = new Mock<IGuardianService>();

		this._sut = new StudentService(this._dbContextMock, this._userServiceMock.Object, this._guardianServiceMock.Object);
	}

	[Fact]
	public async Task GetByIdAsync_ReturnsStudentWithCorrectId()
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
		var student = new Student
		{
			Id = studentId,
			GuardianId = guardianId,
			SchoolId = Guid.NewGuid(),
			ClassroomId = Guid.NewGuid(),
			ApplicationUser = new ApplicationUser
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

		var guardian = new Guardian
		{
			Id = guardianId,
			ApplicationUser = guardianUser,
			Students = new List<Student> { student }
		};

		student.Guardian = guardian;

		await this._dbContextMock.Students.AddAsync(student);
		await this._dbContextMock.SaveChangesAsync();

		// Act
		var result = await this._sut.GetByIdASync(studentId.ToString());

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
		var invalidId = Guid.NewGuid();

		// Act
		var result = await this._sut.GetByIdASync(invalidId.ToString());

		// Assert
		Assert.Null(result);
	}

	[Fact]
	public async Task StudentChangeGuardianAsync_BothExistChangesGuardian()
	{
		// Arrange
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

		var previousGuardianId = Guid.NewGuid();
		var previousGuardianUser = new ApplicationUser
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

		var newGuardianId = Guid.NewGuid();
		var newGuardianUser = new ApplicationUser
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

		var student = new Student
		{
			Id = studentId,
			ApplicationUser = studentUser,
			GuardianId = previousGuardianId,
			SchoolId = Guid.NewGuid(),
			ClassroomId = Guid.NewGuid()
		};

		var previousGuardian = new Guardian
		{
			Id = previousGuardianId,
			ApplicationUser = previousGuardianUser
		};

		var newGuardian = new Guardian
		{
			Id = newGuardianId,
			ApplicationUser = newGuardianUser
		};

		student.Guardian = previousGuardian;

		this._guardianServiceMock.Setup(m => m.GetByIdAsync(newGuardianId.ToString()))
			.ReturnsAsync(newGuardian);

		await this._dbContextMock.Students.AddAsync(student);
		await this._dbContextMock.Guardians.AddAsync(previousGuardian);
		await this._dbContextMock.Guardians.AddAsync(newGuardian);
		await this._dbContextMock.SaveChangesAsync();

		// Act
		bool result = await this._sut.StudentChangeGuardianAsync(studentId.ToString(), newGuardianId.ToString());

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
		bool result = await this._sut.StudentChangeGuardianAsync(invalidStudentId, guardianId);

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
		bool result = await this._sut.StudentChangeGuardianAsync(studentId, invalidGuardian);

		// Assert
		Assert.False(result);
	}

	[Fact]
	public async Task StudentChangeGuardianAsync_StudentHasSameGuardianReturnsFalse()
	{
		// Arrange
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
			SchoolId = Guid.NewGuid(),
			ClassroomId = Guid.NewGuid(),
			Guardian = guardian
		};

		guardian.Students = new List<Student> { student };


		this._guardianServiceMock.Setup(m => m.GetByIdAsync(guardianId.ToString()))
			.ReturnsAsync(guardian);

		await this._dbContextMock.Students.AddAsync(student);
		await this._dbContextMock.Guardians.AddAsync(guardian);
		await this._dbContextMock.SaveChangesAsync();

		// Act
		bool result = await this._sut.StudentChangeGuardianAsync(studentId.ToString(), guardianId.ToString());

		// Assert
		Assert.False(result);
		Assert.Equal(student.Guardian.Id, guardianId);
	}

	[Fact]
	public async Task GetStudentChangeGuardianAsync_ReturnsViewModelWithUserDetailsAndAvailableGuardians()
	{
		// Arrange
		var userId = Guid.NewGuid();
		var userDetails = new UserDetailsViewModel
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

		var availableGuardians = new List<BasicViewModel>
		{
			new()
				{ Id = "1", Name = "Guardian 1" },
			new()
				{ Id = "2", Name = "Guardian 2" }
		};

		this._userServiceMock.Setup(m => m.GetDetailsAsync(userId.ToString()))
			.ReturnsAsync(userDetails);

		this._guardianServiceMock.Setup(m => m.GetAllGuardiansAsync())
			.ReturnsAsync(availableGuardians);

		// Act
		var result = await this._sut.GetStudentChangeGuardianAsync(userId.ToString());

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

		foreach (var expectedGuardian in availableGuardians)
		{
			Assert.Contains(result.AvailableGuardians, g => g.Id == expectedGuardian.Id && g.Name == expectedGuardian.Name);
		}
	}

	[Fact]
	public async Task AllAsync_ReturnsAllUsersWithStudentRole()
	{
		// Arrange
		var studentRoleId = Guid.NewGuid();
		var student1Id = Guid.NewGuid();
		var student2Id = Guid.NewGuid();
		var students = new List<ApplicationUser>
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

		var roles = new List<IdentityRole<Guid>>
		{
			new()
			{
				Id = studentRoleId,
				Name = "Student",
				NormalizedName = "STUDENT",
				ConcurrencyStamp = Guid.NewGuid().ToString()
			}
		};

		var userRoles = new List<IdentityUserRole<Guid>>
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

		await this._dbContextMock.Roles.AddRangeAsync(roles);
		await this._dbContextMock.Users.AddRangeAsync(students);
		await this._dbContextMock.UserRoles.AddRangeAsync(userRoles);
		await this._dbContextMock.SaveChangesAsync();

		// Act
		IEnumerable<UserViewModel>? result = await this._sut.AllAsync();

		// Assert
		Assert.NotNull(result);
		List<UserViewModel> resultViewModels = result!.ToList();
		Assert.NotEmpty(resultViewModels);
		Assert.Equal(students.Count, resultViewModels.Count);

		foreach (var expectedUser in students)
		{
			var userViewModel = resultViewModels.FirstOrDefault(u => u.Id == expectedUser.Id.ToString());
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
		var roles = new List<IdentityRole<Guid>>
		{
			new()
			{
				Id = Guid.NewGuid(),
				Name = "Teacher",
				NormalizedName = "TEACHER",
				ConcurrencyStamp = Guid.NewGuid().ToString()
			}
		};

		await this._dbContextMock.Roles.AddRangeAsync(roles);
		await this._dbContextMock.SaveChangesAsync();

		// Act
		IEnumerable<UserViewModel>? result = await this._sut.AllAsync();

		// Assert
		Assert.Null(result);
	}


	public async void Dispose()
	{
		await this._dbContextMock.DisposeAsync();
	}
}