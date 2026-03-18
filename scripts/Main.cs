using Godot;

namespace ArcaneDuel;

public partial class Main : Control
{
    private readonly Button[,] _tileButtons = new Button[SkirmishState.BoardSize, SkirmishState.BoardSize];
    private readonly SkirmishState _skirmish = new();

    private GridContainer? _boardGrid;
    private Label? _turnValueLabel;
    private Label? _selectedValueLabel;
    private RichTextLabel? _rosterValueLabel;
    private Label? _statusLabel;

    public override void _Ready()
    {
        _boardGrid = GetNode<GridContainer>("Margin/Layout/Content/BoardColumn/BoardFrame/BoardMargin/BoardGrid");
        _turnValueLabel = GetNode<Label>("Margin/Layout/Content/Sidebar/TurnPanel/TurnValue");
        _selectedValueLabel = GetNode<Label>("Margin/Layout/Content/Sidebar/SelectedPanel/SelectedValue");
        _rosterValueLabel = GetNode<RichTextLabel>("Margin/Layout/Content/Sidebar/RosterPanel/RosterValue");
        _statusLabel = GetNode<Label>("Margin/Layout/Actions/StatusLabel");

        var newSkirmishButton = GetNode<Button>("Margin/Layout/Actions/NewSkirmishButton");
        newSkirmishButton.Pressed += StartNewSkirmish;

        BuildBoard();
        StartNewSkirmish();
    }

    private void BuildBoard()
    {
        if (_boardGrid is null)
        {
            return;
        }

        _boardGrid.Columns = SkirmishState.BoardSize;
        _boardGrid.MouseFilter = MouseFilterEnum.Stop;

        for (var row = 0; row < SkirmishState.BoardSize; row++)
        {
            for (var column = 0; column < SkirmishState.BoardSize; column++)
            {
                var tilePosition = new Vector2I(column, row);
                var tileButton = new Button
                {
                    CustomMinimumSize = new Vector2(96, 96),
                    FocusMode = FocusModeEnum.None,
                    ClipText = true,
                    TextOverrunBehavior = TextServer.OverrunBehavior.TrimChar
                };

                tileButton.Pressed += () => OnTilePressed(tilePosition);
                _tileButtons[column, row] = tileButton;
                _boardGrid.AddChild(tileButton);
            }
        }
    }

    private void StartNewSkirmish()
    {
        _skirmish.Reset();
        RefreshUi();
    }

    private void OnTilePressed(Vector2I tilePosition)
    {
        _skirmish.HandleTileActivated(tilePosition);
        RefreshUi();
    }

    private void RefreshUi()
    {
        if (_turnValueLabel is null || _selectedValueLabel is null || _rosterValueLabel is null || _statusLabel is null)
        {
            return;
        }

        for (var row = 0; row < SkirmishState.BoardSize; row++)
        {
            for (var column = 0; column < SkirmishState.BoardSize; column++)
            {
                var position = new Vector2I(column, row);
                var button = _tileButtons[column, row];
                var unit = _skirmish.GetUnitAt(position);
                var isSelected = _skirmish.SelectedUnit?.Position == position;

                button.Text = unit is null
                    ? $"{(char)('A' + column)}{row + 1}"
                    : $"{unit.ShortCode}\n{(char)('A' + column)}{row + 1}";

                button.Modulate = ResolveTileColor(unit, isSelected, row, column);
                button.Disabled = false;
            }
        }

        _turnValueLabel.Text = _skirmish.WinningTeam is null
            ? $"{_skirmish.ActiveTeam} phase"
            : $"{_skirmish.WinningTeam} victory";
        _selectedValueLabel.Text = _skirmish.SelectedUnit is null
            ? "No unit selected"
            : $"{_skirmish.SelectedUnit.Team} {_skirmish.SelectedUnit.DisplayName} at {FormatTile(_skirmish.SelectedUnit.Position)}";
        _rosterValueLabel.Text = _skirmish.BuildRosterSummary();
        _statusLabel.Text = _skirmish.StatusMessage;
    }

    private static Color ResolveTileColor(UnitPiece? unit, bool isSelected, int row, int column)
    {
        if (isSelected)
        {
            return new Color("d9b56d");
        }

        if (unit is null)
        {
            return (row + column) % 2 == 0 ? new Color("304564") : new Color("1f3149");
        }

        return unit.Team == TeamSide.Dawn ? new Color("8ac6ff") : new Color("ff8ab0");
    }

    private static string FormatTile(Vector2I position)
    {
        return $"{(char)('A' + position.X)}{position.Y + 1}";
    }
}
