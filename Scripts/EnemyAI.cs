using Godot;
using System;

public class EnemyAI : Node
{
    [Export]
    float retargetFlyDuration = 2f;
    [Export]
    float retargetDistance = 20f;

    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";
    ShipMove movement;
    Spatial target;

    bool retargetting = false;
    Vector3 retargetPosition;
    float retargettingTimer;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        movement = GetParent() as ShipMove;
		GD.Print("ASD2: ");
        //Mothership is initial target, can change to fighters if hit?
        SetTarget(MainController.I.mothership);
    }

    private void SetTarget(Spatial target)
    {
        GD.Print("ASD: " + target);
        this.target = target;
    }

    public override void _Process(float delta)
    {
        if (retargettingTimer > OS.GetTicksMsec())
        {

        }
        else
        {
            movement.MoveTo(target.GlobalTransform.origin);

            if (movement.GlobalTransform.origin.DistanceTo(target.GlobalTransform.origin) < retargetDistance)
            {//Fly to random direction for a while before targeting again.
                retargettingTimer = OS.GetTicksMsec() + retargetFlyDuration;

                var fromTargetDir = movement.GlobalTransform.origin - target.GlobalTransform.origin;

                var xQ = new Quat(Vector3.Up, MainController.RandomFloat(-45, 45));
                var randomDir = (xQ * fromTargetDir).GetEuler();

                randomDir = fromTargetDir;

                retargetPosition = movement.GlobalTransform.origin + randomDir * 2;
                movement.MoveTo(retargetPosition);
            }
        }
    }
}
