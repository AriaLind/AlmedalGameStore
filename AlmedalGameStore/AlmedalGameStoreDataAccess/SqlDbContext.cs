using System.ComponentModel.DataAnnotations;
using AlmedalGameStoreShared.Entities;
using AlmedalGameStoreShared.Interfaces;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AlmedalGameStoreDataAccess;

public class SqlDbContext : IdentityDbContext<User>
{
    public DbSet<ApiAuthKey> ApiAuthKeys { get; set; }

    public SqlDbContext(DbContextOptions<SqlDbContext> options) : base(options)
    {
        
    }
}

public class ApiAuthKey : IEntity<int>
{
    [Key]
    public int Id { get; set; }
    [Required]
    public string ApiKeyName { get; set; }
    [Required]
    public string Key { get; set; }
}