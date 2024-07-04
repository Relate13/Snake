using Godot;
using System;

public class Main : Node
{
    [Export] PackedScene PlayerNode;
    
    public override void _Ready()
    {
        var head = PlayerNode.Instance() as Player;
        head.Translation = new Vector3(0, 1, 0);
        AddChild(head);

        int count = 100;
        
        var last = head;
        
        for (int i = 0; i < count; i++)
        {
            var node = PlayerNode.Instance() as Player;
            node.Translation = new Vector3(0, 0, i + 1);
            node.FollowingTarget = last;
            last = node;
            AddChild(node);
        }
        
    }
}
