using Godot;

public class ShipMove : Spatial
{
    [Export]
    bool fullGraphicsRotation = false;

    [Export]
    float maxSpeed = 10f, acceleration = 1f;

    bool moving = false;
    Vector3 moveTarget;
    float speed = 0;

    public override void _Ready()
    {

    }

    public override void _PhysicsProcess(float delta)
    {
        // Called every frame. Delta is time since last frame.
        // Update game logic here.
        //var target = GetNode("../Target") as Spatial;
        //MoveTo(target.Transform.origin);

        if (moving) 
        {
            var dir = moveTarget - GlobalTransform.origin;

            float maxS = maxSpeed;

            float dotNorm = -GlobalTransform.basis.z.Dot(dir) / dir.Length();

            GD.Print(dotNorm);
            if (dotNorm < 0.5f)
                maxS = maxSpeed * 0.3f;

            speed = Mathf.Lerp(speed, maxS, acceleration * delta);


            //Forward movement
            Translate(new Vector3(0, 0, -1) * delta * speed);
                


            if (speed <= maxSpeed)
            {//Rotation
               
                var dirTransform = GlobalTransform.LookingAt(moveTarget, GlobalTransform.basis.y);
                var newRotation = new Quat(GlobalTransform.basis).Slerp(new Quat(dirTransform.basis), delta);

                SetTransform(new Transform(newRotation, GlobalTransform.origin));
            }
        }
    }

    public void MoveTo(Vector3 position) 
    {
        moveTarget = position;
        moving = true;
    }
}
