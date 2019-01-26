using Godot;
using System;

public class CommandController : Node2D
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    [Export]
    Color UiColor;
    
    const float RAY_LENGTH = 500f;
    
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
			
			if (box.Area > 0.2f) {
					
				//TODO: Bandbox selection with proper ship list
				
				var ship = GetNode<Interceptor>("../Interceptor");
				var worldPos = ship.GetTranslation();
				var screenPos = GetViewport().GetCamera().UnprojectPosition(worldPos);
					
				if(box.HasPoint(screenPos)) {
					ship.selected = true;
				}
			} else {
				pointSelect();
			}
			
			box = new Rect2(0, 0, new Vector2(0, 0));
			Update();
	
		}
		
	}
	
	public void pointSelect() {		
		var pos = GetViewport().GetMousePosition();
		var from = GetViewport().GetCamera().ProjectRayOrigin(pos);
		var to = from + GetViewport().GetCamera().ProjectRayNormal(pos) * RAY_LENGTH;
		var space = GetViewport().GetWorld().DirectSpaceState;
		var selection = space.IntersectRay(from, to);
		
		// TODO: Maybe just send signal?
		
		if(selection != null) {
			foreach (var pair in selection) {
				if (pair.Key.Equals("collider")) {
					var ship = GetTree().GetRoot().GetNodeOrNull<Interceptor>("Root/" + pair.Value.ToString());
					if (ship != null) {
						
						ship.selected = true;
					}
				}
			}
		}
	}
	
	public override void _Draw()
	{
		DrawRect(box, UiColor, false);
	}
}
