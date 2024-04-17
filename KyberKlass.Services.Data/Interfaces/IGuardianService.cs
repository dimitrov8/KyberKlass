namespace KyberKlass.Services.Data.Interfaces;

using KyberKlass.Data.Models;
using Web.ViewModels.Admin;

/// <summary>
///     Interface defining operations for retrieving guardian information.
/// </summary>
public interface IGuardianService
{
	/// <summary>
	///     Retrieves a guardian entity by its unique identifier asynchronously.
	/// </summary>
	/// <param name="id">The unique identifier of the guardian.</param>
	/// <returns>The guardian entity if found; otherwise, null.</returns>
	Task<Guardian?> GetByIdAsync(string id);

	/// <summary>
	///     Retrieves a collection of basic view models for all guardians asynchronously.
	/// </summary>
	/// <returns>A collection of basic view models representing all guardians.</returns>
	Task<IEnumerable<BasicViewModel>> GetAllGuardiansAsync();

	/// <summary>
	///     Checks if a guardian is assigned to any student asynchronously.
	/// </summary>
	/// <param name="userId">The unique identifier of the guardian.</param>
	/// <returns>True if the guardian is assigned to any student; otherwise, false.</returns>
	Task<bool> IsGuardianAssignedToStudentAsync(string userId);

	public Task<Guardian?> GetGuardianByUserIdAsync(string userId);
}