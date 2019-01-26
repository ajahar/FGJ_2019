using Godot;
using System;

public class CommandController : Node2D
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    [Export]
    Color UiColor;
    
    const float RAY_LENGTH = 1000f;
    
    Rect2 box;
    bool bandboxing = false;
    RichTextLabel shipList;
    CameraControl camera;
    
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    { 
    	box = new Rect2(0, 0, new Vector2(0, 0));
    	shipList = GetNode<RichTextLabel>("ShipList");
    	shipList.Text = "Ships";
    	camera = GetTree().GetRoot().GetNode<CameraControl>("Root/Gimbal/PitchGimbal/Camera");
    }
    
	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(float delta)
	{
		if (Input.IsActionJustPressed("ui_cancel")) {
			shipList.Text = "Ships";
		}
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
			box = box.Abs();

			if (box.Area > 1f) {
					
				//TODO: Bandbox selection with proper ship list
	
				var ship = GetNode<ShipMain>("../AllyFighter");
				var worldPos = ship.GetTranslation();
				var screenPos = GetViewport().GetCamera().UnprojectPosition(worldPos);

				if(box.HasPoint(screenPos)) {
					shipList.Text = ship.GetName();
					//ship.selected = true;
				}
			} else {
				var selectedShip = pointSelect();
				if (selectedShip != null) {
					shipList.Text = selectedShip.GetName();
				}
			}
			
			box = new Rect2(0, 0, new Vector2(0, 0));
			Update();
		}
		
		if(Input.IsActionPressed("mouse_action") && !camera.drag) {
			// TODO: Set target
			var targetShip = pointSelect();
			if (targetShip != null) {
				shipList.Text = "Attack: " + targetShip.GetName();
			}
		}
	}
	
	public ShipMain pointSelect() {		
		var pos = GetViewport().GetMousePosition();
		var from = GetViewport().GetCamera().ProjectRayOrigin(pos);
		var to = from + GetViewport().GetCamera().ProjectRayNormal(pos) * RAY_LENGTH;
		var space = GetViewport().GetWorld().DirectSpaceState;
		var selection = space.IntersectRay(from, to);
		
		if(selection != null) {
			foreach (var pair in selection) {
				if (pair.Key.Equals("collider")) {
					var ship = (ShipMain)pair.Value;
					return ship;
				}
			}
		}
		return null;
	}
	
	public override void _Draw()
	{
		DrawRect(box, UiColor, false);
	}
}
