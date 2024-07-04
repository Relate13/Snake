using Godot;

public class Player : Spatial
{
    private Vector3 _velocity;

    private Spatial Body;
    public float FollowingDistance = 1.3f;
    public Player FollowingTarget;

    [Export] public float Speed = 20;

    public override void _Ready()
    {
        Body = GetNode<Spatial>("BodyCenter");
    }

    // make body float

    public override void _PhysicsProcess(float delta)
    {
        Body.Translation = new Vector3(0, Mathf.Sin(OS.GetTicksMsec() / 250.0f) * 0.3f, 0);

        // control player
        if (FollowingTarget == null) _PlayerControl(delta);
        else _FollowTarget(delta);
    }

    private void _PlayerControl(float delta)
    {
        if (Input.IsActionPressed("ui_up")) _velocity.x += 1;
        if (Input.IsActionPressed("ui_down")) _velocity.x -= 1;
        if (Input.IsActionPressed("ui_left")) _velocity.z -= 1;
        if (Input.IsActionPressed("ui_right")) _velocity.z += 1;

        _velocity = _velocity.Normalized() * Speed;

        Translation += _velocity * delta;

        Body.LookAt(Translation + _velocity * 10f, Vector3.Up);
    }

    private void _FollowTarget(float delta)
    {
        if (FollowingTarget == null) return;

        if (FollowingTarget.Translation.DistanceTo(Translation) < FollowingDistance) return;

        var targetPos = FollowingTarget.Translation;
        var direction = targetPos - Translation;
        _velocity = direction.Normalized() * Speed;

        Translation += _velocity * delta;

        Body.LookAt(targetPos, Vector3.Up);
    }
}