using System.ComponentModel.DataAnnotations;
using Backend.Models.Enums;

namespace Backend.Models;

public class QuestionType
{
    [Key]
    public QuestionTypeEnum Id { get; set; }

    public required string Name { get; set; }
    public required string Description { get; set; }
}