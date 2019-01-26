using Godot;
using System;

public class ShipWeapons : Node
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";
    Spatial parent, target;
    HP targetHP;


    [Export]
    float damagePerSecond = 1;

    [Export]
    float shootDistance = 0;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        parent = GetParent() as Spatial;
    }

    public void OnTargetSet(Spatial target)
    {
        this.target = target;
        targetHP = target.GetNode("HP") as HP;
    }

    public override void _Process(float delta)
    {
        if (target != null)
        {
            if (parent.GlobalTransform.origin.DistanceTo(target.GlobalTransform.origin) < shootDistance)
            {
                targetHP.LoseHP(damagePerSecond * delta);
            }
        }
    }
}
