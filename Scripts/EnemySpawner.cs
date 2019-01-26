using Godot;
using System;

public class EnemySpawner : Node
{
    Timer timer;
    Random r;
    PackedScene enemyScene;

    public override void _Ready()
    {
        enemyScene = ResourceLoader.Load("Scenes/EnemyFighter.tscn") as PackedScene;

        r = new Random();
        timer = new Timer();
        timer.Connect("timeout", this, "OnSpawnTimer");
        timer.WaitTime = 2;
        AddChild(timer);
        timer.Start();
    }

    public void OnSpawnTimer() 
    {
        timer.WaitTime = r.Next(2,6);
        timer.Start();

        GD.Print("Spawn enemy");
        var enemy = enemyScene.Instance() as Spatial;
        GetParent().AddChild(enemy);
        var randomAngle = MainController.RandomFloat(0, Mathf.Pi * 2);
        enemy.Translation = MainController.I.mothership.GlobalTransform.origin + new Vector3(Mathf.Cos(randomAngle), MainController.RandomFloat(-1, 1), Mathf.Sin(randomAngle)) * 20;

        var AI = enemy.GetNode("AI") as ShipAI;
        AI.SetTarget(MainController.I.mothership);
    }
}
