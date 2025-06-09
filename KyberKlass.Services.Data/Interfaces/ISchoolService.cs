using KyberKlass.Web.ViewModels.Admin;
using KyberKlass.Web.ViewModels.Admin.School;

namespace KyberKlass.Services.Data.Interfaces;
/// <summary>
///     Interface for managing school-related operations.
/// </summary>
public interface ISchoolService
{
    /// <summary>
    ///     Adds a new school asynchronously.
    /// </summary>
    /// <param name="model">The model containing information about the school to add.</param>
    /// <returns>True if the school was added successfully; otherwise, false.</returns>
    Task<bool> AddAsync(AddSchoolFormModel model);

    /// <summary>
    ///     Retrieves all schools asynchronously, optionally filtered by a search term.
    ///     Optionally filters by search term.
    /// </summary>
    /// <param name="searchTerm">Optional search term to filter schools.</param>
    /// <returns>A collection of school view models matching the search criteria.</returns>
    Task<IEnumerable<SchoolViewModel>> AllAsync(string? searchTerm = null);

    /// <summary>
    ///     Retrieves basic information about all schools asynchronously.
    /// </summary>
    /// <returns>A collection of basic user view models representing schools.</returns>
    Task<IEnumerable<BasicViewModel>> BasicAllAsync();

    /// <summary>
    ///     Retrieves a specific school by unique identifier asynchronously.
    /// </summary>
    /// <param name="id">The unique identifier of the school to retrieve.</param>
    /// <returns>The view model for the specified school if found; otherwise, null.</returns>
    Task<SchoolDetailsViewModel?> GetByIdAsync(string id);

    /// <summary>
    ///     Retrieves school information for editing asynchronously.
    /// </summary>
    /// <param name="id">The unique identifier of the school to edit.</param>
    /// <returns>The edit model for the specified school if found; otherwise, null.</returns>
    Task<AddSchoolFormModel?> GetForEditAsync(string id);

    /// <summary>
    ///     Edits a school asynchronously.
    /// </summary>
    /// <param name="id">The unique identifier of the school to edit.</param>
    /// <param name="model">The updated school view model.</param>
    /// <returns>True if the school was edited successfully; otherwise, false.</returns>
    Task<bool> EditAsync(string id, AddSchoolFormModel model);

    /// <summary>
    ///     Retrieves details of a specific school asynchronously.
    /// </summary>
    /// <param name="id">The unique identifier of the school to view.</param>
    /// <returns>The view model for the specified school if found; otherwise, null.</returns>
    Task<SchoolDetailsViewModel?> ViewDetailsAsync(string id);

    /// <summary>
    ///     Checks if a classroom with the specified unique identifier exists in a school asynchronously.
    /// </summary>
    /// <param name="schoolId">The unique identifier of the school to search within.</param>
    /// <param name="classroomId">The unique identifier of the classroom to check.</param>
    /// <returns>True if the classroom exists; otherwise, false.</returns>
    Task<bool> ClassroomExistsInSchoolAsync(string schoolId, string classroomId);

    /// <summary>
    ///     Retrieves school information for deletion asynchronously.
    /// </summary>
    /// <param name="id">The unique identifier of the school to delete.</param>
    /// <returns>The view model for the specified school if found; otherwise, null.</returns>
    Task<SchoolDetailsViewModel?> GetForDeleteAsync(string id);

    /// <summary>
    ///     Deletes a school asynchronously.
    /// </summary>
    /// <param name="id">The unique identifier of the school to delete.</param>
    /// <returns>True if the school was deleted successfully; otherwise, false.</returns>
    Task<bool> DeleteAsync(string id);

    /// <summary>
    ///     Checks if a school has any students asynchronously.
    /// </summary>
    /// <param name="id">The unique identifier of the school.</param>
    /// <returns>True if school has students; otherwise, false.</returns>
    Task<bool> HasStudentsAssignedAsync(string id);
}