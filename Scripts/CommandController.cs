using Godot;
using System;

public class CommandController : Node2D
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    [Export]
    Color UiColor;
    
    Rect2 box;
    bool bandboxing = false;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
    	box = new Rect2(0, 0, new Vector2(0, 0));
    }
    
	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(float delta)
	{
		if (Input.IsActionJustPressed("mouse_select")) {
			bandboxing = true;
			box.Position = GetViewport().GetMousePosition();
			Update();
		}
		if (Input.IsActionPressed("mouse_select")) {
			box.Size = GetViewport().GetMousePosition() - box.Position;
			Update();
		}
		if (Input.IsActionJustReleased("mouse_select")) {
			bandboxing = false;
			box = new Rect2(0, 0, new Vector2(0, 0));
			Update();
		}
		
	}
	
	public override void _Draw()
	{
		DrawRect(box, UiColor, false);
	}
}
