using Godot;

public class HelperFunctions
{
    public static float SinusoidalLevitateMapping(float speed = 250.0f, float amplitude = 0.3f, float bias = 0)
    {
        return (Mathf.Sin((Time.GetTicksMsec() + bias) / speed) + 1) * amplitude / 2;
    }
}