using Godot;
using System;

public class EnemySpawner : Node
{
    Timer timer;
    Random r;
    PackedScene enemyScene;
    
    [Export]
    Curve spawnDelay;

    [Export]
    float roundDuration = 60;
    [Export]
    float baseSpawnDelay = 10;

    int startTime;

    public override void _Ready()
    {
        enemyScene = ResourceLoader.Load("Scenes/EnemyFighter.tscn") as PackedScene;

        r = new Random();
        timer = new Timer();
        timer.Connect("timeout", this, "OnSpawnTimer");
        timer.WaitTime = 2;
        AddChild(timer);
        timer.Start();

        startTime = OS.GetSystemTimeSecs();
    }

    public void OnSpawnTimer()
    {
        if (MainController.I.mothership == null)
        {
            timer.Stop();
            return;
        }

        timer.WaitTime = spawnDelay.Interpolate((OS.GetSystemTimeSecs() - startTime) / roundDuration) * baseSpawnDelay;
        GD.Print( timer.WaitTime);
        
        timer.Start();

        GD.Print("Spawn enemy");
        var enemy = enemyScene.Instance() as KinematicBody;
        GetParent().AddChild(enemy);
        enemy.Translation = MainController.I.mothership.Translation + MainController.RandomPointOnSphere() * 20;
        MainController.I.enemies.Add((ShipMain)enemy);
        
        var AI = enemy.GetNode("AI") as ShipAI;
        AI.SetTarget(MainController.I.mothership);
    }
}
