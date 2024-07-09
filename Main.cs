using Godot;

public class Main : Node
{
    [Export] private PackedScene CommonFoodScene;

    [Export] public int InitialFoodCount = 10;

    [Export] public Vector2 MapSize = new Vector2(60, 70);

    public bool OnGoing;

    public Snake SnakeController;
    public Spatial Foods;
    public UI UIController;

    public override void _Ready()
    {
        GD.Randomize();
        
        UIController = GetNode<UI>("UI");
        Foods = GetNode<Spatial>("Foods");
        SnakeController = GetNode<Snake>("Snake");
        SnakeController.GameBounds = MapSize;

        SnakeController.Connect(nameof(Snake.FoodEaten), this, nameof(OnFoodEaten));
        SnakeController.Connect(nameof(Snake.SnakeDie), this, nameof(OnSnakeDeath));
    }

    public override void _Process(float delta)
    {
        if (!OnGoing && Input.IsActionJustPressed("ui_select")) GameStart();
    }

    public void SummonFood()
    {
        var food = CommonFoodScene.Instance() as CommonFood;
        food.Translation = new Vector3(
            (float)GD.RandRange(0, MapSize.x) - MapSize.x / 2,
            0,
            (float)GD.RandRange(0, MapSize.y) - MapSize.y / 2
        );

        GD.Print("Summoning food at: " + food.Translation);

        Foods.AddChild(food);
    }

    public void GameStart()
    {
        foreach (Node food in Foods.GetChildren()) { food.QueueFree(); }
        for (var i = 0; i < InitialFoodCount; i++) SummonFood();

        SnakeController.ResetSnake();
        UIController.StartGame();
        OnGoing = true;
    }

    public void OnFoodEaten()
    {
        SummonFood();
    }

    public void OnSnakeDeath(int score)
    {
        UIController.ShowDeathMessage(score);
        OnGoing = false;
    }
}