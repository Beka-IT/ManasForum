using Microsoft.EntityFrameworkCore;
using UserService.Models;

namespace UserService.Data;
public class AppDbContext : DbContext
{
    public DbSet<Account> Accounts { get; set; }
    
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }
}