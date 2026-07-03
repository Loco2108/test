using Tapper;

namespace Backend.Models.Enums;

[TranspilationSource]
public enum GameState
{
    Lobby = 1,
    Loading,
    Question,
}