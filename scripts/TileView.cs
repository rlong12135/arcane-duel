using Godot;

namespace ArcaneDuel;

public partial class TileView : Control
{
    private UnitPiece? _unit;
    private bool _isSelected;
    private bool _isHovered;
    private Texture2D? _dawnTexture;
    private Texture2D? _duskTexture;
    private Color _baseColor = new("425973");

    public TileView()
    {
        CustomMinimumSize = new Vector2(96, 96);
        MouseFilter = MouseFilterEnum.Stop;
        FocusMode = FocusModeEnum.None;

        MouseEntered += () =>
        {
            _isHovered = true;
            QueueRedraw();
        };

        MouseExited += () =>
        {
            _isHovered = false;
            QueueRedraw();
        };
    }

    [Signal]
    public delegate void TilePressedEventHandler(Vector2I tilePosition);

    public Vector2I TilePosition { get; set; }

    public override void _GuiInput(InputEvent @event)
    {
        if (@event is InputEventMouseButton mouseButton &&
            mouseButton.Pressed &&
            mouseButton.ButtonIndex == MouseButton.Left)
        {
            EmitSignal(SignalName.TilePressed, TilePosition);
            AcceptEvent();
        }
    }

    public override void _Draw()
    {
        var rect = new Rect2(Vector2.Zero, Size);
        var background = _isHovered ? _baseColor.Lightened(0.18f) : _baseColor;
        DrawRect(rect, background);
        DrawRect(rect, background.Darkened(0.28f), false, 2f);

        var font = ThemeDB.FallbackFont;
        var fontSize = ThemeDB.FallbackFontSize;

        if (_unit is null)
        {
            DrawString(font, new Vector2(8, 20), "Tile", HorizontalAlignment.Left, -1, fontSize - 1, new Color("eaf3ff"));
            DrawString(font, new Vector2(Size.X / 2f, Size.Y - 10), $"{(char)('A' + TilePosition.X)}{TilePosition.Y + 1}", HorizontalAlignment.Center, -1, fontSize + 4, new Color("ffffff"));
            return;
        }

        var markerColor = _unit.Team == TeamSide.Dawn ? new Color("dff1ff") : new Color("ffd7e6");
        DrawCircle(new Vector2(Size.X / 2f, Size.Y / 2f), 23f, markerColor);

        var texture = _unit.Team == TeamSide.Dawn ? _dawnTexture : _duskTexture;
        if (texture is not null)
        {
            var texturePosition = new Vector2((Size.X - 48f) / 2f, (Size.Y - 48f) / 2f - 3f);
            DrawTextureRect(texture, new Rect2(texturePosition, new Vector2(48f, 48f)), false);
        }
        else
        {
            DrawCircle(new Vector2(Size.X / 2f, Size.Y / 2f), 16f, _unit.Team == TeamSide.Dawn ? new Color("4ea8ff") : new Color("ff6f9d"));
        }

        DrawString(font, new Vector2(8, 20), _unit.ShortCode, HorizontalAlignment.Left, -1, fontSize - 1, new Color("08111f"));
        DrawString(font, new Vector2(Size.X / 2f, Size.Y - 10), _unit.DisplayName, HorizontalAlignment.Center, -1, fontSize, new Color("08111f"));

        if (_isSelected)
        {
            DrawRect(new Rect2(3, 3, Size.X - 6, Size.Y - 6), new Color("fff4c2"), false, 3f);
        }
    }

    public void SetTileState(
        Vector2I boardPosition,
        UnitPiece? unit,
        bool isSelected,
        Texture2D? dawnTexture,
        Texture2D? duskTexture)
    {
        TilePosition = boardPosition;
        _unit = unit;
        _isSelected = isSelected;
        _dawnTexture = dawnTexture;
        _duskTexture = duskTexture;
        _baseColor = ResolveBaseColor(unit, isSelected, boardPosition);
        QueueRedraw();
    }

    private static Color ResolveBaseColor(UnitPiece? unit, bool isSelected, Vector2I boardPosition)
    {
        if (isSelected)
        {
            return new Color("f4d778");
        }

        if (unit is null)
        {
            return (boardPosition.X + boardPosition.Y) % 2 == 0
                ? new Color("556f90")
                : new Color("425973");
        }

        return unit.Team == TeamSide.Dawn
            ? new Color("9bd6ff")
            : new Color("ff9dc0");
    }
}
