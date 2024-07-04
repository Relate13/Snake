using Godot;

public class Snake : Spatial
{
    private Timer _updateTimer;

    public SnakeNode SnakeHead;
    public SnakeNode SnakeTail;
    
    [Export] public PackedScene SnakeNodeScene;
    [Export] public float UpdateInterval = 0.1f;

    public override void _Ready()
    {
        _updateTimer = GetNode<Timer>("UpdateTimer");
        _updateTimer.WaitTime = UpdateInterval;
        _updateTimer.Start();

        var head = SnakeNodeScene.Instance() as SnakeNode;
        
        // Log error if head is null
        if(head == null) GD.PrintErr("Head is null");
        
        head.Translation = new Vector3(0, 1, 0);
        AddChild(head);

        SnakeHead = head;
        SnakeTail = head;

        _updateTimer.Connect("timeout", this, nameof(OnUpdateTimerTimeout));
    }
    
    public void AddNode()
    {
        var node = SnakeNodeScene.Instance() as SnakeNode;
        node.Translation = SnakeTail.Translation;
        node.Prev = SnakeTail;
        node.TargetPosition = SnakeTail.Translation;
        SnakeTail.Next = node;
        SnakeTail = node;
        AddChild(node);
    }

    public override void _Process(float delta)
    {
        if (Input.IsActionJustPressed("ui_select")) AddNode();
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
}