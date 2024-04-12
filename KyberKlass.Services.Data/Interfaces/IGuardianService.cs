namespace KyberKlass.Services.Data.Interfaces;

using KyberKlass.Data.Models;

/// <summary>
/// Interface defining operations for retrieving guardian information.
/// </summary>
public interface IGuardianService
{
	/// <summary>
	/// Retrieves a guardian entity by its unique identifier asynchronously.
	/// </summary>
	/// <param name="id">The unique identifier of the guardian.</param>
	/// <returns>The guardian entity if found; otherwise, null.</returns>
	Task<Guardian?> GetByIdAsync(string id);
}