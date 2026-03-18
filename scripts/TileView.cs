using Godot;

namespace ArcaneDuel;

public partial class TileView : Control
{
    private UnitPiece? _unit;
    private bool _isSelected;
    private bool _isHovered;
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

        DrawUnitGlyph(_unit, new Vector2(Size.X / 2f, Size.Y / 2f));

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
        bool isSelected)
    {
        TilePosition = boardPosition;
        _unit = unit;
        _isSelected = isSelected;
        _baseColor = ResolveBaseColor(unit, isSelected, boardPosition);
        QueueRedraw();
    }

    private void DrawUnitGlyph(UnitPiece unit, Vector2 center)
    {
        var ink = new Color("111827");
        switch (unit.DisplayName)
        {
            case "Guardian":
                DrawShield(center, ink);
                break;
            case "Archer":
                DrawBow(center, ink);
                break;
            case "Mage":
                DrawMage(center, ink);
                break;
            case "Vanguard":
                DrawVanguard(center, ink);
                break;
            default:
                DrawCircle(center, 16f, ink);
                break;
        }
    }

    private void DrawShield(Vector2 center, Color ink)
    {
        Vector2[] points =
        [
            center + new Vector2(0, -18),
            center + new Vector2(16, -11),
            center + new Vector2(14, 6),
            center + new Vector2(0, 18),
            center + new Vector2(-14, 6),
            center + new Vector2(-16, -11)
        ];
        DrawPolygon(points, RepeatColor(ink, points.Length));
        DrawPolyline(points.Append(points[0]).ToArray(), new Color("ffffff"), 2f);
    }

    private void DrawBow(Vector2 center, Color ink)
    {
        DrawArc(center + new Vector2(-6, 0), 18f, -1.1f, 1.1f, 24, ink, 4f);
        DrawLine(center + new Vector2(-6, -18), center + new Vector2(-6, 18), ink, 2f);
        DrawLine(center + new Vector2(-2, 0), center + new Vector2(18, 0), ink, 4f);
        Vector2[] arrow =
        [
            center + new Vector2(18, 0),
            center + new Vector2(8, -6),
            center + new Vector2(8, 6)
        ];
        DrawPolygon(arrow, RepeatColor(ink, arrow.Length));
    }

    private void DrawMage(Vector2 center, Color ink)
    {
        DrawCircle(center + new Vector2(10, -18), 8f, ink);
        DrawLine(center + new Vector2(-6, 18), center + new Vector2(0, -10), ink, 5f);
        DrawLine(center + new Vector2(-12, 14), center + new Vector2(6, 14), ink, 5f);
        DrawStar(center + new Vector2(-10, -6), 9f, 4f, 5, ink);
    }

    private void DrawVanguard(Vector2 center, Color ink)
    {
        DrawLine(center + new Vector2(-14, 14), center + new Vector2(10, -10), ink, 6f);
        Vector2[] blade =
        [
            center + new Vector2(10, -10),
            center + new Vector2(18, -18),
            center + new Vector2(22, -6)
        ];
        DrawPolygon(blade, RepeatColor(ink, blade.Length));
        DrawLine(center + new Vector2(-18, 18), center + new Vector2(-10, 10), ink, 4f);
    }

    private void DrawStar(Vector2 center, float outerRadius, float innerRadius, int points, Color color)
    {
        var vertices = new Vector2[points * 2];
        for (var i = 0; i < vertices.Length; i++)
        {
            var angle = -Mathf.Pi / 2f + i * Mathf.Pi / points;
            var radius = i % 2 == 0 ? outerRadius : innerRadius;
            vertices[i] = center + new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * radius;
        }

        DrawPolygon(vertices, RepeatColor(color, vertices.Length));
    }

    private static Color[] RepeatColor(Color color, int count)
    {
        var colors = new Color[count];
        for (var i = 0; i < count; i++)
        {
            colors[i] = color;
        }

        return colors;
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
