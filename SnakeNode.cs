using Godot;

public class SnakeNode : Area
{
    [Signal]
    public delegate void FoodEaten();

    [Signal]
    public delegate void SelfCollision();

    private Spatial _body;
    private Vector3 _velocity = Vector3.Forward;
    public float LevitateBias = 0;
    public SnakeNode Next;
    public SnakeNode Prev;

    [Export] public float Speed = 10;
    public bool Stopped = false;
    public Vector3 TargetPosition;

    public bool IsHead => Prev == null;
    public bool IsTail => Next == null;

    public override void _Ready()
    {
        _body = GetNode<Spatial>("Body");

        Monitoring = IsHead;
        Monitorable = !IsHead;

        GD.Print("Monitoring: " + Monitoring);
        GD.Print("Monitorable: " + Monitorable);

        Connect("area_entered", this, nameof(OnAreaEntered));
    }

    // make body float

    public override void _PhysicsProcess(float delta)
    {
        if (Stopped) return;

        _body.Translation = new Vector3(0, HelperFunctions.SinusoidalLevitateMapping(150f, 0.5f, LevitateBias), 0);

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

    public void OnAreaEntered(Area area)
    {
        if (!IsHead) return;

        GD.Print("Area entered");
        if (area is SnakeNode)
        {
            if (area == Next) return; // ignore collision with next node
            GD.Print("Area is snake node");
            EmitSignal(nameof(SelfCollision));
        }
        else if (area is IFood food)
        {
            GD.Print("Area is food");
            EmitSignal(nameof(FoodEaten));
            food.OnFoodEaten();
        }
    }
}