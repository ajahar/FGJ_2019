using Godot;

public class ShipMain : KinematicBody
{
    [Export]
    bool fullGraphicsRotation = false;

    [Export]
    float maxSpeed = 10f, acceleration = 1f;

    bool moving = false;
    Vector3 moveTarget;
    float speed = 0;
    
    public ShipAI ai;
    
    public override void _Ready()
    {
    	ai = GetNodeOrNull<ShipAI>("AI");
    }

    public override void _PhysicsProcess(float delta)
    {
        if (moving)
        {
            float maxS = maxSpeed;

            var dot = GlobalTransform.basis.z.Normalized().Dot((GlobalTransform.origin - moveTarget).Normalized());

            if (dot < 0.5f)
                maxS = maxSpeed * 0.3f;

            speed = Mathf.Lerp(speed, maxS, acceleration * delta);

            //Forward movement
            Translate(new Vector3(0, 0, -1) * delta * speed);

            if (speed <= maxSpeed)
            {//Rotation
                var dirTransform = GlobalTransform.LookingAt(moveTarget, GlobalTransform.basis.y);
                var newRotation = new Quat(GlobalTransform.basis).Slerp(new Quat(dirTransform.basis), delta);

                SetGlobalTransform(new Transform(newRotation, GlobalTransform.origin));
            }
        }
    }

    public void MoveTo(Vector3 position) 
    {
        moveTarget = position;
        moving = true;
    }

    public void Destroy() 
    {
        QueueFree();
     }
}
