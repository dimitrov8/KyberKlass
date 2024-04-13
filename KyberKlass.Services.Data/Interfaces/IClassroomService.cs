namespace KyberKlass.Services.Data.Interfaces;

using KyberKlass.Web.ViewModels.Admin.School;
using Web.ViewModels.Admin;
using Web.ViewModels.Admin.Classroom;

public interface IClassroomService
{
    /// <summary>
    /// Adds a new classroom asynchronously.
    /// </summary>
    /// <param name="model">The model containing information about the classroom to add.</param>
    /// <returns>True if the classroom was added successfully; otherwise, false.</returns>
    Task<bool> AddAsync(AddClassroomViewModel model);

    /// <summary>
    /// Checks if a classroom with the given name exists in the specified school.
    /// </summary>
    /// <param name="classroomName">The name of the classroom.</param>
    /// <param name="schoolId">The unique identifier of the school.</param>
    /// <returns>True if the classroom exists in the school; otherwise, false.</returns>
    Task<bool> ClassroomExistsInSchoolAsync(string classroomName, string schoolId);

    /// <summary>
    /// Retrieves information of a classroom by unique identifier asynchronously.
    /// </summary>
    /// <param name="schoolId">The unique identifier of the school.</param>
    /// <param name="classroomId">The unique identifier of the classroom to get.</param>
    /// <returns>The information of the classroom if found; otherwise, null.</returns>
    Task<ClassroomDetailsViewModel?> GetClassroomAsync(string schoolId, string classroomId);

    ///// <summary>
    ///// Retrieves details of a classroom by unique identifier asynchronously.
    ///// </summary>
    ///// <param name="id">The unique identifier of the classroom.</param>
    ///// <returns>The details of the classroom if found; otherwise, null.</returns>
    //Task<ClassroomDetailsViewModel?> GetDetailsAsync(string id);

    /// <summary>
    /// Retrieves information about all classrooms of a given school by unique identifier asynchronously.
    /// </summary>
    /// <param name="schoolId">The unique identifier of the school.</param>
    /// <returns>A collection of classroom view models representing classrooms in a school.</returns>
    Task<IEnumerable<ClassroomDetailsViewModel>> GetAllClassroomsAsync(string schoolId);

    /// <summary>
    /// Retrieves details of all classrooms of a given school by unique identifier asynchronously and returns them as JSON.
    /// </summary>
    /// <param name="schoolId">The unique identifier of the school.</param>
    /// <returns>A collection of classroom view models representing classrooms in a school, serialized as JSON.</returns>
    Task<IEnumerable<BasicViewModel>> GetAllClassroomsBySchoolIdAsJsonAsync(string schoolId);

	///// <summary>
	///// Edits a classroom asynchronously.
	///// </summary>
	///// <param name="id">The unique identifier of the classroom to edit.</param>
	///// <param name="model">The updated classroom view model.</param>
	///// <returns>True if the classroom was edited successfully; otherwise, false.</returns>
	//Task<bool> EditAsync(string id, ClassroomDetailsViewModel model);

	/// <summary>
	/// Retrieves classroom information for deletion asynchronously.
	/// </summary>
	/// <param name="schoolId">The unique identifier of the school.</param>
	/// <param name="classroomId">The unique identifier of the classroom in the school to delete.</param>
	/// <returns>The view model for the specified classroom if found; otherwise, null.</returns>
	Task<ClassroomDetailsViewModel?> GetForDeleteAsync(string schoolId, string classroomId);

	/// <summary>
	/// Deletes a classroom asynchronously.
	/// </summary>
	/// <param name="schoolId">The unique identifier of the school.</param>
	/// <param name="classroomId">The unique identifier of the classroom in the school to delete.</param>
	/// <returns>True if the classroom was deleted successfully; otherwise, false.</returns>
	Task<bool> DeleteAsync(string schoolId, string classroomId);
}