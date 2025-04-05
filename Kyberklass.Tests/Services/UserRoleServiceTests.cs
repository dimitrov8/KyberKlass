using KyberKlass.Data;
using KyberKlass.Data.Models;
using KyberKlass.Services.Data.Interfaces;
using KyberKlass.Services.Data.Interfaces.Guardians;
using KyberKlass.Services.Data.Interfaces.Users;
using KyberKlass.Services.Data.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Moq;
using System.Globalization;

namespace KyberKlass.Tests.Services
{
    public class UserRoleServiceTests : IDisposable
    {
        private readonly DbContextOptions<KyberKlassDbContext> _options;
        private readonly KyberKlassDbContext _dbContextMock;
        private readonly Mock<UserManager<ApplicationUser>> _userManagerMock;
        private readonly Mock<RoleManager<IdentityRole<Guid>>> _roleManagerMock;
        private readonly Mock<IUserService> _userServiceMock;
        private readonly Mock<ISchoolService> _schoolServiceMock;
        private readonly Mock<IGuardianService> _guardianServiceMock;
        private readonly UserRoleService _sut;

        public UserRoleServiceTests()
        {
            _options = new DbContextOptionsBuilder<KyberKlassDbContext>()
                .UseInMemoryDatabase(databaseName: "KyberKlassTestDb")
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

            _roleManagerMock = new Mock<RoleManager<IdentityRole<Guid>>>(
              Mock.Of<IRoleStore<IdentityRole<Guid>>>(),
              null,
              null,
              null,
              null
          );

            _userServiceMock = new Mock<IUserService>();
            _schoolServiceMock = new Mock<ISchoolService>();
            _guardianServiceMock = new Mock<IGuardianService>();

            _sut = new UserRoleService(_dbContextMock, _userManagerMock.Object, _roleManagerMock.Object, _userServiceMock.Object, _schoolServiceMock.Object, _guardianServiceMock.Object);
        }

        [Fact]
        public async Task GetRoleNameByIdAsyncReturnsRoleName()
        {
            // Arrange
            string roleId = Guid.NewGuid().ToString();
            string roleName = "Admin";
            IdentityRole<Guid> role = new(roleName);

            _roleManagerMock.Setup(m => m.FindByIdAsync(roleId))
                .ReturnsAsync(role);

            // Act
            string? result = await _sut.GetRoleNameByIdAsync(roleId);

            // Assert
            Assert.Equal(roleName, result);
        }

        [Fact]
        public async Task GetRoleNameByIdAsync_ReturnsNullForInvalidRoleId()
        {
            // Arrange
            string invalidRoleId = Guid.NewGuid().ToString();

            _roleManagerMock.Setup(m => m.FindByIdAsync(invalidRoleId))
                .ReturnsAsync((IdentityRole<Guid>)null!);

            // Act
            string? result = await _sut.GetRoleNameByIdAsync(invalidRoleId);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task UpdateRoleAsync_UserNotFoundReturnsFalse()
        {
            // Arrange
            Guid invalidUserId = Guid.NewGuid();
            Guid roleId = Guid.NewGuid();

            _userManagerMock.Setup(m => m.FindByIdAsync(invalidUserId.ToString()))!
                .ReturnsAsync((ApplicationUser?)null);

            // Act
            bool result = await _sut.UpdateRoleAsync(invalidUserId.ToString(), roleId.ToString(), null, null, null);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public async Task UpdateRoleAsync_RoleNotFoundReturnsFalse()
        {
            // Arrange
            Guid userId = Guid.NewGuid();
            Guid invalidRoleId = Guid.NewGuid();
            ApplicationUser user = new()
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

            _userManagerMock.Setup(m => m.FindByIdAsync(userId.ToString()))
                .ReturnsAsync(user);

            _roleManagerMock.Setup(m => m.FindByIdAsync(invalidRoleId.ToString()))!
                .ReturnsAsync((IdentityRole<Guid>?)null);

            // Act
            bool result = await _sut.UpdateRoleAsync(userId.ToString(), invalidRoleId.ToString(), null, null, null);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public async Task GetForUpdateRoleAsync_UserNotFoundReturnsNull()
        {
            // Arrange
            string userId = Guid.NewGuid().ToString();

            // Act
            Web.ViewModels.Admin.User.UserUpdateRoleViewModel? result = await _sut.GetForUpdateRoleAsync(userId);

            // Assert
            Assert.Null(result);
        }

        public void Dispose()
        {
            _dbContextMock.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
