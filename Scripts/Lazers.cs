using Godot;
using System;

public class Lazers : ImmediateGeometry
{

    Vector3 test = Vector3.Up;
    Quat q = new Quat(new Vector3(-Mathf.Pi / 2,0,0));
    float distance = 5;
    float size = 0.1f;

    public override void _Ready()
    {

    }

    private void AddVertexQ(Vector3 vector3)
    {
        vector3.x *= size;
        vector3.z *= size;
        vector3.y *= distance;
        AddVertex(q.Xform(vector3));
    }

    public void CreateLaser(Spatial target)
    {
        Clear();
        Begin(Mesh.PrimitiveType.Triangles, null);

        distance = GlobalTransform.origin.DistanceTo(target.GlobalTransform.origin);
        GD.Print(distance);
        
        AddVertexQ(new Vector3(-0.5f, 0, -0.5f));//2
        AddVertexQ(new Vector3(0.5f, 0, -0.5f));//1
        AddVertexQ(new Vector3(-0.5f, 1, -0.5f));//3
        
        AddVertexQ(new Vector3(-0.5f, 1, -0.5f));//3
        AddVertexQ(new Vector3(0.5f, 0, -0.5f));//1
        AddVertexQ(new Vector3(0.5f, 1, -0.5f));//4
        
        AddVertexQ(new Vector3(0.5f, 1, -0.5f));//4
        AddVertexQ(new Vector3(0.5f, 0, -0.5f));//1
        AddVertexQ(new Vector3(0, 0, 0.5f));//5
        
        AddVertexQ(new Vector3(0, 0, 0.5f));//5
        AddVertexQ(new Vector3(0, 1, 0.5f));//6
        AddVertexQ(new Vector3(0.5f, 1, -0.5f));//4
       
        AddVertexQ(new Vector3(0, 1, 0.5f));//6
        AddVertexQ(new Vector3(0, 0, 0.5f));//5
        AddVertexQ(new Vector3(-0.5f, 0, -0.5f));//2
        
        AddVertexQ(new Vector3(-0.5f, 0, -0.5f));//2
        AddVertexQ(new Vector3(-0.5f, 1, -0.5f));//3
        AddVertexQ(new Vector3(0, 1, 0.5f));//6
        
        End();

        GlobalTransform.SetLookAt(GlobalTransform.origin, target.GlobalTransform.origin, target.GlobalTransform.basis.z);
    }

    public void DestroyLaser()
    {
        Clear();
    }
}
