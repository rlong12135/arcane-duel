using Godot;

namespace ArcaneDuel;

public partial class Main : Control
{
    private readonly TileView[,] _tiles = new TileView[SkirmishState.BoardSize, SkirmishState.BoardSize];
    private readonly SkirmishState _skirmish = new();

    private GridContainer? _boardGrid;
    private Label? _turnValueLabel;
    private Label? _selectedValueLabel;
    private RichTextLabel? _rosterValueLabel;
    private Label? _statusLabel;

    public override void _Ready()
    {
        _boardGrid = GetNode<GridContainer>("Margin/Layout/Content/BoardColumn/BoardFrame/BoardMargin/BoardGrid");
        _turnValueLabel = GetNode<Label>("Margin/Layout/Content/Sidebar/TurnPanel/TurnMargin/TurnBox/TurnValue");
        _selectedValueLabel = GetNode<Label>("Margin/Layout/Content/Sidebar/SelectedPanel/SelectedMargin/SelectedBox/SelectedValue");
        _rosterValueLabel = GetNode<RichTextLabel>("Margin/Layout/Content/Sidebar/RosterPanel/RosterMargin/RosterBox/RosterValue");
        _statusLabel = GetNode<Label>("Margin/Layout/Actions/StatusLabel");

        GetNode<Button>("Margin/Layout/Actions/NewSkirmishButton").Pressed += StartNewSkirmish;

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
                var tile = new TileView();
                tile.TilePosition = tilePosition;
                tile.TilePressed += OnTilePressed;
                _tiles[column, row] = tile;
                _boardGrid.AddChild(tile);
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
                var unit = _skirmish.GetUnitAt(position);
                var isSelected = _skirmish.SelectedUnit?.Position == position;
                _tiles[column, row].SetTileState(position, unit, isSelected);
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

    private static string FormatTile(Vector2I position)
    {
        return $"{(char)('A' + position.X)}{position.Y + 1}";
    }
}
