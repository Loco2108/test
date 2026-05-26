using Backend.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

public class StimmtiDbContext : IdentityDbContext<User, IdentityRole<Guid>, Guid>
{
    public StimmtiDbContext(DbContextOptions<StimmtiDbContext> options) : base(options) { }

    //public DbSet<User> Users { get; set; }
}