using AlmedalGameStoreShared.Entities;
using AlmedalGameStoreShared.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AlmedalGameStoreDataAccess.Repositories.Sql;

public class UserRepository : IRepository<User, string>
{
    private readonly SqlDbContext _context;

    public UserRepository(SqlDbContext context)
    {
        _context = context;
    }

    public async Task<User> GetByIdAsync(string id)
    {
        var user = await _context.Users.FindAsync(id);

        return user;
    }

    public async Task<IEnumerable<User>> GetAllAsync()
    {
        var users = await _context.Users.ToListAsync(); 
        return users;
    }

    public async Task AddAsync(User entity)
    {
        await _context.Users.AddAsync(entity);
    }

    public async Task UpdateAsync(User entity)
    {
        _context.Users.Update(entity);
    }

    public async Task DeleteAsync(string id)
    {
        var user = await _context.Users.FindAsync(id);
        _context.Users.Remove(user);
    }
}