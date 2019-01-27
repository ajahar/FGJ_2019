using Godot;
using System;

public class ShipAI : Node
{
    [Signal]
    public delegate void OnTargetSet(KinematicBody target);

    [Export]
    float retargetFlyDuration = 2f;
    [Export]
    float retargetDistance = 20f;

    ShipMain main;
    Spatial target;

    Vector3 retargetPosition;
    float retargettingTimer;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        main = GetParent() as ShipMain;
    }

    public void SetTarget(KinematicBody target)
    {
        this.target = target;
        EmitSignal("OnTargetSet", target);
        var targetHP = target.GetNode("HP") as HP;

        targetHP.Connect("OnDeath", this, "OnTargetDeath");
    }
    
    public Vector3 GetTarget() {
    	if (target != null) {
    		return target.GlobalTransform.origin;
    	}
    	return Vector3.Zero;
    }

    public void OnTargetDeath() 
    {
        target = null;
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

                var fromTargetDir = main.Translation - target.Translation;
                Debug.DrawRay(target.Translation, fromTargetDir);

                var xQ = new Quat(Vector3.Up, MainController.RandomFloat(-1, 1));
                var randomDir =  xQ.Xform(fromTargetDir);

                retargetPosition = main.GlobalTransform.origin + randomDir * 200;
                main.MoveTo(retargetPosition);
            }
        }
    }
}
