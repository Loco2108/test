using System.ComponentModel.DataAnnotations;
using TypeGen.Core.TypeAnnotations;

[ExportTsInterface]
public class JoinSessionDto
{
    public required string RoomCode { get; set; }
    public Guid? playerId { get; set; }
}