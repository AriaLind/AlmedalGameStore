using AlmedalGameStoreDataAccess.Repositories.Sql;

namespace AlmedalGameStoreDataAccess.UnitsOfWork;

public class SqlUnitOfWork
{
    private readonly SqlDbContext _context;

    private AuthKeyRepository _authKeyRepository;
    private UserRepository _userRepository;

    public SqlUnitOfWork(SqlDbContext context)
    {
        _context = context;
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }

    public UserRepository UserRepository
    {
        get
        {
            if (_userRepository == null)
            {
                _userRepository = new UserRepository(_context);
            }
            return _userRepository;
        }
    }

    public AuthKeyRepository AuthKeyRepository
    {
        get
        {
            if (_authKeyRepository == null)
            {
                _authKeyRepository = new AuthKeyRepository(_context);
            }
            return _authKeyRepository;
        }
    }
}