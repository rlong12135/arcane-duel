using Godot;

namespace ArcaneDuel;

public partial class Main : Control
{
    private Label? _statusLabel;

    public override void _Ready()
    {
        _statusLabel = GetNode<Label>("Margin/Layout/Actions/StatusLabel");
        var newSkirmishButton = GetNode<Button>("Margin/Layout/Actions/NewSkirmishButton");
        newSkirmishButton.Pressed += OnNewSkirmishPressed;
    }

    private void OnNewSkirmishPressed()
    {
        if (_statusLabel is null)
        {
            return;
        }

        _statusLabel.Text = "Prototype hook fired. Next step: generate the tactical board state.";
    }
}
