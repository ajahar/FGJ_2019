using Godot;
using System;

public class ShipWeapons : Node
{
    [Signal]
    public delegate void OnShoot(Spatial target);
    
    [Signal]
    public delegate void OnShootEnd();

    KinematicBody parent, target;
    HP targetHP;

    [Export]
    float damagePerSecond = 1;

    [Export]
    float shootDistance = 0;

    bool shooting = false;

    public override void _Ready()
    {
        parent = GetParent() as KinematicBody;
    }

    public void OnTargetSet(KinematicBody target)
    {
        this.target = target;
        targetHP = target.GetNode("HP") as HP;

        targetHP.Connect("OnDeath", this, "OnTargetDeath");
    }

    public void OnTargetDeath() 
    {
        target = null;
        shooting = false;
        EmitSignal("OnShootEnd");
    }

    public override void _Process(float delta)
    {
        if (target != null)
        {
            if (parent.Translation.DistanceTo(target.Translation) < shootDistance)
            {
                var dot = parent.Transform.basis.z.Dot((parent.Translation - target.Translation).Normalized());

                if (dot > 0.9f)
                {
					EmitSignal("OnShoot", target);
					
                    targetHP.LoseHP(damagePerSecond * delta);
                    //Debug.DrawLine(parent.Translation, target.Translation);

                    shooting = true;
                    
                }
                else if (shooting)
                {
                    shooting = false;
                    EmitSignal("OnShootEnd");
                }
            }
        }
    }
}
