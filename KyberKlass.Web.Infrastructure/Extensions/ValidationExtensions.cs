namespace KyberKlass.Web.Infrastructure.Extensions;

public static class ValidationExtensions
{
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