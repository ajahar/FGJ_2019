using Godot;
using System;

public class Interceptor : Spatial
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";
    
    public bool selected = false;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
    	// Input connection test
    	//Connect("input_event", this, "clicked");
    }
    
    public override void _Input(InputEvent @event) {
    	
    }
    
    public void clicked(Camera camera, InputEvent @event, Vector3 clickPosition, Vector3 clickNormal, int shapeIdx) {
		if(@event.IsActionReleased("mouse_select")) {
    	//	GD.Print("Selected " + GetName());
		}
    }

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(float delta)
	{
		if (selected) {
			GD.Print("jee");
		}
	}
}
