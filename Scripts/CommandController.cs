using Godot;
using System;

public class CommandController : Node2D
{

    [Export]
    Color UiColor;
    [Export]
    Color CommandColor;
    
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
				MainController.I.selectedFighters.Clear();
				shipList.Text = "";
	
				foreach (var fighter in MainController.I.fighters) {
					var worldPos = fighter.GetTranslation();
					var screenPos = GetViewport().GetCamera().UnprojectPosition(worldPos);
	
					if(box.HasPoint(screenPos)) {		
						MainController.I.selectedFighters.Add(fighter);
						shipList.Text = shipList.Text + fighter.Name + "\n";
					}
				}		
			} else {
				MainController.I.selectedFighters.Clear();
				shipList.Text = "";
				var selectedShip = pointSelect();
				if (selectedShip != null && selectedShip.GetName().Contains("Ally")) {
					MainController.I.selectedFighters.Add(selectedShip);
					shipList.Text = selectedShip.GetName();
				} else if (selectedShip != null && selectedShip.GetName().Contains("Mothership")) {
					shipList.Text = selectedShip.GetName() + "\nHP: " + selectedShip.hp.GetHP();
				} else if (selectedShip != null && selectedShip.GetName().Contains("Enemy")) {
					shipList.Text = selectedShip.GetName() + "\nHP: " + selectedShip.hp.GetHP();
				}
			}
			
			box = new Rect2(0, 0, new Vector2(0, 0));
			Update();
		}
		
		if(Input.IsActionJustReleased("mouse_action") && !camera.drag) {
			var targetShip = pointSelect();
			if (targetShip != null && MainController.I.enemies.Contains(targetShip)) {
				shipList.Text = "";
				foreach (var fighter in MainController.I.selectedFighters) {
					fighter.ai.SetTarget(targetShip);
					shipList.Text = shipList.Text + fighter.Name + "\n";
				}
				shipList.Text = shipList.Text + "Attack: " + targetShip.GetName();
			}
		}
		
		//if (MainController.I.selectedFighters.Count > 0) {
			Update();
		//}
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
		
		foreach(var fighter in MainController.I.selectedFighters) {
			var worldPos = fighter.GetTranslation();
			var screenPos = GetViewport().GetCamera().UnprojectPosition(worldPos);
			
			DrawLine(screenPos + Vector2.Up * 20,screenPos + (Vector2.Left * 20) + (Vector2.Down * 20), UiColor);
			DrawLine(screenPos + (Vector2.Left * 20) + (Vector2.Down * 20),screenPos + (Vector2.Right * 20) + (Vector2.Down * 20), UiColor);
			DrawLine(screenPos + (Vector2.Right * 20) +(Vector2.Down * 20),screenPos + Vector2.Up * 20, UiColor);
			
//			if (fighter.ai.GetTarget() != Vector3.Zero) {
//				var targetScreenPos = GetViewport().GetCamera().UnprojectPosition(fighter.ai.GetTarget());
//				DrawLine(screenPos, targetScreenPos, CommandColor);
//			
//				DrawLine(targetScreenPos + Vector2.Down * 20,targetScreenPos + (Vector2.Left * 10) + (Vector2.Up * 10), CommandColor);
//				DrawLine(targetScreenPos + (Vector2.Left * 10)+ (Vector2.Up * 10) ,targetScreenPos + (Vector2.Right * 10) + (Vector2.Up * 10), CommandColor);
//				DrawLine(targetScreenPos + (Vector2.Right * 10) + (Vector2.Up * 10),targetScreenPos + Vector2.Down * 20, CommandColor);			
//			}
		}
		
		foreach(var fighter in MainController.I.fighters) {
			var worldPos = fighter.GetTranslation();
			var screenPos = GetViewport().GetCamera().UnprojectPosition(worldPos);
			if (fighter.ai.GetTarget() != Vector3.Zero) {
				var targetScreenPos = GetViewport().GetCamera().UnprojectPosition(fighter.ai.GetTarget());
				DrawLine(screenPos, targetScreenPos, CommandColor);
			
				DrawLine(targetScreenPos + Vector2.Down * 20,targetScreenPos + (Vector2.Left * 10) + (Vector2.Up * 10), CommandColor);
				DrawLine(targetScreenPos + (Vector2.Left * 10)+ (Vector2.Up * 10) ,targetScreenPos + (Vector2.Right * 10) + (Vector2.Up * 10), CommandColor);
				DrawLine(targetScreenPos + (Vector2.Right * 10) + (Vector2.Up * 10),targetScreenPos + Vector2.Down * 20, CommandColor);			
			}
		}
		
		DrawRect(box, UiColor, false);
	}
}
