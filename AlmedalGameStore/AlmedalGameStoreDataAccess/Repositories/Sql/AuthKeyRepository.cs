using AlmedalGameStoreShared.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AlmedalGameStoreDataAccess.Repositories.Sql;

public class AuthKeyRepository : IRepository<ApiAuthKey, int>
{
    private readonly SqlDbContext _context;

    public AuthKeyRepository(SqlDbContext context)
    {
        _context = context;
    }

    public async Task<ApiAuthKey> GetByIdAsync(int id)
    {
        var key = await _context.ApiAuthKeys.FirstOrDefaultAsync(k => k.Id == id);

        return key;
    }


    public async Task<ApiAuthKey> GetByNameAsync(string name)
    {
        var apiKey = await _context.ApiAuthKeys.FirstOrDefaultAsync(k => k.ApiKeyName == name);

        return apiKey;
    }




    public async Task<IEnumerable<ApiAuthKey>> GetAllAsync()
    {
        throw new NotImplementedException();
    }

    public async Task AddAsync(ApiAuthKey entity)
    {
        throw new NotImplementedException();
    }

    public async Task UpdateAsync(ApiAuthKey entity)
    {
        throw new NotImplementedException();
    }

    public async Task DeleteAsync(int id)
    {
        throw new NotImplementedException();
    }
}