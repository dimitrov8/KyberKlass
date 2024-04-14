namespace KyberKlass.Web.Infrastructure.Extensions;

/// <summary>
///     Extension methods for input validation.
/// </summary>
public static class ValidationExtensions
{
	/// <summary>
	///     Asynchronously checks if the provided ID and model are not null or empty.
	/// </summary>
	/// <typeparam name="T">Type of the model.</typeparam>
	/// <param name="id">The ID to validate.</param>
	/// <param name="model">The model to validate.</param>
	/// <returns>True if the ID and model are not null or empty; otherwise, false.</returns>
	public static async Task<bool> IsNotNullOrEmptyInputAsync<T>(string? id, T? model) where T : class
	{
		// If id is null or empty, return false
		if (string.IsNullOrEmpty(id))
		{
			return false;
		}

		// If model is provided and its Id property is null or empty, return false
		if (model != null)
		{
			var idProperty = typeof(T).GetProperty("Id");

			if (idProperty != null && string.IsNullOrEmpty(idProperty.GetValue(model) as string))
			{
				return false;
			}
		}

		// If none of the above conditions are met, return true
		return true;
	}
}