using Godot;
using System;

public class Lazers : ImmediateGeometry
{

    Vector3 test = Vector3.Up;
    Quat q;
    float distance = 5;
    float size = 0.1f;

    public override void _Ready()
    {
        var other = GetNode("../MeshInstance") as Spatial;
        
        q = new Quat(other.Transform.basis);

        
    }

    private void AddVertexQ(Vector3 vector3)
    {
        vector3.x *= size;
        vector3.z *= size;
        vector3.y *= distance;
        AddVertex(q.Xform(vector3));
        
    }

    public override void _Process(float delta)
    {
        Clear();
        Begin(Mesh.PrimitiveType.Triangles, null);
        //SetColor(new Color(256f, 0f,0f, 1f));
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
    }
}
