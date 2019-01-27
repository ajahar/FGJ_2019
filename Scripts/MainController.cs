using Godot;
using System;
using System.Collections.Generic;

public class MainController : Spatial
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";
	
	public static MainController I;

    public KinematicBody mothership;
    public static Random r = new Random();
    
    public List<ShipMain> enemies = new List<ShipMain>();
    public List<ShipMain> fighters = new List<ShipMain>();
    public List<ShipMain> selectedFighters = new List<ShipMain>();
    
    CPUParticles explosion;
    Timer timer;
    
    [Export]
    float startDelay = 30f;
    
    [Signal]
    public delegate void start();

    public override void _EnterTree()
    {
        I = this;

        mothership = GetNode("Mothership") as KinematicBody;
        var hp = mothership.GetNode("HP") as HP;

        hp.Connect("OnDeath", this, "OnMothershipDeath");
        
        fighters.Add(GetNode<ShipMain>("AllyFighter"));
        
        explosion = GetNode("Explosion") as CPUParticles;
        

    }
    
    public void OnGameStartTimer() {
    	EmitSignal("start");
    }

    public void OnMothershipDeath() 
    {
    	explosion.Emitting = true;
        mothership = null;
    }

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
		timer = new Timer();
        timer.OneShot = true;
        timer.Connect("timeout", this, "OnGameStartTimer");
        timer.WaitTime = startDelay;
        AddChild(timer);
        timer.Start();
    }


    public static float RandomFloat(float min, float max)
    {
        return min + (float)r.NextDouble() * (max - min);
    }

    public static Vector3 RandomPointOnSphere()
    {
    
        var randomAngle = RandomFloat(0, Mathf.Pi * 2);
        return new Vector3(Mathf.Cos(randomAngle), RandomFloat(-1, 1), Mathf.Sin(randomAngle));
    }
}
