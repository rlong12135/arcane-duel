using Godot;

namespace ArcaneDuel;

public sealed class SkirmishState
{
    public const int BoardSize = 7;

    private readonly List<UnitPiece> _units = [];

    public IReadOnlyList<UnitPiece> Units => _units;
    public TeamSide ActiveTeam { get; private set; } = TeamSide.Dawn;
    public UnitPiece? SelectedUnit { get; private set; }
    public string StatusMessage { get; private set; } = "Dawn to act.";
    public TeamSide? WinningTeam { get; private set; }

    public void Reset()
    {
        _units.Clear();
        _units.AddRange(CreateInitialUnits());
        ActiveTeam = TeamSide.Dawn;
        SelectedUnit = null;
        WinningTeam = null;
        StatusMessage = "Dawn to act. Select a unit and move or attack.";
    }

    public UnitPiece? GetUnitAt(Vector2I position)
    {
        return _units.FirstOrDefault(unit =>
            unit.Position.X == position.X &&
            unit.Position.Y == position.Y);
    }

    public void HandleTileActivated(Vector2I position)
    {
        if (WinningTeam is not null)
        {
            StatusMessage = $"{WinningTeam} already controls the field. Start a new skirmish.";
            return;
        }

        var clickedUnit = GetUnitAt(position);

        if (clickedUnit is not null && clickedUnit.Team == ActiveTeam)
        {
            SelectedUnit = clickedUnit;
            StatusMessage = $"{clickedUnit.DisplayName} selected. Choose an empty tile or adjacent enemy.";
            return;
        }

        if (SelectedUnit is null)
        {
            StatusMessage = $"{ActiveTeam} must select a unit first.";
            return;
        }

        if (clickedUnit is null)
        {
            TryMove(position);
            return;
        }

        if (clickedUnit.Team != ActiveTeam)
        {
            TryAttack(clickedUnit);
        }
    }

    public string BuildRosterSummary()
    {
        var dawnUnits = _units.Where(unit => unit.Team == TeamSide.Dawn).OrderBy(unit => unit.DisplayName).ToList();
        var duskUnits = _units.Where(unit => unit.Team == TeamSide.Dusk).OrderBy(unit => unit.DisplayName).ToList();

        return string.Join(
            "\n\n",
            BuildTeamSummary("Dawn", dawnUnits),
            BuildTeamSummary("Dusk", duskUnits));
    }

    private void TryMove(Vector2I destination)
    {
        if (SelectedUnit is null)
        {
            return;
        }

        if (!IsWithinBounds(destination))
        {
            StatusMessage = "That tile is outside the board.";
            return;
        }

        var distance = Mathf.Abs(destination.X - SelectedUnit.Position.X) + Mathf.Abs(destination.Y - SelectedUnit.Position.Y);
        if (distance == 0)
        {
            StatusMessage = "Select a different tile.";
            return;
        }

        if (distance > SelectedUnit.MoveRange)
        {
            StatusMessage = $"{SelectedUnit.DisplayName} can only move {SelectedUnit.MoveRange} tiles.";
            return;
        }

        SelectedUnit.Position = destination;
        var movedUnit = SelectedUnit;
        EndTurn($"{movedUnit.DisplayName} advances to {FormatTile(destination)}.");
    }

    private void TryAttack(UnitPiece defender)
    {
        if (SelectedUnit is null)
        {
            return;
        }

        var distance = Mathf.Abs(defender.Position.X - SelectedUnit.Position.X) + Mathf.Abs(defender.Position.Y - SelectedUnit.Position.Y);
        if (distance != 1)
        {
            StatusMessage = "Attacks only work against adjacent enemies in this prototype.";
            return;
        }

        var attacker = SelectedUnit;
        _units.Remove(defender);
        attacker.Position = defender.Position;

        WinningTeam = ResolveWinner();
        if (WinningTeam is not null)
        {
            SelectedUnit = null;
            StatusMessage = $"{attacker.DisplayName} defeats {defender.DisplayName}. {WinningTeam} wins the skirmish.";
            return;
        }

        EndTurn($"{attacker.DisplayName} defeats {defender.DisplayName} at {FormatTile(attacker.Position)}.");
    }

    private void EndTurn(string actionSummary)
    {
        SelectedUnit = null;
        ActiveTeam = ActiveTeam == TeamSide.Dawn ? TeamSide.Dusk : TeamSide.Dawn;
        StatusMessage = $"{actionSummary} {ActiveTeam} to act.";
    }

    private TeamSide? ResolveWinner()
    {
        var dawnAlive = _units.Any(unit => unit.Team == TeamSide.Dawn);
        var duskAlive = _units.Any(unit => unit.Team == TeamSide.Dusk);

        if (!dawnAlive)
        {
            return TeamSide.Dusk;
        }

        if (!duskAlive)
        {
            return TeamSide.Dawn;
        }

        return null;
    }

    private static bool IsWithinBounds(Vector2I position)
    {
        return position.X >= 0 && position.X < BoardSize && position.Y >= 0 && position.Y < BoardSize;
    }

    private static string FormatTile(Vector2I position)
    {
        return $"{(char)('A' + position.X)}{position.Y + 1}";
    }

    private static string BuildTeamSummary(string teamName, IEnumerable<UnitPiece> units)
    {
        var lines = units.Select(unit => $"{unit.ShortCode}  {unit.DisplayName}  {FormatTile(unit.Position)}");
        return $"{teamName}\n{string.Join("\n", lines)}";
    }

    private static List<UnitPiece> CreateInitialUnits()
    {
        return new List<UnitPiece>
        {
            new UnitPiece
            {
                Id = "dawn-guardian",
                DisplayName = "Guardian",
                ShortCode = "GU",
                Team = TeamSide.Dawn,
                Position = new Vector2I(0, 1),
                MoveRange = 2,
                Power = 3
            },
            new UnitPiece
            {
                Id = "dawn-archer",
                DisplayName = "Archer",
                ShortCode = "AR",
                Team = TeamSide.Dawn,
                Position = new Vector2I(1, 0),
                MoveRange = 3,
                Power = 2
            },
            new UnitPiece
            {
                Id = "dawn-mage",
                DisplayName = "Mage",
                ShortCode = "MG",
                Team = TeamSide.Dawn,
                Position = new Vector2I(1, 2),
                MoveRange = 2,
                Power = 4
            },
            new UnitPiece
            {
                Id = "dawn-vanguard",
                DisplayName = "Vanguard",
                ShortCode = "VG",
                Team = TeamSide.Dawn,
                Position = new Vector2I(0, 4),
                MoveRange = 2,
                Power = 3
            },
            new UnitPiece
            {
                Id = "dusk-guardian",
                DisplayName = "Guardian",
                ShortCode = "GU",
                Team = TeamSide.Dusk,
                Position = new Vector2I(6, 5),
                MoveRange = 2,
                Power = 3
            },
            new UnitPiece
            {
                Id = "dusk-archer",
                DisplayName = "Archer",
                ShortCode = "AR",
                Team = TeamSide.Dusk,
                Position = new Vector2I(5, 6),
                MoveRange = 3,
                Power = 2
            },
            new UnitPiece
            {
                Id = "dusk-mage",
                DisplayName = "Mage",
                ShortCode = "MG",
                Team = TeamSide.Dusk,
                Position = new Vector2I(5, 4),
                MoveRange = 2,
                Power = 4
            },
            new UnitPiece
            {
                Id = "dusk-vanguard",
                DisplayName = "Vanguard",
                ShortCode = "VG",
                Team = TeamSide.Dusk,
                Position = new Vector2I(6, 2),
                MoveRange = 2,
                Power = 3
            }
        };
    }
}
