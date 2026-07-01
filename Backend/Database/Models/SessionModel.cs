using System.ComponentModel.DataAnnotations;

namespace Backend.Models;

public class Session
{
    [Key]
    public Guid Id { get; set; }

    [MaxLength(255)]
    public required string Name { get; set; }

    [MaxLength(2048)]
    public required string Description { get; set; }

    public required Guid SurveyId { get; set; }
    public Survey? Survey { get; set; }

    public List<AnonymousUser> AnonymousParticipants { get; } = new List<AnonymousUser>();
    public List<Question> Questions { get; } = new List<Question>();
}