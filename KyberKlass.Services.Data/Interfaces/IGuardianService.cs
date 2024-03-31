namespace KyberKlass.Services.Data.Interfaces;

using KyberKlass.Data.Models;

public interface IGuardianService
{
	Task<Guardian?> GetById(string id);
}