namespace Kyberklass.Tests.Services;

using System.Globalization;
using KyberKlass.Data;
using KyberKlass.Data.Models;
using KyberKlass.Services.Data;
using KyberKlass.Services.Data.Interfaces;
using KyberKlass.Web.ViewModels.Admin.User;
using Microsoft.AspNetCore.Identity;
using Moq;

public class UserServiceTests : IDisposable
{
	private readonly Mock<KyberKlassDbContext> _dbContextMock;
	private readonly Mock<UserManager<ApplicationUser>> _userManagerMock;
	private readonly Mock<RoleManager<IdentityRole<Guid>>> _roleManagerMock;
	private readonly Mock<IGuardianService> _guardianServiceMock;
	private readonly Mock<IStudentService> _studentServiceMock;
	private readonly Mock<ISchoolService> _schoolServiceMock;
	private readonly UserService _sut;

	public UserServiceTests()
	{
		this._dbContextMock = new Mock<KyberKlassDbContext>();
		this._userManagerMock = this.MockUserManager<ApplicationUser>();
		this._roleManagerMock = this.MockRoleManager<IdentityRole<Guid>>();
		this._guardianServiceMock = new Mock<IGuardianService>();
		this._studentServiceMock = new Mock<IStudentService>();
		this._schoolServiceMock = new Mock<ISchoolService>();

		this._sut = new UserService(
			this._dbContextMock.Object,
			this._userManagerMock.Object,
			this._roleManagerMock.Object,
			this._guardianServiceMock.Object,
			this._schoolServiceMock.Object
		);
	}

	// Helper method to mock UserManager
	private Mock<UserManager<TUser>> MockUserManager<TUser>() where TUser : class
	{
		var store = new Mock<IUserStore<TUser>>();
		var userManagerMock = new Mock<UserManager<TUser>>(store.Object, null, null, null, null, null, null, null, null);
		return userManagerMock;
	}

	// Helper method to mock RoleManager
	private Mock<RoleManager<TRole>> MockRoleManager<TRole>() where TRole : class
	{
		var store = new Mock<IRoleStore<TRole>>();
		var roleManagerMock = new Mock<RoleManager<TRole>>(store.Object, null, null, null, null);
		return roleManagerMock;
	}

	[Fact]
	public async Task GetUserById_Returns_User()
	{
		// Arrange
		var userId = Guid.NewGuid();
		var user = new ApplicationUser
		{
			Id = userId
		};

		// Mock call to GetUserById
		this._dbContextMock.Setup(m => m.Users.FindAsync(user.Id))
			.ReturnsAsync(user);

		// Act
		var result = await this._sut.GetUserById(user.Id.ToString());

		Assert.NotNull(result);
		Assert.Equal(userId, result!.Id);
	}

	[Fact]
	public async Task GetUserById_Returns_Null_IfUserDoesNotExist()
	{
		// Arrange
		var invalidUserId = Guid.NewGuid();

		// Mock call to GetUserById
		this._dbContextMock.Setup(m => m.Users.FindAsync(invalidUserId))
			.ReturnsAsync((ApplicationUser?)null);

		// Act
		var result = await this._sut.GetUserById(invalidUserId.ToString());

		Assert.Null(result);
	}

	[Fact]
	public async Task GetRoleNameByIdAsync_Returns_RoleName()
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
	public async Task GetRoleNameByIdAsync_Returns_Null_For_Invalid_RoleId()
	{
		// Arrange
		string invalidRoleId = Guid.NewGuid().ToString();

		// Setup mock to return null for an invalid role ID
		this._roleManagerMock.Setup(m => m.FindByIdAsync(invalidRoleId))
			.ReturnsAsync((IdentityRole<Guid>)null!);

		// Act
		string? result = await this._sut.GetRoleNameByIdAsync(invalidRoleId);

		// Assert
		Assert.Null(result);
	}

	[Fact]
	public async Task GetDetailsAsync_Returns_UserWithNoRoleDetailsViewModel()
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

		// Mock GetUserById to return the user
		this._dbContextMock.Setup(m => m.Users.FindAsync(userId)).ReturnsAsync(user);

		// Mock GetRoleAsync to return "No Role Assigned"
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
	public async Task GetDetailsAsync_Returns_NullViewModel_IfUserNotFound()
	{
		// Arrange
		string invalidUserId = Guid.NewGuid().ToString();

		this._dbContextMock.Setup(m => m.Users.FindAsync(Guid.Parse(invalidUserId)))
			.ReturnsAsync((ApplicationUser?)null);

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

		// Mock the call to GetUserById to return the student user
		this._dbContextMock.Setup(m => m.Users.FindAsync(student.Id))
			.ReturnsAsync(studentUser);

		// Mock UserManager to return "Student" role
		this._userManagerMock.Setup(m => m.GetRolesAsync(studentUser))
			.ReturnsAsync(new List<string> { "Student" });

		// Act
		var result = await this._sut.GetDetailsAsync(studentUser.Id.ToString());

		// Assert
		Assert.NotNull(result);
		Assert.Equal("Student", result!.Role);
	}

	[Fact]
	public async Task GetForEditAsync_Returns_UserEditFormModel_When_UserExists()
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

		// Mock the GetUserById method to return the user
		this._dbContextMock.Setup(m => m.Users.FindAsync(userId))
			.ReturnsAsync(user);

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
	public async Task GetForEditAsync_Returns_Null_When_UserDoesNotExist()
	{
		// Arrange
		var invalidUserId = Guid.NewGuid();

		// Mock the GetUserById method to return null for an invalid user ID
		this._dbContextMock.Setup(m => m.Users.FindAsync(invalidUserId))
			.ReturnsAsync((ApplicationUser?)null);

		// Act
		var result = await this._sut.GetForEditAsync(invalidUserId.ToString());

		// Assert
		Assert.Null(result);
	}

	[Fact]
	public async Task EditAsync_Returns_UserEditFormModel_When_UserExists()
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

		// Mock the GetUserById method to return the user
		this._dbContextMock.Setup(m => m.Users.FindAsync(userId))
			.ReturnsAsync(user);

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
	public async Task EditAsync_Returns_Null_When_UserDoesNotExist()
	{
		// Arrange
		var invalidUserId = Guid.NewGuid();
		var model = new UserEditFormModel();

		// Mock the GetUserById method to return null for an invalid user ID
		this._dbContextMock.Setup(m => m.Users.FindAsync(invalidUserId))
			.ReturnsAsync((ApplicationUser?)null);

		// Act
		var result = await this._sut.EditAsync(invalidUserId.ToString(), model);

		// Assert
		Assert.Null(result);
		this._dbContextMock.Verify(m => m.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
	}

	[Fact]
	public async Task EditAsync_Updates_All_Properties()
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

		var initialUser = new ApplicationUser
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

		this._dbContextMock.Setup(m => m.Users.FindAsync(userId))
			.ReturnsAsync(initialUser);

		// Act
		var result = await this._sut.EditAsync(userId.ToString(), model);

		// Assert
		Assert.NotNull(result);
		this._dbContextMock.Verify(m => m.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);

		Assert.Equal(model.FirstName, initialUser.FirstName);
		Assert.Equal(model.LastName, initialUser.LastName);
		Assert.Equal(DateTime.ParseExact(model.BirthDate, "yyyy-MM-dd", CultureInfo.InvariantCulture), initialUser.BirthDate);
		Assert.Equal(model.Address, initialUser.Address);
		Assert.Equal(model.PhoneNumber, initialUser.PhoneNumber);
		Assert.Equal(model.Email, initialUser.Email);
		Assert.Equal(model.IsActive, initialUser.IsActive);
	}

	public void Dispose()
	{
	}
}