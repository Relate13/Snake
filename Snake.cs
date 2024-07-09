using Godot;

public class Snake : Spatial
{
    [Signal]
    public delegate void FoodEaten();

    [Signal]
    public delegate int SnakeDie();

    private Timer _deathAnimationTimer;

    private int _snakeLength;
    private Timer _updateTimer;

    public bool Dead = true;
    [Export] public float DeathAnimationInterval = 0.1f;
    public Vector2 GameBounds = new Vector2(60, 70);
    public float LeviatateBiasIncrement = 50f;

    public SnakeNode SnakeHead;

    [Export] public PackedScene SnakeNodeScene;
    public SnakeNode SnakeTail;
    [Export] public float UpdateInterval = 0.1f;

    public override void _Ready()
    {
        _updateTimer = GetNode<Timer>("UpdateTimer");
        _updateTimer.WaitTime = UpdateInterval;
        _updateTimer.Start();

        _deathAnimationTimer = GetNode<Timer>("DeathAnimationTimer");
        _updateTimer.WaitTime = DeathAnimationInterval;

        _updateTimer.Connect("timeout", this, nameof(OnUpdateTimerTimeout));
        _deathAnimationTimer.Connect("timeout", this, nameof(OnDeathAnimationTimer));
    }

    public override void _Process(float delta)
    {
        //check out of bounds
        if (Dead) return;

        if (Mathf.Abs(SnakeHead.Translation.x) > GameBounds.x / 2 ||
            Mathf.Abs(SnakeHead.Translation.z) > GameBounds.y / 2)
        {
            GD.Print("Game Over : Out of bounds");
            Die();
        }
    }

    public void AddNode()
    {
        var node = SnakeNodeScene.Instance() as SnakeNode;
        node.Translation = SnakeTail.IsHead
            ? SnakeTail.Translation
            : 2 * SnakeTail.Translation - SnakeTail.Prev.Translation;
        node.Prev = SnakeTail;
        node.TargetPosition = SnakeTail.Translation;
        node.LevitateBias = SnakeTail.LevitateBias + LeviatateBiasIncrement;

        SnakeTail.Next = node;
        SnakeTail = node;
        AddChild(node);
        _snakeLength++;

        EmitSignal(nameof(FoodEaten));
    }

    public void OnUpdateTimerTimeout()
    {
        if (SnakeHead == null) return;

        if (SnakeHead.Prev != null) return;

        var currentPos = SnakeHead.Translation;
        var current = SnakeHead;

        while (current.Next != null)
        {
            var next = current.Next;
            (next.TargetPosition, currentPos) = (currentPos, next.TargetPosition);

            current = next;
            current.Speed = current.Translation.DistanceTo(current.TargetPosition) / UpdateInterval;
        }
    }

    public void OnSnakeNodeSelfCollision()
    {
        GD.Print("Game Over : Self Collision");
        Die();
    }

    private void StopSnake()
    {
        _updateTimer.Stop();
        var curr = SnakeHead;
        while (curr != null)
        {
            curr.Stopped = true;
            curr = curr.Next;
        }
    }

    private void Die()
    {
        Dead = true;
        StopSnake();
        _deathAnimationTimer.Start();
    }

    private void OnDeathAnimationTimer()
    {
        // Implement death animation
        if (SnakeHead == null)
        {
            SnakeTail = null;
            _deathAnimationTimer.Stop();

            EmitSignal(nameof(SnakeDie), _snakeLength);
            return;
        }

        var curr = SnakeHead;
        SnakeHead = SnakeHead.Next;
        curr.SummonDeathEffect();
        curr.QueueFree();
    }

    public void ResetSnake()
    {
        Translation = new Vector3(0, 0, 0);
        while (SnakeHead != null)
        {
            SnakeHead.QueueFree();
            SnakeHead = SnakeHead.Next;
        }

        SnakeHead = SnakeNodeScene.Instance() as SnakeNode;
        SnakeTail = SnakeHead;
        SnakeHead.Translation = new Vector3(0, 0, 0);

        SnakeHead.Connect(nameof(SnakeNode.SelfCollision), this, nameof(OnSnakeNodeSelfCollision));
        SnakeHead.Connect(nameof(SnakeNode.FoodEaten), this, nameof(AddNode));

        _snakeLength = 1;

        AddChild(SnakeHead);
        Dead = false;
        _updateTimer.Start();
    }
}