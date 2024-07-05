using Godot;

public class CommonFood : Area, IFood
{
    public Spatial Body;

    public void OnFoodEaten()
    {
        GD.Print("Common Food eaten");
        QueueFree();
    }

    public override void _Ready()
    {
        Body = GetNode<Spatial>("BodyCenter");
    }

    public override void _PhysicsProcess(float delta)
    {
        Body.Translation = new Vector3(0, HelperFunctions.SinusoidalLevitateMapping(), 0);
    }
}