namespace KyberKlass.Services.Data;

using Interfaces;
using KyberKlass.Data;
using KyberKlass.Data.Models;
using Microsoft.EntityFrameworkCore;

public class GuardianService : IGuardianService
{
	private readonly KyberKlassDbContext _dbContext;

	public GuardianService(KyberKlassDbContext dbContext)
	{
		this._dbContext = dbContext;
	}

	public Task<Guardian?> GetById(string id)
	{
		return this._dbContext
			.Guardians
			.AsNoTracking()
			.FirstOrDefaultAsync(g => g.Id == Guid.Parse(id));
	}
}