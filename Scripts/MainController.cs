using Godot;
using System;

public class MainController : Spatial
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";
	
	public static MainController I;

    public Spatial mothership;
    public static Random r = new Random();

    public override void _EnterTree()
    {
        I = this;
		GD.Print("ASD: "+mothership);

        mothership = GetNode("Mothership") as Spatial;
		
		GD.Print("ASD: "+mothership);
    }

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {

    }


    public static float RandomFloat(float min, float max)
    {
        return min + (float)r.NextDouble() * (max - min);
    }
}
