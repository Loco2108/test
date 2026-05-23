using Microsoft.EntityFrameworkCore;

public class StimmtiDbContext : DbContext
{
    public StimmtiDbContext(DbContextOptions<StimmtiDbContext> options) : base(options) { }

    public DbSet<Poll> Polls { get; set; }
}