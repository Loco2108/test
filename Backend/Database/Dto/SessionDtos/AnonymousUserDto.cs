using TypeGen.Core.TypeAnnotations;

namespace Backend.Dto;

[ExportTsInterface]
public class AnonymousUserDto
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
}