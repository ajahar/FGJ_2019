using Godot;
using System;

public class Debug : Node2D
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    static Vector3 start, end;
    static Vector3 start2, end2;
    static bool draw1, draw2;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        
    }

    public static void DrawLine(Vector3 startPoint, Vector3 endPoint)
    {
        start = startPoint;
        end = endPoint;
        draw1 = true;
    }
    
    public static void DrawRay(Vector3 startPoint, Vector3 ray)
    {
        start = startPoint;
        end = startPoint + ray;
        draw1 = true;
    }
    
    public static void DrawLine2(Vector3 startPoint, Vector3 endPoint)
    {
        start2 = startPoint;
        end2 = endPoint;
        draw2 = true;
    }
    
    public static void DrawRay2(Vector3 startPoint, Vector3 ray)
    {
        start2 = startPoint;
        end2 = startPoint + ray;
        draw2 = true;
    }

    public override void _Process(float delta)
    {
        Update();
    }

    public override void _Draw()
    {
    	var c = GetViewport().GetCamera();
        
        if (draw1)
        	DrawLine(c.UnprojectPosition(start), c.UnprojectPosition(end), Color.ColorN("red"));
        
        if (draw2)
        	DrawLine(c.UnprojectPosition(start2), c.UnprojectPosition(end2), Color.ColorN("blue"));

        draw1 = false;
        draw2 = false;
    }
}
