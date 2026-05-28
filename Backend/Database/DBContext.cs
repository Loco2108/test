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
    public DbSet<QuestionType> QuestionTypes { get; set; }
    public DbSet<QuestionTemplate> QuestionTemplates { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<QuestionType>().HasData(
            new QuestionType
            {
                Id = QuestionTypeEnum.SingleChoice,
                Name = "Single Choice",
                Description = "Choose one of multiple answer options"
            },
            new QuestionType
            {
                Id = QuestionTypeEnum.MultipleChoice,
                Name = "Multiple Choice",
                Description = "Choose multiple answer options"
            },
            new QuestionType
            {
                Id = QuestionTypeEnum.WordCloud,
                Name = "Word Cloud",
                Description = "Submit a single word which is displayed in a cloud"
            },
            new QuestionType
            {
                Id = QuestionTypeEnum.FreeText,
                Name = "Free Text",
                Description = "Submit any text of your choice"
            },
            new QuestionType
            {
                Id = QuestionTypeEnum.NumberScale,
                Name = "Number Scale",
                Description = "Submit a rating from 1 to 10"
            }
        );

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

        builder.Entity<QuestionTemplate>()
            .HasOne(x => x.QuestionType)
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