using Godot;

public class Main : Node
{
    [Export] private PackedScene CommonFoodScene;

    [Export] public Vector2 MapSize = new Vector2(60, 70);

    public Snake SnakeController;

    public override void _Ready()
    {
        SnakeController = GetNode<Snake>("Snake");
        SnakeController.GameBounds = MapSize;
    }

    public override void _Process(float delta)
    {
        if (Input.IsActionJustPressed("ui_select"))
            SummonFood();
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

        AddChild(food);
    }
}