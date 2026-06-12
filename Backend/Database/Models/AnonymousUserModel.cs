using System.ComponentModel.DataAnnotations;

namespace Backend.Models;

public class AnonymousUser
{
    [Key]
    public Guid Id { get; set; }

    public required string Name { get; set; }

    public Guid SessionId { get; set; }
    public required Session Session { get; set; }

    public Guid ProfilePictureId { get; set; }
    public required AnonymousProfilePicture ProfilePicture { get; set; }

    public List<Answer> Answers { get; set; } = new List<Answer>();
}