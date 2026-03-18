using Godot;

namespace ArcaneDuel;

public sealed class UnitPiece
{
    public required string Id { get; init; }
    public required string DisplayName { get; init; }
    public required string ShortCode { get; init; }
    public required TeamSide Team { get; init; }
    public required Vector2I Position { get; set; }
    public required int MoveRange { get; init; }
    public required int Power { get; init; }
}

