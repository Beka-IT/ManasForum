using DiscussionService.Models;
using Microsoft.EntityFrameworkCore;
using UserService.Models;

namespace DiscussionService.Data;

public class DiscussionServiceContext : DbContext
{
    public DbSet<Account> Accounts { get; set; }
    
    public DbSet<Question> Questions { get; set; }
    
    public DbSet<Answer> Answers { get; set; }
    
    public DiscussionServiceContext(DbContextOptions<DiscussionServiceContext> options) : base(options)
    {
    }
}