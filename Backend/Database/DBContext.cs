using Backend.Models;
using Backend.Models.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

public class StimmtiDbContext : IdentityDbContext<User, IdentityRole<Guid>, Guid>
{
    public StimmtiDbContext(DbContextOptions<StimmtiDbContext> options) : base(options) { }

    public DbSet<Folder> Folders { get; set; }
    public DbSet<Survey> Surveys { get; set; }
    public DbSet<Session> Sessions { get; set; }
    public DbSet<QuestionTemplate> QuestionTemplates { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<Folder>()
            .HasOne(x => x.Owner)
            .WithMany(x => x.Folders);

        builder.Entity<Survey>()
            .HasOne(x => x.Folder)
            .WithMany(x => x.Surveys);

        builder.Entity<Session>()
            .HasOne(x => x.Survey)
            .WithMany(x => x.Sessions);

        builder.Entity<QuestionTemplate>()
            .HasOne(x => x.Survey)
            .WithMany(x => x.QuestionTemplates);

        builder.Entity<AnonymousUser>()
            .HasOne(x => x.Session)
            .WithMany(x => x.AnonymousParticipants);

        builder.Entity<AnonymousUser>()
            .HasOne(x => x.ProfilePicture)
            .WithOne(x => x.AnonymousUser);

        builder.Entity<AnswerOption>()
            .HasOne(x => x.QuestionTemplate)
            .WithMany(x => x.AnswerOptions);

        builder.Entity<AnswerOption>()
            .HasOne(x => x.Answer)
            .WithOne(x => x.AnswerOption);

        builder.Entity<Answer>()
            .HasOne(x => x.AnonymousUser)
            .WithMany(x => x.Answers);

        builder.Entity<Answer>()
            .HasOne(x => x.Question)
            .WithMany(x => x.Answers);
    }
}