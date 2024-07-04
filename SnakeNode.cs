using Godot;

public class SnakeNode : Spatial
{
    private Spatial _body;
    private Vector3 _velocity = Vector3.Forward;
    public SnakeNode Next;
    public SnakeNode Prev;

    [Export] public float Speed = 10;
    public Vector3 TargetPosition;

    public override void _Ready()
    {
        _body = GetNode<Spatial>("BodyCenter");
    }

    // make body float

    public override void _PhysicsProcess(float delta)
    {
        _body.Translation = new Vector3(0, Mathf.Sin(OS.GetTicksMsec() / 250.0f) * 0.3f, 0);

        // control player
        if (Prev == null) _PlayerControl(delta);
        else _MoveTowardsTarget(delta);
    }

    private void _PlayerControl(float delta)
    {
        if (Input.IsActionPressed("ui_up")) _velocity.x += 1;
        if (Input.IsActionPressed("ui_down")) _velocity.x -= 1;
        if (Input.IsActionPressed("ui_left")) _velocity.z -= 1;
        if (Input.IsActionPressed("ui_right")) _velocity.z += 1;

        _velocity = _velocity.Normalized() * Speed;

        Translation += _velocity * delta;

        _body.LookAt(Translation + _velocity * 10f, Vector3.Up);
    }

    private void _MoveTowardsTarget(float delta)
    {
        if (Prev == null) return;

        var direction = (TargetPosition - Translation).Normalized();
        Translation += direction * Speed * delta;
    }
}