using Godot;
using System;

public class MainController : Spatial
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";
	
	public static MainController I;

    public KinematicBody mothership;
    public static Random r = new Random();

    public override void _EnterTree()
    {
        I = this;

        mothership = GetNode("Mothership") as KinematicBody;
        var hp = mothership.GetNode("HP") as HP;

        hp.Connect("OnDeath", this, "OnMothershipDeath");
    }

    public void OnMothershipDeath() 
    {
        mothership = null;
    }

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {

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
