using Godot;

public class UI : CanvasLayer
{
    private Label _clickToStart;
    private Label _death;
    private Timer _flashingTimer;
    private Label _title;

    public string DeathMessage = "You died!\nScore : ";

    public override void _Ready()
    {
        _title = GetNode<Label>("Title");
        _clickToStart = GetNode<Label>("ClickToStart");
        _death = GetNode<Label>("Death");

        _death.Text = DeathMessage;

        _flashingTimer = GetNode<Timer>("FlashingTimer");

        _flashingTimer.Connect("timeout", this, nameof(OnFlashTimerTimeout));

        ShowTitle();
    }

    public void StartGame()
    {
        HideAll();
    }

    private void ShowTitle()
    {
        _title.Visible = true;
        _clickToStart.Visible = true;
        _death.Visible = false;
        _flashingTimer.Start();
    }

    public void ShowDeathMessage(int score)
    {
        HideAll();
        _death.Text = DeathMessage + score;
        _death.Visible = true;
    }

    private void HideAll()
    {
        _title.Visible = false;
        _clickToStart.Visible = false;
        _death.Visible = false;
        _flashingTimer.Stop();
    }

    private void OnFlashTimerTimeout()
    {
        _clickToStart.Visible = !_clickToStart.Visible;
    }
}