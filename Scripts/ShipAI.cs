using Godot;
using System;

public class ShipAI : Node
{
    [Signal]
    public delegate void OnTargetSet(Spatial target);

    [Export]
    float retargetFlyDuration = 2f;
    [Export]
    float retargetDistance = 20f;

    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";
    ShipMain main;
    Spatial target;

    Vector3 retargetPosition;
    float retargettingTimer;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        main = GetParent() as ShipMain;
    }

    public void SetTarget(Spatial target)
    {
        this.target = target;
        EmitSignal("OnTargetSet", target);
    }

    public override void _Process(float delta)
    {
        if (retargettingTimer > OS.GetTicksMsec())
        {

        }
        else if (target != null)
        {
            main.MoveTo(target.GlobalTransform.origin);

            if (main.GlobalTransform.origin.DistanceTo(target.GlobalTransform.origin) < retargetDistance)
            {//Fly to random direction for a while before targeting again.
                retargettingTimer = OS.GetTicksMsec() + retargetFlyDuration;

                var fromTargetDir = main.GlobalTransform.origin - target.GlobalTransform.origin;

                var xQ = new Quat(Vector3.Up, MainController.RandomFloat(-45, 45));
                var randomDir = (xQ * fromTargetDir).GetEuler();

                randomDir = fromTargetDir;

                retargetPosition = main.GlobalTransform.origin + randomDir * 2;
                main.MoveTo(retargetPosition);
            }
        }
    }
}
