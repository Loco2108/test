using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace Backend.Models;

public class User : IdentityUser<Guid>
{
    [Url]
    [MaxLength(2048)]
    public string? ProfilePictureUrl { get; set; }

    public List<Folder> Folders { get; } = new List<Folder>();
    public List<Survey> Surveys { get; } = new List<Survey>();
}