using Godot;

public class Particle : Spatial
{
    private Timer _elapseTimer;
    public float ElapseTime = 1;
    public float Speed = 10;
    public Vector3 Velocity = Vector3.Up;

    public override void _Ready()
    {
        _elapseTimer = GetNode<Timer>("ElapseTimer");
        _elapseTimer.WaitTime = ElapseTime;
        _elapseTimer.OneShot = true;
        _elapseTimer.Start();


        _elapseTimer.Connect("timeout", this, nameof(OnElapseTimerTimeout));
    }

    public override void _Process(float delta)
    {
        Translation += Velocity * Speed * delta;
    }

    private void OnElapseTimerTimeout()
    {
        QueueFree();
    }
}