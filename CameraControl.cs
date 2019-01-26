using Godot;
using System;

public class CameraControl : InterpolatedCamera
{
    const float SENSITIVITY = .3f;
    
    bool drag = false;
    bool freeCam = false;
    Spatial pitchGimbal;
    Spatial yawGimbal;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	    pitchGimbal = GetTree().GetRoot().GetNode<Spatial>("Root/Gimbal/PitchGimbal");
	    yawGimbal = GetTree().GetRoot().GetNode<Spatial>("Root/Gimbal");
	}
	
	public override void _Input(InputEvent @event) {
		if (@event is InputEventMouseButton) {
			var buttonEvent = (InputEventMouseButton)@event;
			if(buttonEvent.IsActionPressed("mouse_action")) {
				drag = true;
				Input.SetMouseMode(Input.MouseMode.Captured);
			}
			if(buttonEvent.IsActionReleased("mouse_action")) {
				drag = false;
				Input.SetMouseMode(Input.MouseMode.Visible);
			}
    	}
    	if (@event is InputEventMouseMotion && drag){
    		var motionEvent = (InputEventMouseMotion)@event;

    		yawGimbal.RotateY(Mathf.Deg2Rad(motionEvent.Relative.x * SENSITIVITY));
    		pitchGimbal.RotateX(Mathf.Deg2Rad(motionEvent.Relative.y * SENSITIVITY));
    	}
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(float delta)
	{
	   
        if (Input.IsActionJustPressed("camera_focus")) {
        	freeCam = !freeCam;
        	if (!freeCam) {
			
        		// TODO: Move camera to focused ship
        		
//        		tween_node.interpolate_property(node_to_rotate, "transform",
//  				transform,
//  				transform.looking_at(target.translation,Vector3(0,1,0)), #if up
//  				time,trans_type,ease_type
//				)
        		
        		SetRotation(Vector3.Zero);
        		SetTranslation(new Vector3(0, 0, 5.5f));
        	}
        }
		
        if (freeCam) {
	        if (Input.IsActionPressed("ui_up"))
	        {
	        	Translate(Vector3.Forward * 10 * delta);
	        }
	        if (Input.IsActionPressed("ui_down"))
	        {
	        	Translate(Vector3.Back * 10 * delta);
	        }
			if (Input.IsActionPressed("ui_right"))
	        {
				Translate(Vector3.Right * 10 * delta);
	        }
	        if (Input.IsActionPressed("ui_left"))
	        {
	        	Translate(Vector3.Left * 10 * delta);
	        }
	        if (Input.IsActionPressed("ui_page_up"))
	        {
	        	Translate(Vector3.Up * 10 * delta);
	        }
	        if (Input.IsActionPressed("ui_page_down"))
	        {
	        	Translate(Vector3.Down * 10 * delta);
	        }
        } else {
        	if (Input.IsActionPressed("ui_up"))
	        {
	        	pitchGimbal.RotateX(-2 * delta);
	        }
	        if (Input.IsActionPressed("ui_down"))
	        {
	        	pitchGimbal.RotateX(2 * delta);
	        }
	        if (Input.IsActionPressed("ui_right"))
	        {
	        	yawGimbal.RotateY(2 * delta);
	        }
	        if (Input.IsActionPressed("ui_left"))
	        {
	        	yawGimbal.RotateY(-2 * delta);
	        }	
	        if (Input.IsActionPressed("ui_page_up"))
	        {
	        	Translate(Vector3.Forward * 10 * delta);
	        }
	        if (Input.IsActionPressed("ui_page_down"))
	        {
	        	Translate(Vector3.Back * 10 * delta);
	        }
        }
	}
}
