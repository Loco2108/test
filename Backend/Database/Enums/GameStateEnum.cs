using TypeGen.Core.TypeAnnotations;

namespace Backend.Models.Enums;

[ExportTsEnum]
public enum GameState
{
    Lobby = 1,
    Loading,
    Question,
}